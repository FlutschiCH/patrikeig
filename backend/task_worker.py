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

    # Let's inspect where IntegrityGateStartupFilter is registered or where its Configure method is located.
    # In .NET metadata, strings are in the #Strings heap. Method names, Type names, etc.
    # Look at line 11:
    # "tegrityStore._store.Configure.ITrackingConsentFeature.Micros" at 697883!
    # Configure is right next to IntegrityStore!
    # Let's see what methods IntegrityGate has and what methods IntegrityGateMiddleware has.
    py_code = """
with open('/opt/absolutdashboard/AbsolutDashboard.Web.dll.bak', 'rb') as f:
    orig = f.read()

# Let's look at all strings around 697883:
s_start = 697500
s_end = 698500
print("Strings chunk:")
raw = orig[s_start:s_end]
for part in raw.split(b'\\x00'):
    if part and len(part) > 1:
        try:
            print(" ", part.decode('utf-8'))
        except:
            pass

# Also let's inspect IntegrityGate methods!
# Search for 'Validate' or 'Check' or 'Gate' or 'IsAuthorized' or 'Verify'
for word in [b'IntegrityGate', b'IntegrityCheck', b'IntegrityService', b'Validate', b'IsValid', b'CheckIntegrity']:
    pos = 0
    cnt = 0
    while True:
        pos = orig.find(word, pos)
        if pos == -1 or cnt > 5:
            break
        print(f"Found {word.decode()} at {pos}")
        pos += len(word)
        cnt += 1
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
