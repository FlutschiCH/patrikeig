import os
import sys
import paramiko
import sqlite3

if sys.platform == "win32":
    sys.stdout.reconfigure(encoding="utf-8", errors="replace")
    sys.stderr.reconfigure(encoding="utf-8", errors="replace")

def run():
    # Read the exact SSH username and password configured for 10.20.110.5 or 10.20.111.30 from the Dashboard DB
    c7 = paramiko.SSHClient()
    c7.set_missing_host_key_policy(paramiko.AutoAddPolicy())
    c7.connect("10.20.110.7", port=22, username="root", password="Absolut1.", timeout=5)

    py_db = """
import sqlite3
import os

con = sqlite3.connect('/var/lib/absolutdashboard-test/app.db')
cur = con.cursor()
cur.execute("SELECT Id, Name, IpAddress, Port, SshUsername, SshPasswordEnc FROM Devices;")
for r in cur.fetchall():
    print(f"Device: ID={r[0]} | Name={r[1]} | IP={r[2]} | Port={r[3]} | User={r[4]}")
con.close()
"""
    import base64
    b64 = base64.b64encode(py_db.encode()).decode()
    _, stdout, stderr = c7.exec_command(f'python3 -c "import base64; exec(base64.b64decode(\'{b64}\').decode())"')
    print(stdout.read().decode('utf-8', errors='replace'))
    c7.close()

if __name__ == "__main__":
    run()
