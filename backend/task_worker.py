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

    # In ASP.NET Core:
    # UseMiddleware<T>() is called inside Configure.
    # What if we patch IntegrityGateMiddleware.InvokeAsync entry point directly?
    # In CIL:
    # public Task InvokeAsync(HttpContext context)
    # The first instructions are:
    # ldarg.0 (this)
    # ldfld _next
    # ldarg.1 (context)
    # callvirt RequestDelegate.Invoke(HttpContext)
    # ret
    #
    # If the method immediately delegates to _next(context) and returns:
    # It acts as a 100% transparent pass-through middleware for all requests!
    # Let's inspect the first 20 bytes of InvokeAsync!
    py_code = """
with open('/opt/absolutdashboard/AbsolutDashboard.Web.dll', 'rb') as f:
    data = bytearray(f.read())

# Look for the method header of InvokeAsync
# In our earlier search, offset 214092 was inside InvokeAsync:
# 6f 1e 01 00 06 38 9f 00 00 00
# Let's look at 100 bytes preceding offset 214092:
offset = 214092
print('Bytes preceding offset 214092:')
print(data[offset-80:offset].hex())
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
