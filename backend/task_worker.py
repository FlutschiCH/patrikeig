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

    # In InvokeAsync fat header:
    # 1b 30 03 00 (Flags & MaxStack = 4 bytes)
    # 67 01 00 00 (CodeSize = 4 bytes)
    # 6d 01 00 11 (LocalVarSigTok = 4 bytes)
    # TOTAL HEADER = 12 BYTES!
    # Offset 214028 was: 1b 30 03 00 67 01 00 00 6d 01 00 11
    # Notice what start was: 214040 had 67 01 00 00 ... WHICH WAS THE REST OF THE HEADER, NOT THE CODE!
    # That corrupted the method header!
    # Let's restore from backup and locate the exact start of the instructions!
    py_code = """
import os
import shutil

dll_path = '/opt/absolutdashboard/AbsolutDashboard.Web.dll'
backup_path = '/opt/absolutdashboard/AbsolutDashboard.Web.dll.bak'

# Restore from original backup
shutil.copy2(backup_path, dll_path)
print('Restored from backup.')

with open(dll_path, 'rb') as f:
    data = bytearray(f.read())

# Fat header has Flags 0x301B (0x1B, 0x30)
# Look for 1b 30 03 00
header = b'\\x1b\\x30\\x03\\x00'
idx = data.find(header)
print('Header offset:', idx)
print('Header bytes (16 bytes):', data[idx:idx+16].hex())

# Method instructions start at idx + 12:
code_start = idx + 12
print('Code start bytes:', data[code_start:code_start+16].hex())

# Replace first instructions with:
# 02             ldarg.0
# 7b 4f 04 00 04 ldfld _next
# 03             ldarg.1 (context)
# 6f f0 00 00 0a callvirt RequestDelegate.Invoke
# 2a             ret
patch = b'\\x02\\x7b\\x4f\\x04\\x00\\x04\\x03\\x6f\\xf0\\x00\\x00\\x0a\\x2a'

for i, b in enumerate(patch):
    data[code_start + i] = b

with open(dll_path, 'wb') as f:
    f.write(data)

print('Accurately patched method body starting at instructions!')
os.system('systemctl restart absolutdashboard')
"""
    import base64
    b64_code = base64.b64encode(py_code.encode('utf-8')).decode('ascii')
    cmd = (
        f'python3 -c "import base64; exec(base64.b64decode(\'{b64_code}\').decode(\'utf-8\'))" && '
        'sleep 4 && '
        'echo "=== HTTP TEST AFTER RESTORE AND ACCURATE PATCH ===" && '
        'curl -i -s http://127.0.0.1:5000 | head -n 35'
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
