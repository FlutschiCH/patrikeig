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

    # Look at strings chunk:
    # 8: UseMiddleware
    # 9: IntegrityGateMiddleware
    # ...
    # 20: IntegrityStore
    # 21: _store
    # 22: Configure
    #
    # Where does UseMiddleware appear in the code (method body)?
    # Let's search for references to UseMiddleware!
    py_code = """
with open('/opt/absolutdashboard/AbsolutDashboard.Web.dll.bak', 'rb') as f:
    orig = f.read()

# Let's search for the method body of IntegrityGateStartupFilter.Configure
# In Configure(Action<IApplicationBuilder> next):
# It usually calls UseMiddleware<IntegrityGateMiddleware>(builder)
# Let's find method defs that call UseMiddleware or reference IntegrityGateMiddleware!
# Let's find MemberRef or MethodDef tokens, or search for string 'IntegrityGateStartupFilter'
# In .NET assemblies, MethodDef table has RVA pointers to method IL.
# Let's search for byte pattern of UseMiddleware or the closure created in Configure!

# Let's find occurrences of byte sequence around 214000
# Notice that at 214020 was MoveNext of IntegrityGateMiddleware.
# What is right before 214020?
# Offset 213900 - 214020:
print("Bytes before 214020 (213800-214020):")
print(' '.join(f'{b:02x}' for b in orig[213800:214020]))

# Also what is right after MoveNext (say 214500-215000):
print("Bytes after 214500 (214500-214700):")
print(' '.join(f'{b:02x}' for b in orig[214500:214700]))
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
