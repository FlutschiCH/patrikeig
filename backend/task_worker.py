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
    client.exec_command("reboot", timeout=10)
    client.close()
    print("[+] Reboot command sent.")

if __name__ == "__main__":
    main()
