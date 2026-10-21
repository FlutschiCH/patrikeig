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

    # Check the exact IntegrityGate code in the binary using UTF-16LE / insensitive search
    py_code = """
import re

with open('/opt/absolutdashboard/AbsolutDashboard.Web.dll', 'rb') as f:
    data = f.read()

# Let's find IntegrityGate class definition and methods
idx = data.find(b'IntegrityGate')
print('IntegrityGate at:', idx)

# Check what methods exist in IntegrityGate
methods = re.findall(rb'[A-Za-z0-9_]{3,30}', data[idx:idx+800])
print('Methods/Tokens in IntegrityGate:')
for m in methods[:25]:
    print(' ', m.decode('ascii', 'ignore'))

# Look for any text returned on 500 error
print('\\n=== ALL OCCURRENCES OF 500 ===')
for m in re.finditer(rb'500', data):
    pos = m.start()
    snippet = data[max(0, pos-20):min(len(data), pos+30)]
    print('  500 snippet:', snippet)
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
