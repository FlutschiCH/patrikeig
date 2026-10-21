import os
import sys
import paramiko

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

    # Check: What if the database has WAL files or SQLite foreign key checks failing under EF Core connection?
    # EF Core with SQLite: If a foreign key is enabled and violated in child tables (e.g. DevicePollResults -> Devices)
    py_code = """
import sqlite3

con = sqlite3.connect('/var/lib/absolutdashboard/app.db')
cur = con.cursor()

# Check DevicePollResults for orphaned records
bad_polls = cur.execute('SELECT count(*) FROM DevicePollResults WHERE DeviceId NOT IN (SELECT Id FROM Devices);').fetchone()[0]
print(f'DevicePollResults with non-existent DeviceId: {bad_polls}')

# Check ThroughputSamples for orphaned records
bad_samples = cur.execute('SELECT count(*) FROM ThroughputSamples WHERE DeviceId NOT IN (SELECT Id FROM Devices);').fetchone()[0]
print(f'ThroughputSamples with non-existent DeviceId: {bad_samples}')

if bad_polls > 0:
    print('Cleaning orphaned DevicePollResults...')
    cur.execute('DELETE FROM DevicePollResults WHERE DeviceId NOT IN (SELECT Id FROM Devices);')
    con.commit()
    print('Cleaned!')

if bad_samples > 0:
    print('Cleaning orphaned ThroughputSamples...')
    cur.execute('DELETE FROM ThroughputSamples WHERE DeviceId NOT IN (SELECT Id FROM Devices);')
    con.commit()
    print('Cleaned!')

con.close()
"""
    import base64
    b64_code = base64.b64encode(py_code.encode('utf-8')).decode('ascii')
    cmd = (
        f'python3 -c "import base64; exec(base64.b64decode(\'{b64_code}\').decode(\'utf-8\'))" && '
        'systemctl restart absolutdashboard && '
        'sleep 4 && '
        'echo "=== HTTP STATUS AFTER ORPHAN PURGE ===" && '
        'curl -i -s http://127.0.0.1:5000 | head -n 15'
    )

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
