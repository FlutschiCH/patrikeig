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

    py_db = """
import sqlite3

con = sqlite3.connect('/var/lib/absolutdashboard-test/app.db')
cur = con.cursor()
cur.execute("SELECT COUNT(*) FROM ThroughputSamples;")
print("ThroughputSamples in active test DB:", cur.fetchone()[0])
cur.execute("SELECT COUNT(*) FROM DevicePollResults;")
print("DevicePollResults in active test DB:", cur.fetchone()[0])
cur.execute("SELECT Id, Name, LastPolledAt, LastReachable FROM Devices WHERE Id IN (1, 8);")
print("Devices status in active test DB:")
for r in cur.fetchall():
    print(" ", r)
con.close()
"""
    import base64
    b64 = base64.b64encode(py_db.encode()).decode()
    _, stdout, stderr = c7.exec_command(f'python3 -c "import base64; exec(base64.b64decode(\'{b64}\').decode())"')
    print(stdout.read().decode('utf-8', errors='replace'))
    c7.close()

if __name__ == "__main__":
    run()
