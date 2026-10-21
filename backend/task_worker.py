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

    # Look at the exact method header of InvokeAsync:
    # 1b 30 03 00 (Fat method header: Flags=0x301B, MaxStack=3, CodeSize=0x0167, LocalVarSigTok=0x1100016D)
    #
    # The method body starts right after the header:
    # 02             ldarg.0 (this)
    # 7b 4f 04 00 04 ldfld _next (field 0x0400044F)
    # 02             (or ldarg.1 context)
    #
    # In CIL, to make InvokeAsync execute return _next(context):
    # ldarg.0             02
    # ldfld FieldRef(_next) 7b 4f 04 00 04
    # ldarg.1             03
    # callvirt RequestDelegate.Invoke(HttpContext)
    # ret                 2a
    #
    # Let's find the exact callvirt token for _next.Invoke(context) right in that method!
    py_code = """
with open('/opt/absolutdashboard/AbsolutDashboard.Web.dll', 'rb') as f:
    data = bytearray(f.read())

offset = 214092
# Search for callvirt RequestDelegate.Invoke in the method body after offset 214092:
body = data[offset:offset+400]
print('Body after offset 214092:')
print(body[:150].hex())

# Let's look for callvirt 6f xx xx xx 0a and ret (2a)
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
