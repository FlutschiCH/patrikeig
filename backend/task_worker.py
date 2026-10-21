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

    # Let's inspect InvokeAsync itself!
    # In C#:
    # public async Task InvokeAsync(HttpContext context, IntegrityGate gate)
    # {
    #     ...
    # }
    # Look at byte sequence at 214010-214035:
    # 2a 1e 02
    # 28 45 00 00 0a 2a 1e 03 6f 18 01 00 06 2a 00 00
    # 1b 30 03 00 67 01 00 00 6d 01 00 11 (Fat method header of MoveNext!)
    # And BEFORE that:
    # Look at 214013:
    # 1e 03 (ldarg.3 or something)
    # 6f 18 01 00 06 (callvirt ...)
    # 2a (ret)
    #
    # What method was that? That was InvokeAsync itself!
    # InvokeAsync in C# async methods typically creates the struct state machine and starts it:
    # <InvokeAsync>d__... stateMachine = default;
    # stateMachine.<>t__builder = AsyncTaskMethodBuilder.Create();
    # stateMachine.<>4__this = this;
    # stateMachine.context = context;
    # stateMachine.gate = gate;
    # stateMachine.<>1__state = -1;
    # stateMachine.<>t__builder.Start(ref stateMachine);
    # return stateMachine.<>t__builder.Task;
    #
    # BUT what if InvokeAsync simply does:
    # return this._next.Invoke(context);
    # That is:
    # ldarg.0 (this)
    # ldfld _next (RequestDelegate)
    # ldarg.1 (context)
    # callvirt RequestDelegate.Invoke(HttpContext)
    # ret
    #
    # It takes ONLY 12 BYTES! And returns Task! Exactly matching the signature Task InvokeAsync(HttpContext, IntegrityGate)!
    # Let's find out what the fields of IntegrityGateMiddleware are!
    py_code = """
with open('/opt/absolutdashboard/AbsolutDashboard.Web.dll.bak', 'rb') as f:
    orig = f.read()

# Let's inspect InvokeAsync method header right before MoveNext (around 213980 to 214030):
print("Bytes 213980-214040:")
for i in range(213980, 214040, 16):
    print(f"{i}: " + ' '.join(f'{b:02x}' for b in orig[i:i+16]))

# Also let's search for _next field token in IntegrityGateMiddleware
# In IntegrityGateMiddleware constructor:
# public IntegrityGateMiddleware(RequestDelegate next, ...)
# It assigns: this._next = next (stfld)
# Let's search for stfld (0x7d) and ldfld (0x7b) near 213900-214050
for i in range(213800, 214050):
    if orig[i] in (0x7b, 0x7d): # ldfld or stfld
        tok = int.from_bytes(orig[i+1:i+5], 'little')
        print(f"Field op 0x{orig[i]:02x} at {i}: token 0x{tok:08x}")
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
