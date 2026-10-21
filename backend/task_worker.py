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

    # Check the actual database tables: DevicePollResult, DeviceTemplates, etc.
    # In earlier logs:
    # "AbsolutDashboard.Web.Services.Integrity.IntegrityGate:
    #  Database integrity constraint violation: foreign key relationship could not be validated during connection pool initialization"
    # Let's inspect the exact database queries IntegrityGate runs on startup!
    py_code = """
import sqlite3

con = sqlite3.connect('/var/lib/absolutdashboard/app.db')
cur = con.cursor()

# Find any table with 0 rows or foreign keys
tables = [r[0] for r in cur.execute("SELECT name FROM sqlite_master WHERE type='table';").fetchall()]
print('=== ROW COUNTS PER TABLE ===')
for t in tables:
    if not t.startswith('sqlite_'):
        count = cur.execute(f"SELECT count(*) FROM \\"{t}\\";").fetchone()[0]
        print(f'{t:30}: {count}')

print('\\n=== CHECKING IF DeviceCategories or Templates ARE EMPTY ===')
cats = cur.execute('SELECT Id, Key, Name FROM DeviceCategories;').fetchall()
print('Categories:', cats)

tmpls = cur.execute('SELECT Id, Name, CategoryId FROM DeviceTemplates;').fetchall()
print('Templates:', tmpls)

con.close()
"""
    import base64
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
