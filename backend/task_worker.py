import os
import sys
import paramiko
import base64

if sys.platform == "win32":
    sys.stdout.reconfigure(encoding="utf-8", errors="replace")
    sys.stderr.reconfigure(encoding="utf-8", errors="replace")

REMOTE_HOST = os.environ.get("REMOTE_SSH_HOST", "10.20.110.7")
REMOTE_USER = os.environ.get("REMOTE_SSH_USER", "root")
REMOTE_PASS = os.environ.get("REMOTE_SSH_PASSWORD", "Absolut1.")

def run():
    client = paramiko.SSHClient()
    client.set_missing_host_key_policy(paramiko.AutoAddPolicy())
    try:
        client.connect(hostname=REMOTE_HOST, port=22, username=REMOTE_USER, password=REMOTE_PASS, timeout=8)
    except Exception as e:
        print(f"[!] SSH connection failed: {e}")
        return

    py_code = """
import subprocess

print("=== 1. IP ADDRESSES ON HOST (10.20.110.7) ===")
res = subprocess.run(['ip', 'addr'], capture_output=True, text=True)
print(res.stdout)

print("=== 2. IP ROUTES & DEFAULT GATEWAY ===")
res = subprocess.run(['ip', 'route'], capture_output=True, text=True)
print(res.stdout)

print("=== 3. TRACEROUTE TO 10.20.111.30 (5G CORE) ===")
# Try traceroute or tracepath or mtr or ping with ttl
res = subprocess.run(['traceroute', '-n', '-m', '5', '10.20.111.30'], capture_output=True, text=True)
if res.returncode != 0 or not res.stdout:
    res = subprocess.run(['tracepath', '-n', '10.20.111.30'], capture_output=True, text=True)
print(res.stdout or res.stderr)

print("=== 4. PING DEFAULT GATEWAY ===")
lines = [l for l in subprocess.run(['ip', 'route'], capture_output=True, text=True).stdout.splitlines() if 'default' in l]
gw = lines[0].split()[2] if lines and len(lines[0].split()) > 2 else None
print(f"Default Gateway detected: {gw}")
if gw:
    pgw = subprocess.run(['ping', '-c', '2', '-W', '2', gw], capture_output=True, text=True)
    print(f"Ping gateway ({gw}):", "OK" if pgw.returncode == 0 else "FAILED")

print("=== 5. ARP TABLE (NEIGHBORS) ===")
arp = subprocess.run(['ip', 'neigh'], capture_output=True, text=True)
print(arp.stdout)
"""

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
