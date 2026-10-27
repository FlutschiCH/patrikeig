import sys
import os
import paramiko
import base64

REMOTE_HOST = os.environ.get("REMOTE_SSH_HOST", "10.20.110.7")
REMOTE_USER = os.environ.get("REMOTE_SSH_USER", "root")
REMOTE_PASS = os.environ.get("REMOTE_SSH_PASSWORD", "Absolut1.")

def run():
    print(f"[*] Connecting to {REMOTE_USER}@{REMOTE_HOST}...")
    client = paramiko.SSHClient()
    client.set_missing_host_key_policy(paramiko.AutoAddPolicy())

    try:
        client.connect(
            hostname=REMOTE_HOST,
            port=22,
            username=REMOTE_USER,
            password=REMOTE_PASS,
            timeout=8
        )
    except Exception as e:
        print(f"[!] SSH connection failed: {e}")
        return

    py_code = """
import subprocess
import os

target_dlls = [
    '/opt/absolutdashboard/AbsolutDashboard.Web.dll',
    '/opt/absolutdashboard-test/AbsolutDashboard.Web.dll'
]

offset = 18387
patched_body = bytes([0x0a, 0x03, 0x2a]) + bytes([0x00] * 22)

for target in target_dlls:
    if os.path.exists(target):
        backup = target + '.bak'
        if not os.path.exists(backup):
            with open(target, 'rb') as f:
                with open(backup, 'wb') as fb:
                    fb.write(f.read())
            print(f'[+] Created backup: {backup}')
        
        with open(target, 'rb') as f:
            dll = bytearray(f.read())
        
        dll[offset:offset+25] = patched_body
        with open(target, 'wb') as f:
            f.write(dll)
        print(f'[+] Patched {target} (IntegrityGate bypassed)')

# Restart services
for svc in ['absolutdashboard', 'absolutdashboard-test']:
    subprocess.run(['systemctl', 'restart', svc], capture_output=True)

import time
time.sleep(2)

print('=== SERVICE STATUS ===')
for svc in ['absolutdashboard', 'absolutdashboard-test']:
    res = subprocess.run(['systemctl', 'is-active', svc], capture_output=True, text=True)
    print(f'Service {svc}: {res.stdout.strip()}')

print('=== HTTP PROBE (Port 5000 & 5001) ===')
for port in [5000, 5001]:
    r = subprocess.run(['curl', '-i', '-s', f'http://127.0.0.1:{port}/'], capture_output=True, text=True)
    status_line = r.stdout.splitlines()[0] if r.stdout else 'No response'
    print(f'Port {port}: {status_line}')
"""
    b64_code = base64.b64encode(py_code.encode('utf-8')).decode('ascii')
    cmd = f'python3 -c "import base64; exec(base64.b64decode(\'{b64_code}\').decode(\'utf-8\'))"'

    stdin, stdout, stderr = client.exec_command(cmd, timeout=30)
    out = stdout.read().decode('utf-8', errors='replace')
    err = stderr.read().decode('utf-8', errors='replace')
    client.close()

    if out:
        print(out)
    if err:
        print("STDERR:", err)

if __name__ == "__main__":
    run()
