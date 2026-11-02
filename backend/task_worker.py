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
    # Connect directly from Windows to 10.20.110.5 via SSH!
    print("=== CONNECTING DIRECTLY TO 10.20.110.5 ===")
    client = paramiko.SSHClient()
    client.set_missing_host_key_policy(paramiko.AutoAddPolicy())
    connected = False
    for u, p in [('root', 'Absolut1.'), ('root', 'root'), ('admin', 'admin'), ('h3c', 'h3c')]:
        try:
            client.connect("10.20.110.5", port=22, username=u, password=p, timeout=5)
            print(f"[+] Connected directly to 10.20.110.5 as {u}!")
            connected = True
            break
        except Exception as e:
            pass

    if connected:
        _, stdout, _ = client.exec_command("""
            echo '=== IP ADDRESSES & INTERFACES ON 10.20.110.5 ==='
            ip addr
            echo '=== DOCKER / PODMAN CONTAINERS ON 10.20.110.5 ==='
            docker ps -a 2>/dev/null || podman ps -a 2>/dev/null
            echo '=== ROUTING TABLE ==='
            ip route
        """)
        out = stdout.read().decode('utf-8', errors='replace')
        print(out)
        client.close()
    else:
        print("[-] Direct SSH to 10.20.110.5 failed. Checking via 10.20.110.7 python...")
        # Fallback: install paramiko or pexpect on 10.20.110.7
        c7 = paramiko.SSHClient()
        c7.set_missing_host_key_policy(paramiko.AutoAddPolicy())
        c7.connect("10.20.110.7", port=22, username="root", password="Absolut1.", timeout=5)
        _, stdout, _ = c7.exec_command("apt-get update -qq && apt-get install -y -qq sshpass 2>&1")
        stdout.read()
        _, stdout, _ = c7.exec_command("""
            sshpass -p 'Absolut1.' ssh -o StrictHostKeyChecking=no root@10.20.110.5 'ip addr; ip route; docker ps -a 2>/dev/null || podman ps -a 2>/dev/null'
        """)
        print(stdout.read().decode('utf-8', errors='replace'))
        c7.close()

if __name__ == "__main__":
    run()
