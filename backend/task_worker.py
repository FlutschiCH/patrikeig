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
import sqlite3
import time

time.sleep(8)
con = sqlite3.connect('/var/lib/absolutdashboard/app.db')
cur = con.cursor()
cur.execute("SELECT Id, Name, IpAddress, LastReachable, LastPolledAt, LastError FROM Devices WHERE Id=8;")
r = cur.fetchone()
print(f"5G CORE: IP={r[2]} | Reachable={r[3]} | LastPolled={r[4]}")
print(f"LastError: {r[5]}")

cur.execute("SELECT COUNT(*) FROM ThroughputSamples;")
print("ThroughputSamples count:", cur.fetchone()[0])
con.close()
"""
    import base64
    b64 = base64.b64encode(py_poller.encode()).decode()
    _, stdout, stderr = c7.exec_command(f'python3 -c "import base64, os; exec(base64.b64decode(\'{b64}\').decode())"')
    print(stdout.read().decode('utf-8', errors='replace'))
    c7.close()

if __name__ == "__main__":
    run()
