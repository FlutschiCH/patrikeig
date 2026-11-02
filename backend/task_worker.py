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
import subprocess

print("=== CHECK IF POLLER IS INSERTING NEW RECORDS ===")
for path in ['/var/lib/absolutdashboard-test/app.db', '/var/lib/absolutdashboard/app.db']:
    if os.path.exists(path):
        con = sqlite3.connect(path)
        cur = con.cursor()
        cur.execute("SELECT COUNT(*) FROM ThroughputSamples;")
        ts = cur.fetchone()[0]
        cur.execute("SELECT COUNT(*) FROM DevicePollResults;")
        pr = cur.fetchone()[0]
        cur.execute("SELECT Id, Name, LastPolledAt, LastReachable FROM Devices WHERE Id IN (1, 8, 10);")
        rows = cur.fetchall()
        print(f"DB {path}: ThroughputSamples={ts}, PollResults={pr}")
        for r in rows:
            print(f"  {r[1]}: LastPolled={r[2]}, Reachable={r[3]}")
        con.close()
"""
    import base64
    b64 = base64.b64encode(py_poller.encode()).decode()
    _, stdout, stderr = c7.exec_command(f'python3 -c "import base64, os; exec(base64.b64decode(\'{b64}\').decode())"')
    print(stdout.read().decode('utf-8', errors='replace'))
    c7.close()

if __name__ == "__main__":
    run()
