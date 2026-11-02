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

    # Use native ssh / curl / nc commands on 10.20.110.7
    cmd = """
    echo '=== 1. TEST SSH PORT 22 ON 10.20.110.5 ==='
    nc -zv -w 2 10.20.110.5 22 2>&1 || timeout 2 bash -c "</dev/tcp/10.20.110.5/22" && echo "Port 22 OPEN on 10.20.110.5" || echo "Port 22 CLOSED/FILTERED on 10.20.110.5"

    echo ''
    echo '=== 2. SSH KEYS OR ATTEMPT SSH WITH PASSWORDS TO 10.20.110.5 ==='
    ssh -o BatchMode=yes -o ConnectTimeout=2 root@10.20.110.5 "echo SSH_KEY_AUTH_OK" 2>&1 || true

    echo ''
    echo '=== 3. CHECK PROXMOX API / PORT 8006 ON 10.20.110.4 ==='
    curl -k -s -I --connect-timeout 2 https://10.20.110.4:8006/ | head -n 5

    echo ''
    echo '=== 4. CHECK 10.20.111.30 PORTS (PING / TCP) ==='
    ping -c 2 -W 1 10.20.111.30 || true
    timeout 2 bash -c "</dev/tcp/10.20.111.30/22" 2>/dev/null && echo "Port 22 OPEN on 10.20.111.30" || echo "10.20.111.30 not reachable on port 22"
    """

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
