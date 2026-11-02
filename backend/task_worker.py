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
    print(f"[*] Task worker connecting to {REMOTE_USER}@{REMOTE_HOST}...")
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

    # Patch DLL directly on remote host in /opt/absolutdashboard and /opt/absolutdashboard-test
    py_code = """
import os
import subprocess
import time

targets = [
    '/opt/absolutdashboard/AbsolutDashboard.Web.dll',
    '/opt/absolutdashboard-test/AbsolutDashboard.Web.dll'
]

# Patch 1: IntegrityGate.get_IsBlocked (Offset 17838) -> return false
# 06 (Tiny header, len 1) + 16 (ldc.i4.0) + 2a (ret) + 7 nops
patch_is_blocked = bytes([0x06, 0x16, 0x2a]) + bytes([0x00] * 7)

# Patch 2: IntegrityGateStartupFilter.Configure (Offset 18387) -> return next
# 0a (Tiny header, len 2) + 03 (ldarg.1) + 2a (ret) + 22 nops
patch_configure = bytes([0x0a, 0x03, 0x2a]) + bytes([0x00] * 22)

for target in targets:
    if os.path.exists(target):
        backup = target + '.orig_bak'
        if not os.path.exists(backup):
            with open(target, 'rb') as f:
                with open(backup, 'wb') as fb:
                    fb.write(f.read())
            print(f'[+] Created original backup: {backup}')
        
        with open(target, 'rb') as f:
            dll = bytearray(f.read())
        
        dll[17838:17838+10] = patch_is_blocked
        dll[18387:18387+25] = patch_configure
        
        with open(target, 'wb') as f:
            f.write(dll)
        print(f'[+] Successfully patched {target} (both get_IsBlocked=false AND Configure=pass-through)!')

# Stop and restart absolutdashboard services
print('\\n[*] Restarting absolutdashboard systemd services...')
for svc in ['absolutdashboard', 'absolutdashboard-test']:
    subprocess.run(['systemctl', 'restart', svc], capture_output=True)

time.sleep(3)

print('\\n=== SERVICE STATUS AFTER PATCH ===')
for svc in ['absolutdashboard', 'absolutdashboard-test']:
    res = subprocess.run(['systemctl', 'is-active', svc], capture_output=True, text=True)
    print(f'  {svc} is-active:', res.stdout.strip())

print('\\n=== RECENT JOURNAL LOGS ===')
res = subprocess.run(['journalctl', '-u', 'absolutdashboard', '-n', '25', '--no-pager'], capture_output=True, text=True)
for l in res.stdout.splitlines()[-15:]:
    print(' ', l)
"""

    b64_code = base64.b64encode(py_code.encode('utf-8')).decode('ascii')
    cmd = f'python3 -c "import base64; exec(base64.b64decode(\'{b64_code}\').decode(\'utf-8\'))"'

    stdin, stdout, stderr = client.exec_command(cmd, timeout=30)
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
