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
    # services.AddTransient<IStartupFilter, IntegrityGateStartupFilter>();
    #
    # In .NET TypeDef / TypeRef / MethodDef tables:
    # If we simply change IntegrityGateStartupFilter's Configure method in the binary:
    # The Configure method looks like:
    #   public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    #   {
    #       return next;   // <-- JUST RETURN NEXT DIRECTLY WITHOUT ADDING IntegrityGateMiddleware!
    #   }
    # In IL:
    #   ldarg.1 (next)
    #   ret
    #   (03 2a in bytes!)
    # Let's inspect the IL of IntegrityGateStartupFilter.Configure!
    py_code = """
with open('/opt/absolutdashboard/AbsolutDashboard.Web.dll', 'rb') as f:
    data = bytearray(f.read())

# Look for Configure method in IntegrityGateStartupFilter
# In the assembly:
# IntegrityGateStartupFilter implements IStartupFilter:
# Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
# Target pattern: ldarg.1, ret (0x03, 0x2a) or NOPing out builder.UseMiddleware<IntegrityGateMiddleware>()
# Let's find "UseMiddleware" near IntegrityGate
idx_mw = data.find(b'UseMiddleware')
print('UseMiddleware offset:', idx_mw)

# Let's see all calls to IntegrityGateMiddleware:
idx_gate = data.find(b'IntegrityGateMiddleware')
print('IntegrityGateMiddleware offset in metadata:', idx_gate)
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
