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

    # Check what causes Kestrel to return 500 when calling http://127.0.0.1:5000/
    # Let's test calling other paths: /login, /setup, /api, /_blazor, /health
    py_code = """
import urllib.request
import urllib.error

urls = [
    'http://127.0.0.1:5000/',
    'http://127.0.0.1:5000/login',
    'http://127.0.0.1:5000/setup',
    'http://127.0.0.1:5000/api',
    'http://127.0.0.1:5000/favicon.png',
    'http://127.0.0.1:5000/app.css'
]

print('=== TESTING MULTIPLE ENDPOINTS ===')
for u in urls:
    try:
        req = urllib.request.urlopen(u, timeout=3)
        print(f'{u:35}: Status {req.status}, len={len(req.read())}')
    except urllib.error.HTTPError as e:
        print(f'{u:35}: HTTPError {e.code} - {e.read().decode("utf-8", errors="replace").strip()}')
    except Exception as e:
        print(f'{u:35}: Error ({e})')
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
