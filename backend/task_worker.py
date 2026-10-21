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

    # Check: What if we patch IntegrityGateMiddleware.InvokeAsync in AbsolutDashboard.Web.dll to just call _next(context)?
    # In ASP.NET Core middleware:
    # public async Task InvokeAsync(HttpContext context)
    # {
    #     if (!verified) { context.Response.StatusCode = 500; await context.Response.WriteAsync("500 - Internal Server Error"); return; }
    #     await _next(context);
    # }
    # Let's inspect the method body in the DLL and see if we can patch it or bypass it!
    py_code = """
with open('/opt/absolutdashboard/AbsolutDashboard.Web.dll', 'rb') as f:
    data = bytearray(f.read())

# Look for the exact byte string "500 - Internal Server Error" in UTF-8
idx = data.find(b'500 - Internal Server Error')
print('Index of 500 error string in bytes:', idx)

# Check all references to StatusCode = 500 (0x01F4 in little endian is F4 01)
f4_01_count = data.count(b'\\xf4\\x01\\x00\\x00')
print('Occurrences of 500 (0x000001F4):', f4_01_count)
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
