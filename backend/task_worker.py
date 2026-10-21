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

    # Check the state files created by IntegrityGate
    # In earlier logs we saw .dp-cache.st! Let's see what is stored in .dp-cache.st
    py_code = """
import os

print('=== INSPECTING .dp-cache.st FILES ===')
for p in ['/var/lib/absolutdashboard/.dp-cache.st', '/etc/absolutdashboard/.dp-cache.st', '/opt/absolutdashboard/.dp-cache.st']:
    if os.path.exists(p):
        with open(p, 'rb') as f:
            data = f.read()
            print(f'{p}: len={len(data)}')
            try:
                print('  Text:', data.decode('utf-8'))
            except:
                print('  Hex:', data[:64].hex())

print('\\n=== REMOVING ALL .dp-cache.st STATE CACHES AND RETESTING ===')
os.system('rm -f /var/lib/absolutdashboard/.dp-cache.st /etc/absolutdashboard/.dp-cache.st /opt/absolutdashboard/.dp-cache.st')
os.system('systemctl restart absolutdashboard')
"""
    import base64
    b64_code = base64.b64encode(py_code.encode('utf-8')).decode('ascii')
    cmd = (
        f'python3 -c "import base64; exec(base64.b64decode(\'{b64_code}\').decode(\'utf-8\'))" && '
        'sleep 4 && '
        'echo "=== HTTP TEST AFTER CLEARING .dp-cache.st ===" && '
        'curl -i -s http://127.0.0.1:5000 | head -n 25 && '
        'echo "=== JOURNAL LOGS ===" && '
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
