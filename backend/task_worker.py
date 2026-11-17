import os
import paramiko

REPO_DIR = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
MOD_PATH = os.path.join(REPO_DIR, "AbsolutPatchPortable", "RebootModule.dll")

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
    sftp.put(MOD_PATH, "/tmp/RebootModule.dll")
    sftp.close()
    print("[+] Uploaded RebootModule.dll to /tmp/")

    cmd = (
        "cp /tmp/RebootModule.dll /opt/absolutdashboard/RebootModule.dll && "
        "cp /tmp/RebootModule.dll /opt/absolutdashboard-test/RebootModule.dll && "
        "cp /tmp/RebootModule.dll /home/absolutsrf/deploy/app/RebootModule.dll && "
        "systemctl restart absolutdashboard absolutdashboard-test && "
        "sleep 3 && "
        "systemctl is-active absolutdashboard && "
        "curl -s -o /dev/null -w 'HTTP %{http_code}\\n' http://127.0.0.1:5000/"
    )
    stdin, stdout, stderr = client.exec_command(cmd, timeout=30)
    print(stdout.read().decode("utf-8", errors="replace"))
    err = stderr.read().decode("utf-8", errors="replace")
    if err:
        print("STDERR:", err)
    client.close()

if __name__ == "__main__":
    main()
