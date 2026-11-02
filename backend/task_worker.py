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
        print(f"SSH failed: {e}")
        return

    py_code = """
import subprocess

print("=== 1. ROUTING FROM 10.20.110.7 (LINUX) ===")
# Ping default gateway 10.20.110.1
p_gw = subprocess.run(['ping', '-c', '2', '-W', '2', '10.20.110.1'], capture_output=True, text=True)
print("Ping 10.20.110.1 (Router):", "OK" if p_gw.returncode == 0 else "FAIL")

# Ping Proxmox 10.20.110.4
p_prox = subprocess.run(['ping', '-c', '2', '-W', '2', '10.20.110.4'], capture_output=True, text=True)
print("Ping 10.20.110.4 (Proxmox):", "OK" if p_prox.returncode == 0 else "FAIL")

# Ping 5G Core VM 10.20.110.5
p_vm = subprocess.run(['ping', '-c', '2', '-W', '2', '10.20.110.5'], capture_output=True, text=True)
print("Ping 10.20.110.5 (5G Core VM Mgmt):", "OK" if p_vm.returncode == 0 else "FAIL")

# Ping 5G Core Service 10.20.111.30
p_core = subprocess.run(['ping', '-c', '2', '-W', '2', '10.20.111.30'], capture_output=True, text=True)
print("Ping 10.20.111.30 (5G Core):", "OK" if p_core.returncode == 0 else "FAIL")

# Traceroute to 10.20.111.30
tr = subprocess.run(['traceroute', '-n', '-m', '4', '10.20.111.30'], capture_output=True, text=True)
print("Traceroute to 10.20.111.30:\\n", tr.stdout or tr.stderr)

# Scan for any alive IPs on 10.20.110.0/24 from 10.20.110.7
print("\\n=== 2. QUICK ARP SCAN OF 10.20.110.x FROM LINUX ===")
scan = subprocess.run(['arp-scan', '--localnet'], capture_output=True, text=True)
if scan.returncode == 0:
    print(scan.stdout)
else:
    # fallback to fping or nmap
    nmap = subprocess.run(['nmap', '-sn', '10.20.110.0/24'], capture_output=True, text=True)
    print(nmap.stdout[:1500])
"""

    b64_code = base64.b64encode(py_code.encode('utf-8')).decode('ascii')
    cmd = f'python3 -c "import base64; exec(base64.b64decode(\'{b64_code}\').decode(\'utf-8\'))"'

    _, stdout, stderr = client.exec_command(cmd, timeout=60)
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
