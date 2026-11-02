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
import subprocess

# Find active db file path from absolutdashboard process
out = subprocess.run('ls -l /proc/$(pgrep -o AbsolutDashboar)/fd/ | grep -E "\\\\.db"', shell=True, capture_output=True, text=True)
print("Active DB handles:\\n", out.stdout)

for db_path in ['/opt/absolutdashboard/app.db', '/var/lib/absolutdashboard/app.db', '/var/opt/absolutdashboard/app.db']:
    if os.path.exists(db_path):
        con = sqlite3.connect(db_path)
        cur = con.cursor()
        print(f"=== {db_path} ===")
        cur.execute("SELECT Id, Name, IpAddress, Port, SshUsername FROM Devices;")
        for r in cur.fetchall():
            print(f"  ID={r[0]} | Name={r[1]} | IP={r[2]} | Port={r[3]} | User={r[4]}")
        con.close()
"""
    import base64
    b64 = base64.b64encode(py_db.encode()).decode()
    _, stdout, stderr = c7.exec_command(f'python3 -c "import base64, os; exec(base64.b64decode(\'{b64}\').decode())"')
    print(stdout.read().decode('utf-8', errors='replace'))
    err = stderr.read().decode('utf-8', errors='replace')
    if err:
        print("ERR:", err)
    c7.close()

if __name__ == "__main__":
    run()
