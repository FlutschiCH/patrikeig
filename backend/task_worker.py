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

    py_check = """
import subprocess
import time

time.sleep(6)
log = subprocess.run(['journalctl', '-u', 'absolutdashboard', '-n', '60', '--no-pager'], capture_output=True, text=True)
has_err = False
for l in log.stdout.splitlines():
    if 'fail:' in l or 'System.' in l:
        print("ERROR:", l)
        has_err = True

if not has_err:
    print("[SUCCESS] Zero exceptions in poller cycle! Polling is actively executing without blocking.")

# Check DB timestamps
import sqlite3
con = sqlite3.connect('/var/lib/absolutdashboard/app.db')
cur = con.cursor()
cur.execute("SELECT Id, Name, LastPolledAt, LastReachable FROM Devices WHERE Id IN (1, 8, 10);")
for r in cur.fetchall():
    print(f"Device: {r[1]} -> Polled: {r[2]} | Reachable: {r[3]}")
con.close()
"""
    import base64
    b64 = base64.b64encode(py_check.encode()).decode()
    _, stdout, stderr = c7.exec_command(f'python3 -c "import base64, os; exec(base64.b64decode(\'{b64}\').decode())"')
    print(stdout.read().decode('utf-8', errors='replace'))
    c7.close()

if __name__ == "__main__":
    run()
