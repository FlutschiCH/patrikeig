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
    print(f"[*] Task worker connecting to {REMOTE_USER}@{REMOTE_HOST}...")
    client = paramiko.SSHClient()
    client.set_missing_host_key_policy(paramiko.AutoAddPolicy())

    try:
        client.connect(
            hostname=REMOTE_HOST,
            port=22,
            username=REMOTE_USER,
            password=REMOTE_PASS,
            timeout=8,
            look_for_keys=True,
            allow_agent=True
        )
    except Exception as e:
        print(f"[!] SSH connection failed: {e}")
        return

    # Check dashboard database, 5G Core connection, and poller logs
    py_code = """
import subprocess
import os
import sqlite3
import datetime

print("=== 1. SERVICE STATUS & LOGS ===")
res = subprocess.run(['systemctl', 'status', 'absolutdashboard'], capture_output=True, text=True)
print(res.stdout[:1500])

print("\\n=== 2. RECENT JOURNAL LOGS (POLLER / ERRORS) ===")
jlog = subprocess.run(['journalctl', '-u', 'absolutdashboard', '-n', '40', '--no-pager'], capture_output=True, text=True)
print(jlog.stdout)

print("\\n=== 3. NETWORK REACHABILITY FROM HOST ===")
# Ping 5G Core (10.20.111.30)
p_core = subprocess.run(['ping', '-c', '2', '-W', '2', '10.20.111.30'], capture_output=True, text=True)
print("Ping 10.20.111.30 (5G CORE):", "OK" if p_core.returncode == 0 else f"FAILED (exit {p_core.returncode})")

# Ping BBU (10.20.110.8)
p_bbu = subprocess.run(['ping', '-c', '2', '-W', '2', '10.20.110.8'], capture_output=True, text=True)
print("Ping 10.20.110.8 (BBU):", "OK" if p_bbu.returncode == 0 else f"FAILED (exit {p_bbu.returncode})")

print("\\n=== 4. DATABASE CHECK (DEVICES & LAST POLLED) ===")
db_paths = [
    '/opt/absolutdashboard/app.db',
    '/opt/absolutdashboard-test/app.db',
    '/var/opt/absolutdashboard/app.db'
]
# Also find dynamically if not in fixed list
found_db = None
for p in db_paths:
    if os.path.exists(p):
        found_db = p
        break

if not found_db:
    out = subprocess.run(['find', '/opt', '/var', '-name', '*.db'], capture_output=True, text=True)
    for line in out.stdout.splitlines():
        if 'absolut' in line.lower() or 'app.db' in line.lower():
            found_db = line.strip()
            break

print("Using DB:", found_db)
if found_db and os.path.exists(found_db):
    try:
        con = sqlite3.connect(found_db)
        cur = con.cursor()
        
        print("\\n-- Devices in DB --")
        cur.execute("SELECT Id, Name, IpAddress, LastReachable, LastPolledAt, LastError FROM Devices;")
        devices = cur.fetchall()
        for d in devices:
            print(f"Device ID: {d[0]} | Name: {d[1]} | IP: {d[2]} | Reachable: {d[3]} | LastPolled: {d[4]} | Error: {d[5]}")

        print("\\n-- ThroughputSamples count & recent rows --")
        cur.execute("SELECT COUNT(*) FROM ThroughputSamples;")
        print("ThroughputSamples count:", cur.fetchone()[0])
        cur.execute("SELECT * FROM ThroughputSamples ORDER BY Timestamp DESC LIMIT 5;")
        for row in cur.fetchall():
            print("  Sample:", row)

        print("\\n-- DevicePollResults count --")
        cur.execute("SELECT COUNT(*) FROM DevicePollResults;")
        print("DevicePollResults count:", cur.fetchone()[0])
        cur.execute("SELECT DeviceId, Timestamp, Reachable, SnapshotJson FROM DevicePollResults ORDER BY Timestamp DESC LIMIT 3;")
        for row in cur.fetchall():
            snap_preview = (row[3][:100] + '...') if row[3] else 'None'
            print(f"  PollResult: Device={row[0]} | Time={row[1]} | Reachable={row[2]} | Snap={snap_preview}")

        con.close()
    except Exception as dbe:
        print("DB query error:", dbe)
else:
    print("Database not found!")
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
