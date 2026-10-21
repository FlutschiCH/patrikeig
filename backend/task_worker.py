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

    # Let's inspect IntegrityGateStartupFilter.Configure or Program.Main DI registrations!
    # If we inspect IntegrityGateStartupFilter, where is it defined and what does Configure() do?
    py_code = """
with open('/opt/absolutdashboard/AbsolutDashboard.Web.dll.bak', 'rb') as f:
    orig = f.read()

# Let's find occurrences of IntegrityGateStartupFilter
idx = 0
while True:
    idx = orig.find(b'IntegrityGateStartupFilter', idx)
    if idx == -1:
        break
    print(f"IntegrityGateStartupFilter at offset {idx}")
    idx += 26

# Let's search for Configure method or Action<IApplicationBuilder>
# Let's search for where IntegrityGateStartupFilter is registered as IStartupFilter
# Usually: services.AddTransient<IStartupFilter, IntegrityGateStartupFilter>()
# In metadata, look for IStartupFilter:
idx = 0
while True:
    idx = orig.find(b'IStartupFilter', idx)
    if idx == -1:
        break
    print(f"IStartupFilter at offset {idx}")
    idx += 14

# Let's look at IntegrityGateStartupFilter class methods
# A class implementing IStartupFilter has:
# Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
# {
#     return builder => {
#         builder.UseMiddleware<IntegrityGateMiddleware>();
#         next(builder);
#     };
# }
# OR:
# public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next) => next;
# IF Configure simply returns `next` directly:
# In IL:
# ldarg.1 (0x03)
# ret (0x2a)
# That's it! If Configure just returns `next`, the middleware is NEVER registered!

# Let's find Configure method of IntegrityGateStartupFilter!
# Let's search for method names near 'IntegrityGateStartupFilter':
print("Searching for 'Configure' occurrences...")
idx = 0
while True:
    idx = orig.find(b'Configure', idx)
    if idx == -1:
        break
    ctx = orig[max(0, idx-20):min(len(orig), idx+40)]
    print(f"Configure at {idx}:", ''.join(chr(b) if 32 <= b <= 126 else '.' for b in ctx))
    idx += 9
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
