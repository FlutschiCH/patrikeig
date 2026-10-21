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

    # Look at offset 214114 in AbsolutDashboard.Web.dll:
    # 20 f4 01 00 00  => ldc.i4 500
    # 6f 34 06 00 0a  => callvirt set_StatusCode
    # 72 87 2a 02 70  => ldstr "text/plain"
    # 6f 35 06 00 0a  => callvirt set_ContentType
    # 72 9d 2a 02 70  => ldstr "500 - Internal Server Error"
    # (6f) WriteAsync!
    # THIS IS THE EXACT 500 GATE!
    py_code = """
with open('/opt/absolutdashboard/AbsolutDashboard.Web.dll', 'rb') as f:
    data = bytearray(f.read())

offset = 214114 # points to 20 f4 01 00 00 (ldc.i4 500)
# Let's inspect 50 bytes before and 100 bytes after offset 214114
print('Offset 214114 exact bytes:')
print(data[offset-30:offset+80].hex())

# In IL:
# If we replace the branch instruction that jumps to this block or NOP out the 500 block and call _next(context)
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
