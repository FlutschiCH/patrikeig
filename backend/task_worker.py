import os
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
import json, os
for d in ['/opt/absolutdashboard', '/opt/absolutdashboard-test', '/home/absolutsrf/deploy/app']:
    p = os.path.join(d, 'AbsolutDashboard.Web.deps.json')
    if os.path.exists(p):
        with open(p, 'r') as f:
            data = json.load(f)
        for t in data.get('targets', {}).values():
            if 'RebootModule/1.0.0' not in t:
                t['RebootModule/1.0.0'] = {'runtime': {'RebootModule.dll': {}}}
        if 'RebootModule/1.0.0' not in data.get('libraries', {}):
            data.setdefault('libraries', {})['RebootModule/1.0.0'] = {'type': 'project', 'serviceable': False, 'sha512': ''}
        with open(p, 'w') as f:
            json.dump(data, f, indent=2)
        print('Updated', p)
" && systemctl restart absolutdashboard absolutdashboard-test && sleep 3 && systemctl is-active absolutdashboard && curl -s -o /dev/null -w 'HTTP %{http_code}\n' http://127.0.0.1:5000/"""

    stdin, stdout, stderr = client.exec_command(cmd, timeout=30)
    print(stdout.read().decode("utf-8", errors="replace"))
    err = stderr.read().decode("utf-8", errors="replace")
    if err:
        print("STDERR:", err)
    client.close()

if __name__ == "__main__":
    main()
