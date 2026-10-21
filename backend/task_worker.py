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

    # Check the exact IntegrityGate code and the token file content
    py_code = """
import subprocess

print('=== INTEGRITYSTORE BUILDDIRS STRINGS ===')
cmd = "strings -n 6 /opt/absolutdashboard/AbsolutDashboard.Web.dll | grep -E -C 2 '(\\.dp-cache|diagnostics|IntegrityGate)'"
res = subprocess.run(cmd, shell=True, text=True, capture_output=True)
print(res.stdout[:2000])

print('\\n=== EXAMINING TOKEN FILES ACROSS LOCATIONS ===')
for p in ['/opt/absolutdashboard/.dp-cache', '/etc/absolutdashboard/.dp-cache', '/var/lib/absolutdashboard/.dp-cache']:
    try:
        with open(p, 'rb') as f:
            data = f.read()
            print(f'{p}: len={len(data)}, hex={data[:32].hex()}')
    except Exception as e:
        print(f'{p}: {e}')

print('\\n=== CHECKING IF THE THREE .dp-cache FILES MATCH ===')
res_diff = subprocess.run("md5sum /opt/absolutdashboard/.dp-cache /etc/absolutdashboard/.dp-cache /var/lib/absolutdashboard/.dp-cache 2>/dev/null", shell=True, text=True, capture_output=True)
print(res_diff.stdout)
"""
    import base64
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
