import os
import sys
import paramiko
import base64

if sys.platform == "win32":
    sys.stdout.reconfigure(encoding="utf-8", errors="replace")
    sys.stderr.reconfigure(encoding="utf-8", errors="replace")

REMOTE_HOST = os.environ.get("REMOTE_SSH_HOST", "10.20.110.7")
REMOTE_USER = os.environ.get("REMOTE_SSH_USER", "root")
REMOTE_PASS = os.environ.get("REMOTE_SSH_PASSWORD", "Absolut1.")

def run():
    client = paramiko.SSHClient()
    client.set_missing_host_key_policy(paramiko.AutoAddPolicy())
    try:
        client.connect(hostname=REMOTE_HOST, port=22, username=REMOTE_USER, password=REMOTE_PASS, timeout=8)
    except Exception as e:
        print(f"[!] SSH connection failed: {e}")
        return

    py_code = """
import sqlite3
import os
import subprocess

# Find all SQLite DBs and inspect Devices table
db_candidates = [
    '/opt/absolutdashboard/app.db',
    '/opt/absolutdashboard-test/app.db',
    '/var/opt/absolutdashboard/app.db',
    '/var/lib/absolutdashboard-test/app.db'
]

# Search for any sqlite files on disk
out = subprocess.run(['find', '/opt', '/var', '/root', '-name', '*.db'], capture_output=True, text=True)
for l in out.stdout.splitlines():
    if l.strip() and l.strip() not in db_candidates:
        db_candidates.append(l.strip())

for db_path in db_candidates:
    if os.path.exists(db_path):
        try:
            con = sqlite3.connect(db_path)
            cur = con.cursor()
            cur.execute("SELECT name FROM sqlite_master WHERE type='table' AND name='Devices';")
            if cur.fetchone():
                print(f"=== DB FOUND WITH DEVICES: {db_path} ===")
                cur.execute("SELECT Id, Name, IpAddress, LastReachable, LastPolledAt, LastSnapshotJson FROM Devices WHERE Name LIKE '%CORE%';")
                rows = cur.fetchall()
                for r in rows:
                    snap_len = len(r[5]) if r[5] else 0
                    print(f"  Device: Id={r[0]} | Name={r[1]} | IP={r[2]} | Reachable={r[3]} | LastPolled={r[4]} | SnapLen={snap_len}")
                    if r[5]:
                        print(f"  LastSnapshotJson preview: {r[5][:250]}")
            con.close()
        except Exception as e:
            pass

print("\\n=== CHECK SERVICE LOGS FOR POLLER ===")
log = subprocess.run(['journalctl', '-u', 'absolutdashboard', '-n', '100', '--no-pager'], capture_output=True, text=True)
for line in log.stdout.splitlines():
    if any(k in line.lower() for k in ['poll', 'integrity', 'blocked', 'metric', 'error', 'fail']):
        print("  LOG:", line)
"""

    b64_code = base64.b64encode(py_code.encode('utf-8')).decode('ascii')
    cmd = f'python3 -c "import base64; exec(base64.b64decode(\'{b64_code}\').decode(\'utf-8\'))"'

    _, stdout, stderr = client.exec_command(cmd, timeout=30)
    out = stdout.read().decode('utf-8', errors='replace')
    err = stderr.read().decode('utf-8', errors='replace')
    client.close()

    if out:
        sys.stdout.buffer.write(out.encode('utf-8', errors='replace'))
        sys.stdout.buffer.write(b'\n')
    if err:
        sys.stderr.buffer.write(err.encode('utf-8', errors='replace'))
        sys.stderr.buffer.write(b'\n')

if __name__ == "__main__":
    run()
