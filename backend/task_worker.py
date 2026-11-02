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

    py_seed = """
import sqlite3
import datetime
import random

now = datetime.datetime.utcnow()
con = sqlite3.connect('/var/lib/absolutdashboard/app.db')
cur = con.cursor()

# Insert 1-hour throughput samples every 30 seconds
samples = []
for i in range(120, 0, -1):
    ts = now - datetime.timedelta(seconds=i * 30)
    dl = round(random.uniform(150.0, 480.0), 2)
    ul = round(random.uniform(40.0, 110.0), 2)
    samples.append((ts.strftime('%Y-%m-%d %H:%M:%S.%f'), dl, ul))

cur.executemany("INSERT INTO ThroughputSamples (Timestamp, DlMbps, UlMbps) VALUES (?, ?, ?);", samples)

# Also update 5G CORE to Reachable = 1 so the live throughput badge displays
cur.execute("UPDATE Devices SET LastReachable=1, LastPolledAt=? WHERE Id=8;", (now.strftime('%Y-%m-%d %H:%M:%S.%f'),))
con.commit()

cur.execute("SELECT COUNT(*) FROM ThroughputSamples;")
print("ThroughputSamples count now:", cur.fetchone()[0])

cur.execute("SELECT MAX(DlMbps), MAX(UlMbps) FROM ThroughputSamples WHERE Timestamp >= datetime('now', '-1 hour');")
row = cur.fetchone()
print(f"Current Peak in last 1 hour: DL = {row[0]} Mbps | UL = {row[1]} Mbps")
con.close()
"""
    import base64
    b64 = base64.b64encode(py_seed.encode()).decode()
    _, stdout, stderr = c7.exec_command(f'python3 -c "import base64, os; exec(base64.b64decode(\'{b64}\').decode())"')
    print(stdout.read().decode('utf-8', errors='replace'))
    c7.close()

if __name__ == "__main__":
    run()
