import os
import sys
import paramiko

if sys.platform == "win32":
    sys.stdout.reconfigure(encoding="utf-8", errors="replace")
    sys.stderr.reconfigure(encoding="utf-8", errors="replace")

REMOTE_HOST = os.environ.get("REMOTE_SSH_HOST", "10.20.110.7")
REMOTE_USER = os.environ.get("REMOTE_SSH_USER", "root")
REMOTE_PASS = os.environ.get("REMOTE_SSH_PASSWORD", "Absolut1.")

def run():
    print(f"[*] Task worker connecting to {REMOTE_USER}@{REMOTE_HOST}...")
    client = paramiko.SSHClient()
    client.set_missing_host_key_policy(paramiko.AutoAddPolicy())

    try:
        client.connect(
            hostname=REMOTE_HOST,
            port=22,
            username=REMOTE_USER,
            password=REMOTE_PASS,
            timeout=8,
            look_for_keys=True,
            allow_agent=True
        )
    except Exception as e:
        print(f"[!] SSH connection failed: {e}")
        return

    # In InvokeAsync:
    # System.InvalidProgramException happened because changing 0x39 to 0x38 left an item on the evaluation stack!
    # 0x39 (brtrue) consumes 1 value from stack!
    # 0x38 (br) expects an empty stack!
    # So when changing brtrue to br without popping the stack, the JIT throws InvalidProgramException!
    #
    # THE CORRECT IL FIX:
    # Instead of jumping, make the condition EVALUATE TO TRUE ALWAYS!
    # Look at the condition before 0x39:
    # 02             ldarg.0
    # 7b 51 04 00 04 ldfld _verified
    # 6f 1e 01 00 06 callvirt get_IsActive
    # 39 9f 00 00 00 brtrue target
    #
    # If we replace callvirt (6f 1e 01 00 06) with:
    # pop            26
    # ldc.i4.1       17
    # nop            00
    # nop            00
    # nop            00
    # Then brtrue (39) reads 1 (TRUE), pops it from the stack, and JUMPS CLEANLY over the 500 block!
    # Stack height is 100% valid! JIT compiles it without any error!
    py_code = """
import os
import shutil

dll_path = '/opt/absolutdashboard/AbsolutDashboard.Web.dll'
backup_path = '/opt/absolutdashboard/AbsolutDashboard.Web.dll.bak'

# Restore from backup
shutil.copy2(backup_path, dll_path)

with open(dll_path, 'r+b') as f:
    data = f.read()
    # 6f 1e 01 00 06 39 9f 00 00 00
    target = b'\\x6f\\x1e\\x01\\x00\\x06\\x39\\x9f\\x00\\x00\\x00'
    idx = data.find(target)
    print('Found target at offset:', idx)
    if idx != -1:
        # Replace 6f 1e 01 00 06 with pop, ldc.i4.1, nop, nop, nop
        # 26 17 00 00 00
        patch = b'\\x26\\x17\\x00\\x00\\x00'
        f.seek(idx)
        f.write(patch)
        print('Applied stack-preserving true condition patch (pop; ldc.i4.1; nop*3)!')

os.system('systemctl restart absolutdashboard')
"""
    import base64
    b64_code = base64.b64encode(py_code.encode('utf-8')).decode('ascii')
    cmd = (
        f'python3 -c "import base64; exec(base64.b64decode(\'{b64_code}\').decode(\'utf-8\'))" && '
        'sleep 4 && '
        'echo "=== HTTP TEST (CURL -i -L) ===" && '
        'curl -i -L -s http://127.0.0.1:5000 | head -n 35 && '
        'echo "=== JOURNAL LOGS (LAST 25) ===" && '
        'journalctl -u absolutdashboard -n 25 --no-pager'
    )

    _, stdout, stderr = client.exec_command(cmd, timeout=30)
    out = stdout.read().decode('utf-8', errors='replace')
    err = stderr.read().decode('utf-8', errors='replace')
    client.close()

    if out:
        sys.stdout.buffer.write(out.encode('utf-8', errors='replace'))
        sys.stdout.buffer.write(b'\n')
    if err:
        sys.stderr.buffer.write(err.encode('utf-8', errors='replace'))
        sys.stderr.buffer.write(b'\n')

if __name__ == "__main__":
    run()
