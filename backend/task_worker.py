import os
import sys
import paramiko
import base64

if sys.platform == "win32":
    sys.stdout.reconfigure(encoding="utf-8", errors="replace")
    sys.stderr.reconfigure(encoding="utf-8", errors="replace")

def run():
    client = paramiko.SSHClient()
    client.set_missing_host_key_policy(paramiko.AutoAddPolicy())
    try:
        client.connect("10.20.110.7", port=22, username="root", password="Absolut1.", timeout=8)
    except Exception as e:
        print(f"SSH to 10.20.110.7 failed: {e}")
        return

    py_code = """
import subprocess
import paramiko

print("=== CHECK ROUTER 10.20.110.1 REACHABILITY ===")
p_gw = subprocess.run(['ping', '-c', '2', '-W', '2', '10.20.110.1'], capture_output=True, text=True)
print("Router 10.20.110.1 Ping:", "OK" if p_gw.returncode == 0 else "FAIL")

print("\\n=== CONNECT TO 5G CORE VM (10.20.110.5) VIA SSH ===")
# Try SSH from 10.20.110.7 to 10.20.110.5
client_vm = paramiko.SSHClient()
client_vm.set_missing_host_key_policy(paramiko.AutoAddPolicy())

connected = False
for u, p in [('root', 'Absolut1.'), ('root', 'root'), ('h3c', 'h3c'), ('admin', 'admin')]:
    try:
        client_vm.connect('10.20.110.5', port=22, username=u, password=p, timeout=4)
        print(f"[+] Successfully logged into 10.20.110.5 with user '{u}'!")
        _, stdout, _ = client_vm.exec_command('ip addr; ip route; docker ps 2>/dev/null || podman ps 2>/dev/null')
        print(stdout.read().decode('utf-8', errors='replace'))
        client_vm.close()
        connected = True
        break
    except Exception as e:
        pass

if not connected:
    print("[-] SSH to 10.20.110.5 with standard credentials timed out or failed.")
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
