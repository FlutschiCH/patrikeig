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
import sqlite3

con = sqlite3.connect('/var/lib/absolutdashboard/app.db')
cur = con.cursor()
cur.execute("SELECT MAX(Timestamp) FROM ThroughputSamples;")
print("Max ThroughputSample Timestamp:", cur.fetchone()[0])

cur.execute("SELECT MAX(Timestamp) FROM DevicePollResults;")
print("Max DevicePollResult Timestamp:", cur.fetchone()[0])
con.close()
"""
    import base64
    b64 = base64.b64encode(py_check.encode()).decode()
    _, stdout, stderr = c7.exec_command(f'python3 -c "import base64; exec(base64.b64decode(\'{b64}\').decode())"')
    print(stdout.read().decode('utf-8', errors='replace'))
    c7.close()

if __name__ == "__main__":
    run()
