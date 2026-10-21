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

    # Check: Why did IntegrityGate throw:
    # "Database integrity constraint violation: foreign key relationship could not be validated during connection pool initialization"
    # Let's inspect the actual SQLite foreign key constraints across ALL tables!
    py_code = """
import sqlite3

con = sqlite3.connect('/var/lib/absolutdashboard/app.db')
cur = con.cursor()

print('=== INSPECTING ALL FOREIGN KEYS IN ALL TABLES ===')
tables = [r[0] for r in cur.execute("SELECT name FROM sqlite_master WHERE type='table';").fetchall()]
for t in tables:
    fks = cur.execute(f"PRAGMA foreign_key_list('{t}');").fetchall()
    if fks:
        for fk in fks:
            # fk format: (id, seq, table, from, to, on_update, on_delete, match)
            parent_table = fk[2]
            from_col = fk[3]
            to_col = fk[4]
            # Check for orphaned rows
            query = f"SELECT count(*) FROM \\"{t}\\" WHERE \\"{from_col}\\" IS NOT NULL AND \\"{from_col}\\" NOT IN (SELECT \\"{to_col}\\" FROM \\"{parent_table}\\");"
            try:
                bad_count = cur.execute(query).fetchone()[0]
                if bad_count > 0:
                    print(f'VIOLATION: Table {t}.{from_col} -> {parent_table}.{to_col} has {bad_count} orphaned records!')
                else:
                    print(f'OK: Table {t}.{from_col} -> {parent_table}.{to_col}')
            except Exception as e:
                print(f'Error checking {t} -> {parent_table}: {e}')

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
