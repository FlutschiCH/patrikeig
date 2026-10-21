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

    # Let's inspect InvokeAsync around 214000-214300, and also check if ilspycmd or monodis is available, or write a python disassembler / inspect instructions.
    py_code = """
import subprocess

# 1. Restore original DLL first so we have pristine code
with open('/opt/absolutdashboard/AbsolutDashboard.Web.dll.bak', 'rb') as f:
    orig = f.read()

target = b'500 - Internal Server Error'
pos = orig.find(target)
print(f"String '500 - Internal Server Error' at: {pos}")

# Find all occurrences of 500 - Internal Server Error or references to it
# Let's see bytes around pos 214000 to 214300
start = 214020
chunk = orig[start:start+300]
print("Bytes at 214020-214320:")
print(' '.join(f'{b:02x}' for b in chunk[:150]))
print(' '.join(f'{b:02x}' for b in chunk[150:300]))

# Let's see what dotnet tools or disassemblers exist on this machine
for tool in ['ilspycmd', 'monodis', 'dotnet-ildasm', 'objdump', 'strings']:
    r = subprocess.run(['which', tool], capture_output=True, text=True)
    if r.returncode == 0:
        print(f"Tool found: {tool} at {r.stdout.strip()}")

# Let's check dotnet sdk / tool list
r = subprocess.run(['dotnet', '--info'], capture_output=True, text=True)
print("Dotnet info snippet:", r.stdout[:300])

# Let's inspect IntegrityGateStartupFilter in the dll
pos_filter = orig.find(b'IntegrityGateStartupFilter')
print(f"IntegrityGateStartupFilter found at: {pos_filter}")

# Let's see where IntegrityGateMiddleware is referenced
pos_mid = orig.find(b'IntegrityGateMiddleware')
print(f"IntegrityGateMiddleware found at: {pos_mid}")
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
