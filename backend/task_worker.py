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

    # Check the exact IntegrityGate failure reason and check if a simple backup + clean DB works
    py_code = """
import subprocess
import os

print('=== DUMPING INTEGRITYGATE CODE STRINGS ===')
res = subprocess.run("strings /opt/absolutdashboard/AbsolutDashboard.Web.dll | grep -C 3 'IntegrityGate'", shell=True, text=True, capture_output=True)
print(res.stdout[:1500])

print('\\n=== TESTING CLEAN RE-INIT OF APP.DB (TEST INSTANCE) ===')
# If the issue is IntegrityGate foreign key check during pool initialization,
# let's see what happens on the clean test DB:
res_test = subprocess.run("curl -i -s http://127.0.0.1:5001", shell=True, text=True, capture_output=True)
print('Test instance port 5001 headers:', res_test.stdout[:300])

print('\\n=== EXECUTING REQUEST DIRECTLY TO CAPTURE CALLSTACK ===')
res_log = subprocess.run("journalctl -u absolutdashboard -g 'IntegrityGate' -n 5 --no-pager", shell=True, text=True, capture_output=True)
print(res_log.stdout)
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
