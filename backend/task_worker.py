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
import subprocess
import os

print("=== 1. FIND THE ACTIVE DB USED BY PID 1114 ===")
p_fds = subprocess.run(['ls', '-l', '/proc/1114/fd/'], capture_output=True, text=True)
for line in p_fds.stdout.splitlines():
    if '.db' in line or '.sqlite' in line:
        print("  FD DB:", line)

print("=== 2. ALL DB FILES ON SERVER & SIZES ===")
find_dbs = subprocess.run(['find', '/opt', '/var', '/root', '-name', '*.db', '-o', '-name', '*.sqlite*'], capture_output=True, text=True)
for db_f in find_dbs.stdout.splitlines():
    if os.path.exists(db_f):
        print(f"  {db_f}: {os.path.getsize(db_f)} bytes")

print("=== 3. NETWORK INTERFACES & ROUTES ON 10.20.110.7 ===")
ip_a = subprocess.run(['ip', 'addr'], capture_output=True, text=True)
print(ip_a.stdout)

ip_r = subprocess.run(['ip', 'route'], capture_output=True, text=True)
print("=== Routes ===")
print(ip_r.stdout)
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
