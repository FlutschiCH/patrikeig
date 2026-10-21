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

    # Install monodis / ilspy or use a python disassembler to view IntegrityGateMiddleware
    py_code = """
import subprocess

# Check if dotnet is installed
res = subprocess.run("dotnet --info || which dotnet", shell=True, text=True, capture_output=True)
print('Dotnet runtime:', res.stdout[:500])

# Disassemble IntegrityGate methods using Python pefile or dnlib or simple string/byte scan
with open('/opt/absolutdashboard/AbsolutDashboard.Web.dll', 'rb') as f:
    data = f.read()

idx = data.find(b'IntegrityGateMiddleware')
print('IntegrityGateMiddleware offset:', idx)

# Extract nearby strings (UTF-16LE / ASCII)
import re
print('\\n=== STRINGS NEAR INTEGRITYGATE ===')
chunk = data[max(0, idx-2000):min(len(data), idx+4000)]
strings = re.findall(b'[\x20-\x7e]{4,}', chunk)
for s in strings[:40]:
    print(' ', s.decode('ascii', 'ignore'))
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
