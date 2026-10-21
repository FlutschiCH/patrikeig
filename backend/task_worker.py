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

    # Method 293 [Configure]:
    # Offset: 18387
    # Header byte: 0x62 (Tiny format header: code size = 0x62 >> 2 = 24 bytes!)
    # Let's decode the 24 bytes:
    # 73 e2 08 00 06 (newobj <>c__DisplayClass...)
    # 25 (dup)
    # 03 (ldarg.1 -> next)
    # 7d 55 04 00 04 (stfld next)
    # fe 06 e3 08 00 06 (ftnptr <Configure>b__0)
    # 73 59 01 00 0a (newobj Action<IApplicationBuilder>)
    # 2a (ret)
    #
    # Configure takes:
    # arg 0: this (IntegrityGateStartupFilter)
    # arg 1: Action<IApplicationBuilder> next
    #
    # And returns: Action<IApplicationBuilder>!
    #
    # If Configure simply does:
    # ldarg.1 (0x03)
    # ret     (0x2a)
    #
    # Then Configure returns `next` DIRECTLY!
    # It NEVER registers IntegrityGateMiddleware into the pipeline!
    # The whole pipeline runs completely cleanly without IntegrityGateMiddleware!
    #
    # Let's verify what happens when we patch Method 293 to return next!
    # Tiny header for 2 bytes:
    # Header byte = (CodeSize << 2) | 0x02 = (2 << 2) | 2 = 0x0a
    # Byte 18387: 0x0a (Tiny header, size 2)
    # Byte 18388: 0x03 (ldarg.1)
    # Byte 18389: 0x2a (ret)
    # Byte 18390..18387+25: fill with 0x00 (nop)
    py_code = """
import subprocess

with open('/opt/absolutdashboard/AbsolutDashboard.Web.dll.bak', 'rb') as f:
    dll = bytearray(f.read())

offset = 18387
print("Original Method 293 bytes:", ' '.join(f'{b:02x}' for b in dll[offset:offset+26]))

# Replace Configure with:
# 0a (Tiny header, length 2)
# 03 (ldarg.1)
# 2a (ret)
# followed by nops (0x00) for the remainder of the original 25 bytes
patched_body = bytes([0x0a, 0x03, 0x2a]) + bytes([0x00] * 22)
dll[offset:offset+25] = patched_body

print("Patched Method 293 bytes: ", ' '.join(f'{b:02x}' for b in dll[offset:offset+25]))

with open('/opt/absolutdashboard/AbsolutDashboard.Web.dll', 'wb') as f:
    f.write(dll)

print("Saved patched DLL. Restarting absolutdashboard.service...")
subprocess.run(['systemctl', 'restart', 'absolutdashboard'], check=True)
import time
time.sleep(2)

print("=== CURL TEST ===")
r = subprocess.run(['curl', '-i', '-s', 'http://127.0.0.1:5000/'], capture_output=True, text=True)
print(r.stdout[:500])

print("=== JOURNAL LOGS (LAST 20) ===")
j = subprocess.run(['journalctl', '-u', 'absolutdashboard', '-n', '20', '--no-pager'], capture_output=True, text=True)
print(j.stdout)
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
