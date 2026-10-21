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

    # Let's inspect the IL around 214020.
    # Notice:
    # 20 f4 01 00 00 -> ldc.i4 500 (0x01f4 = 500!)
    # 6f 34 06 00 0a -> callvirt HttpResponse.set_StatusCode(500)
    #
    # And look what's before that:
    # 02 7b 51 04 00 04 6f 1f 01 00 06
    # 02 7b 51 04 00 04 6f 1e 01 00 06
    # 39 9f 00 00 00 (brtrue +0x9f)
    #
    # When it does NOT branch, it falls through to:
    # 02 7b 52 04 00 04 (ldarg.0, ldfld context/response)
    # 6f 5d 05 00 0a (get_Response)
    # 20 f4 01 00 00 (ldc.i4 500)
    # 6f 34 06 00 0a (set_StatusCode)
    #
    # And notice before that:
    # 06 39 9c 00 00 00
    # 06 17 3b fa 00 00 00
    # There are multiple checks!
    # Let's read all of InvokeAsync from 214020 to 214600 and find where the success branch (invoking next middleware) is!
    py_code = """
with open('/opt/absolutdashboard/AbsolutDashboard.Web.dll.bak', 'rb') as f:
    orig = f.read()

start = 214020
print("Length of method body to analyze: 600 bytes")
code = orig[start:start+600]

# Let's find all occurrences of 20 f4 01 00 00 (ldc.i4 500)
idx = 0
while True:
    idx = code.find(b'\\x20\\xf4\\x01\\x00\\x00', idx)
    if idx == -1:
        break
    print(f"ldc.i4 500 at relative offset {idx} (file offset {start+idx})")
    idx += 5

# Let's inspect where _next is invoked in InvokeAsync or what the state machine is
# Notice that InvokeAsync is an async state machine!
# Look at the fields:
# 7b 4f 04 00 04 -> ldfld <>1__state
# 7b 53 04 00 04
# 7b 51 04 00 04
# 7b 52 04 00 04
# It is an async state machine struct's MoveNext() method!
# Look at the beginning:
# 02 7b 4f 04 00 04 0a -> ldarg.0; ldfld <>1__state; stloc.0
# 06 39 9c 00 00 00 -> ldloc.0; brtrue ...
# YES! This is MoveNext() of <InvokeAsync>d__...
print("This is MoveNext() of the async state machine struct!")

# In an async state machine:
# state == -1 is initial call.
# If check passes, it calls _next(context) and awaits it!
# Where does it call _next? Let's search for RequestDelegate.Invoke
# Let's look for callvirt / call in the whole 600 bytes:
for i in range(len(code)-5):
    if code[i] in (0x28, 0x6f): # call or callvirt
        tok = int.from_bytes(code[i+1:i+5], 'little')
        # print some notable calls
        if tok in (0x0a000634, 0x0a000635, 0x0a000636, 0x0a000637):
            print(f"Offset {i} (file {start+i}): opcode 0x{code[i]:02x}, token 0x{tok:08x}")

# Let's see what happens if 39 9f 00 00 00 (brtrue +0x9f) branches!
# Offset of 39 9f is 72. Branch target is 72 + 5 + 0x9f = 77 + 159 = 236!
branch_target = 72 + 5 + 0x9f
print(f"Branch target if valid: relative {branch_target} (file offset {start+branch_target})")
print("Target bytes:", ' '.join(f'{b:02x}' for b in code[branch_target:branch_target+50]))
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
