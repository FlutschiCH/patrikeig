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

    # Use python file directly on remote without raw null bytes in regex string literal
    py_code = """
with open('/opt/absolutdashboard/AbsolutDashboard.Web.dll', 'rb') as f:
    data = f.read()

# Search for null-delimited ascii bytes
pattern = bytes([0])
words = []
for kw in [b'IntegrityGate', b'IntegrityStore', b'.dp-cache', b'diagnostics']:
    pos = 0
    while True:
        pos = data.find(kw, pos)
        if pos == -1:
            break
        # grab surrounding 200 bytes
        snippet = data[max(0, pos-100):min(len(data), pos+150)]
        printable = ''.join(chr(b) if 32 <= b <= 126 else ' ' for b in snippet)
        words.append(printable.strip())
        pos += len(kw)

print('=== OCCURRENCES OF INTEGRITY KEYWORDS ===')
for w in words[:15]:
    print('-->', w)
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
