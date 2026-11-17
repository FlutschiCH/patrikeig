import os
import paramiko

REPO_DIR = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
MOD_PATH = os.path.join(REPO_DIR, "AbsolutPatchPortable", "AbsolutDashboard.Web.dll")

HOST = "10.20.110.7"
PORT = 22
USER = "root"
PASS = "Absolut1."

def main():
    client = paramiko.SSHClient()
    client.set_missing_host_key_policy(paramiko.AutoAddPolicy())
    client.connect(hostname=HOST, port=PORT, username=USER, password=PASS, timeout=10)
    print(f"[+] Connected to {HOST}")

    sftp = client.open_sftp()
    sftp.put(MOD_PATH, "/tmp/AbsolutDashboard.Web.dll")
    sftp.close()
    print("[+] Uploaded AbsolutDashboard.Web.dll to /tmp/")

    cmd = (
        "cp /tmp/AbsolutDashboard.Web.dll /opt/absolutdashboard/AbsolutDashboard.Web.dll && "
        "cp /tmp/AbsolutDashboard.Web.dll /opt/absolutdashboard-test/AbsolutDashboard.Web.dll && "
        "cp /tmp/AbsolutDashboard.Web.dll /home/absolutsrf/deploy/app/AbsolutDashboard.Web.dll && "
        "systemctl restart absolutdashboard absolutdashboard-test && "
        "sleep 3 && "
        "systemctl is-active absolutdashboard"
    )
    stdin, stdout, stderr = client.exec_command(cmd, timeout=30)
    print(stdout.read().decode("utf-8", errors="replace"))
    err = stderr.read().decode("utf-8", errors="replace")
    if err:
        print("STDERR:", err)
    client.close()

if __name__ == "__main__":
    main()
