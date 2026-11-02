import os
import sys
import paramiko
import base64

if sys.platform == "win32":
    sys.stdout.reconfigure(encoding="utf-8", errors="replace")
    sys.stderr.reconfigure(encoding="utf-8", errors="replace")

REMOTE_HOST = os.environ.get("REMOTE_SSH_HOST", "10.20.110.7")
REMOTE_USER = os.environ.get("REMOTE_SSH_USER", "root")
REMOTE_PASS = os.environ.get("REMOTE_SSH_PASSWORD", "Absolut1.")

def run():
    client = paramiko.SSHClient()
    client.set_missing_host_key_policy(paramiko.AutoAddPolicy())
    try:
        client.connect(hostname=REMOTE_HOST, port=22, username=REMOTE_USER, password=REMOTE_PASS, timeout=8)
    except Exception as e:
        print(f"[!] SSH connection failed: {e}")
        return

    py_code = """
import os
import subprocess
import time

targets = [
    '/opt/absolutdashboard/AbsolutDashboard.Web.dll',
    '/opt/absolutdashboard-test/AbsolutDashboard.Web.dll'
]

# Valid ECMA-335 tiny method header:
# Code size = 9 (header byte = (9 << 2) | 2 = 0x26)
# 7 nops (0x00) + ldc.i4.0 (0x16) + ret (0x2a)
valid_is_blocked = bytes([0x26] + [0x00] * 7 + [0x16, 0x2a])

# Valid Configure: code size = 2 (header byte = (2 << 2) | 2 = 0x0a)
# ldarg.1 (0x03) + ret (0x2a) + 22 nops
valid_configure = bytes([0x0a, 0x03, 0x2a]) + bytes([0x00] * 22)

for target in targets:
    if os.path.exists(target):
        with open(target, 'rb') as f:
            dll = bytearray(f.read())
        dll[17838:17838+10] = valid_is_blocked
        dll[18387:18387+25] = valid_configure
        with open(target, 'wb') as f:
            f.write(dll)
        print(f'[+] Applied clean ECMA-335 IL patch to {target}')

print('[*] Restarting absolutdashboard...')
subprocess.run(['systemctl', 'restart', 'absolutdashboard'], capture_output=True)
time.sleep(3)

print('=== SERVICE STATUS ===')
res = subprocess.run(['systemctl', 'is-active', 'absolutdashboard'], capture_output=True, text=True)
print('is-active:', res.stdout.strip())

print('=== RECENT JOURNAL LOGS ===')
res = subprocess.run(['journalctl', '-u', 'absolutdashboard', '-n', '25', '--no-pager'], capture_output=True, text=True)
for l in res.stdout.splitlines()[-15:]:
    print(' ', l)
"""

    b64_code = base64.b64encode(py_code.encode('utf-8')).decode('ascii')
    cmd = f'python3 -c "import base64; exec(base64.b64decode(\'{b64_code}\').decode(\'utf-8\'))"'

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
