import os
import sys
import paramiko

if sys.platform == "win32":
    sys.stdout.reconfigure(encoding="utf-8", errors="replace")
    sys.stderr.reconfigure(encoding="utf-8", errors="replace")

def run():
    c7 = paramiko.SSHClient()
    c7.set_missing_host_key_policy(paramiko.AutoAddPolicy())
    c7.connect("10.20.110.7", port=22, username="root", password="Absolut1.", timeout=5)

    py_poller = """
import subprocess

print("=== CHECK SYSTEMD FOR POLLER ERRORS IN LAST 500 LINES ===")
log = subprocess.run(['journalctl', '-u', 'absolutdashboard', '-n', '500', '--no-pager'], capture_output=True, text=True)
poller_lines = [l for l in log.stdout.splitlines() if any(k in l.lower() for k in ['polling', 'poll', 'corelive', 'integrity', 'blocked', 'due', '5g core'])]
for l in poller_lines[-25:]:
    print(" ", l)

print("\\n=== CHECK INTEGRITY / LICENSE SETTINGS IN APPSETTINGS ===")
cfg = subprocess.run(['cat', '/opt/absolutdashboard/appsettings.json'], capture_output=True, text=True)
print(cfg.stdout[:500])
"""
    import base64
    b64 = base64.b64encode(py_poller.encode()).decode()
    _, stdout, stderr = c7.exec_command(f'python3 -c "import base64; exec(base64.b64decode(\'{b64}\').decode())"')
    print(stdout.read().decode('utf-8', errors='replace'))
    c7.close()

if __name__ == "__main__":
    run()
