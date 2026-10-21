import os
import sys
import paramiko

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

    cmd = (
        "echo '=== SERVICE STATUS ===' && "
        "systemctl status absolutdashboard --no-pager && "
        "echo '=== HTTP RESPONSE ON 5000 ===' && "
        "curl -i -s http://127.0.0.1:5000 | head -n 15 && "
        "echo '=== LAST 20 LOG LINES ===' && "
        "journalctl -u absolutdashboard -n 20 --no-pager"
    )

    _, stdout, stderr = client.exec_command(cmd, timeout=30)
    out = stdout.read().decode('utf-8', errors='replace')
    err = stderr.read().decode('utf-8', errors='replace')
    client.close()

    if out:
        print(out)
    if err:
        print(err)

if __name__ == "__main__":
    run()
