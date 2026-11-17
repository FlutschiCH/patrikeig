import paramiko

HOST = "10.20.110.7"
PORT = 22
USER = "root"
PASS = "Absolut1."

def main():
    client = paramiko.SSHClient()
    client.set_missing_host_key_policy(paramiko.AutoAddPolicy())
    client.connect(hostname=HOST, port=PORT, username=USER, password=PASS, timeout=10)
    print(f"[+] Connected to {HOST}, sending reboot...")
    stdin, stdout, stderr = client.exec_command("uptime && systemctl is-active absolutdashboard && curl -s -o /dev/null -w 'HTTP %{http_code}\\n' http://127.0.0.1:5000/", timeout=10)
    print(stdout.read().decode("utf-8", errors="replace"))
    client.close()
    print("[+] Reboot command sent.")

if __name__ == "__main__":
    main()
