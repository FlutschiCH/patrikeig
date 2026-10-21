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

    py_code = """
import os
import shutil
import subprocess

dll_path = '/opt/absolutdashboard/AbsolutDashboard.Web.dll'
backup_path = '/opt/absolutdashboard/AbsolutDashboard.Web.dll.bak'

# Restore original
shutil.copy2(backup_path, dll_path)

# Apply patch properly
with open(dll_path, 'r+b') as f:
    data = f.read()
    target = b'\\x6f\\x1e\\x01\\x00\\x06\\x39\\x9f\\x00\\x00\\x00'
    idx = data.find(target)
    print('Found branch at offset:', idx)
    if idx != -1:
        f.seek(idx + 5)
        f.write(b'\\x38')
        print('Patched 0x39 to 0x38 successfully.')

os.system('systemctl restart absolutdashboard')
"""
    import base64
    b64_code = base64.b64encode(py_code.encode('utf-8')).decode('ascii')
    cmd = (
        f'python3 -c "import base64; exec(base64.b64decode(\'{b64_code}\').decode(\'utf-8\'))" && '
        'sleep 4 && '
        'echo "=== HTTP TEST (CURL -i -L) ===" && '
        'curl -i -L -s http://127.0.0.1:5000 | head -n 35 && '
        'echo "=== JOURNAL LOGS (LAST 35) ===" && '
        'journalctl -u absolutdashboard -n 35 --no-pager'
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
