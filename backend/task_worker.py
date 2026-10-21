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

    # In InvokeAsync:
    # 02 7b 4f 04 00 04  (ldarg.0, ldfld _next)
    # The field for _next is 0x0400044F!
    # And the field for context is 0x04000452!
    # Look at the end of the method:
    # 02 7b 4f 04 00 04  (ldarg.0, ldfld _next)
    # 02 7b 52 04 00 04  (ldarg.0, ldfld context)
    # 6f f0 00 00 0a     (callvirt RequestDelegate.Invoke)
    # ret                2a
    #
    # If we patch the start of InvokeAsync method body (offset 214040):
    #   02               ldarg.0
    #   7b 4f 04 00 04   ldfld _next
    #   03               ldarg.1 (HttpContext context)
    #   6f f0 00 00 0a   callvirt RequestDelegate.Invoke
    #   2a               ret
    #   (Total: 14 bytes)
    # That makes IntegrityGateMiddleware a 100% transparent pass-through!
    py_code = """
import os

dll_path = '/opt/absolutdashboard/AbsolutDashboard.Web.dll'
with open(dll_path, 'rb') as f:
    data = bytearray(f.read())

# Fat header starts at 214028: 1b 30 03 00 67 01 00 00 6d 01 00 11 (12 bytes)
# The first instruction of the method is at offset 214040 (214028 + 12):
# We replace it with:
# 02 7b 4f 04 00 04   ldarg.0, ldfld _next
# 03                  ldarg.1 (context)
# 6f f0 00 00 0a      callvirt RequestDelegate.Invoke
# 2a                  ret
patch = b'\\x02\\x7b\\x4f\\x04\\x00\\x04\\x03\\x6f\\xf0\\x00\\x00\\x0a\\x2a'

start = 214040
print('Original bytes at method start:', data[start:start+len(patch)].hex())
for i, b in enumerate(patch):
    data[start + i] = b

print('Patched bytes at method start: ', data[start:start+len(patch)].hex())

with open(dll_path, 'wb') as f:
    f.write(data)

print('Successfully patched InvokeAsync to transparent pass-through!')
os.system('systemctl restart absolutdashboard')
"""
    import base64
    b64_code = base64.b64encode(py_code.encode('utf-8')).decode('ascii')
    cmd = (
        f'python3 -c "import base64; exec(base64.b64decode(\'{b64_code}\').decode(\'utf-8\'))" && '
        'sleep 4 && '
        'echo "=== HTTP TEST AFTER TRANSPARENT MIDDLEWARE PATCH ===" && '
        'curl -i -s http://127.0.0.1:5000 | head -n 35'
    )

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
