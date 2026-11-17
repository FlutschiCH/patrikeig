import os
import sys
import time
import paramiko

REPO_DIR = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
DLL_PATH = os.path.join(REPO_DIR, "AbsolutPatchPortable", "AbsolutDashboard.Web.dll")

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
    sftp.put(DLL_PATH, "/tmp/AbsolutDashboard.Web.dll")
    sftp.put(os.path.join(REPO_DIR, "AbsolutPatchPortable", "RebootModule.dll"), "/tmp/RebootModule.dll")
    sftp.close()
    print("[+] Uploaded AbsolutDashboard.Web.dll and RebootModule.dll to /tmp/")

    cmd = (
        "journalctl -u absolutdashboard --since '2 minutes ago' --no-pager | "
        "grep -i -A 15 -E 'unhandled exception|fail:|crit:' | tail -n 40 || true"
    )
    stdin, stdout, stderr = client.exec_command(cmd, timeout=30)
    out = stdout.read().decode("utf-8", errors="replace")
    err = stderr.read().decode("utf-8", errors="replace")
    print(out)
    if err:
        print("STDERR:", err)
    client.close()

if __name__ == "__main__":
    main()
