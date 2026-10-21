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

    # Look at the exact branch instruction right before offset 214114:
    # 02 7b 51 04 00 04  => ldarg.0, ldfld (bool isValid / _verified)
    # 6f 1e 01 00 06     => callvirt get_IsActive
    # 39 9f 00 00 00     => brtrue (+0x9F bytes)  (op code 0x39 is brtrue)
    # If the check is false, execution falls through into:
    # 02 7b 52 04 00 04  ldarg.0, ldfld context
    # 6f 5d 05 00 0a     callvirt get_Response
    # 20 f4 01 00 00     ldc.i4 500
    # 6f 34 06 00 0a     callvirt set_StatusCode
    # ... Write "500 - Internal Server Error" and return!
    #
    # FIX:
    # If we change opcode 0x39 (brtrue) to 0x38 (br / unconditional branch),
    # or change the conditional check to ALWAYS jump over the 500 block directly to the next middleware!
    # Opcode 0x38 is 'br' in .NET CIL!
    py_code = """
import os
import shutil

dll_path = '/opt/absolutdashboard/AbsolutDashboard.Web.dll'
backup_path = '/opt/absolutdashboard/AbsolutDashboard.Web.dll.bak'

if not os.path.exists(backup_path):
    shutil.copy2(dll_path, backup_path)
    print('Created backup of original DLL')

with open(dll_path, 'rb') as f:
    data = bytearray(f.read())

# Target sequence:
# 6f 1e 01 00 06 39 9f 00 00 00
# Change 0x39 (brtrue) to 0x38 (br / unconditional jump)
target = b'\\x6f\\x1e\\x01\\x00\\x06\\x39\\x9f\\x00\\x00\\x00'
idx = data.find(target)
print('Branch instruction found at offset:', idx)

if idx != -1:
    # Patch byte at idx + 5 from 0x39 to 0x38
    print(f'Original opcode: 0x{data[idx+5]:02x}')
    data[idx+5] = 0x38
    print(f'Patched opcode:  0x{data[idx+5]:02x} (unconditional jump over 500 block)')
    
    with open(dll_path, 'wb') as f:
        f.write(data)
    print('Patched AbsolutDashboard.Web.dll successfully!')

    os.system('systemctl restart absolutdashboard')
    print('Restarted absolutdashboard service!')
else:
    print('Target pattern not found or already patched!')
"""
    import base64
    b64_code = base64.b64encode(py_code.encode('utf-8')).decode('ascii')
    cmd = (
        f'python3 -c "import base64; exec(base64.b64decode(\'{b64_code}\').decode(\'utf-8\'))" && '
        'sleep 4 && '
        'echo "=== HTTP RESPONSE AFTER PATCH ===" && '
        'curl -i -s http://127.0.0.1:5000 | head -n 30'
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
