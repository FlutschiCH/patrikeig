import paramiko

HOST = "10.20.110.7"
PORT = 22
USER = "root"
PASS = "Absolut1."

def main():
    client = paramiko.SSHClient()
    client.set_missing_host_key_policy(paramiko.AutoAddPolicy())
    client.connect(hostname=HOST, port=PORT, username=USER, password=PASS, timeout=10)
    print(f"[+] Connected to {HOST}")

    cmd = """python3 -c "
p = '/etc/absolutdashboard/absolut.env'
with open(p, 'r') as f:
    lines = [l for l in f.readlines() if not l.startswith('Operations__Reboot__') and l.strip() != '# System reboot steps']
lines.append('\\n# System reboot steps\\n')
lines.append('Operations__Reboot__Steps__0__Label=Reboot the base station\\n')
lines.append('Operations__Reboot__Steps__0__Kind=mml\\n')
lines.append('Operations__Reboot__Steps__0__Command=RST BTS:TYPE=PROCESS;\\n')
lines.append('Operations__Reboot__Steps__1__Label=Reboot the 5G core containers (H3C Linux)\\n')
lines.append('Operations__Reboot__Steps__1__Kind=ssh\\n')
lines.append('Operations__Reboot__Steps__1__Host=10.20.110.5\\n')
lines.append('Operations__Reboot__Steps__1__User=root\\n')
lines.append('Operations__Reboot__Steps__1__Password=root@vSCN5100\\n')
lines.append('Operations__Reboot__Steps__1__Command=reboot\\n')
with open(p, 'w') as f:
    f.writelines(lines)
print('Updated', p)
" && systemctl restart absolutdashboard absolutdashboard-test && sleep 3 && systemctl is-active absolutdashboard"""

    stdin, stdout, stderr = client.exec_command(cmd, timeout=20)
    print(stdout.read().decode("utf-8", errors="replace"))
    err = stderr.read().decode("utf-8", errors="replace")
    if err:
        print("STDERR:", err)
    client.close()

if __name__ == "__main__":
    main()
