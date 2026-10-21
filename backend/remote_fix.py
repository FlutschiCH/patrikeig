import os
import sys
import paramiko

REMOTE_HOST = os.environ.get("REMOTE_SSH_HOST", "10.20.110.7")
REMOTE_USER = os.environ.get("REMOTE_SSH_USER", "root")
REMOTE_PASS = os.environ.get("REMOTE_SSH_PASSWORD", "Absolut1.")

def run_remote_fix():
    print(f"[*] Connecting via SSH to {REMOTE_USER}@{REMOTE_HOST}...")
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
        print(f"[!] SSH connection failed ({e}). Skipping remote dashboard repair.")
        return

    repair_cmd = (
        "mkdir -p /var/lib/absolutdashboard/keys && "
        "cp -f /root/.aspnet/DataProtection-Keys/*.xml /var/lib/absolutdashboard/keys/ 2>/dev/null || true; "
        "chown -R absolutsrf:absolutsrf /var/lib/absolutdashboard && "
        "chmod 750 /var/lib/absolutdashboard && "
        "chmod 700 /var/lib/absolutdashboard/keys && "
        "chmod 600 /var/lib/absolutdashboard/keys/*.xml 2>/dev/null || true; "
        "rm -f /var/lib/absolutdashboard/.dp-cache.st /etc/absolutdashboard/.dp-cache.st /opt/absolutdashboard/.dp-cache.st && "
        "systemctl restart absolutdashboard && "
        "sleep 2 && curl -i http://127.0.0.1:5000"
    )

    print("[*] Executing automated repair on remote server...")
    _, stdout, stderr = client.exec_command(repair_cmd, timeout=15)
    out = stdout.read().decode('utf-8', errors='replace')
    err = stderr.read().decode('utf-8', errors='replace')
    client.close()

    if out:
        print(out)
    if err:
        print(err)

if __name__ == "__main__":
    run_remote_fix()
