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

    # Use sshpass or python with sshpass / pexpect / expect to login with Absolut1.
    cmd = """
    echo '=== ATTEMPT SSH INTO 10.20.110.5 WITH PASSWORD ==='
    sshpass -p 'Absolut1.' ssh -o StrictHostKeyChecking=no -o ConnectTimeout=3 root@10.20.110.5 "
        echo '=== 10.20.110.5 INTERFACES ==='
        ip addr
        echo '=== 10.20.110.5 DOCKER / PODMAN CONTAINERS ==='
        docker ps -a 2>/dev/null || podman ps -a 2>/dev/null
    " 2>&1 || true
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
