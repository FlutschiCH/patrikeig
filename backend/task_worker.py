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

    # Check why IntegrityGate threw foreign key violation during pool init:
    # We will query all foreign key references between tables, check for nulls or mismatches,
    # and inspect the exact database table definitions.
    cmd = """
python3 - <<'EOF'
import sqlite3

con = sqlite3.connect('/var/lib/absolutdashboard/app.db')
cur = con.cursor()

print("=== CHECKING ALL TABLES AND SCHEMAS ===")
tables = [r[0] for r in cur.execute("SELECT name FROM sqlite_master WHERE type='table';").fetchall()]
print("Tables:", tables)

print("\n=== FOREIGN KEYS PRAGMAS ===")
for t in tables:
    fks = cur.execute(f"PRAGMA foreign_key_list('{t}');").fetchall()
    if fks:
        print(f"Table {t} foreign keys: {fks}")

print("\n=== FOREIGN KEY ENFORCEMENT CHECK ===")
cur.execute("PRAGMA foreign_keys = ON;")
try:
    violations = cur.execute("PRAGMA foreign_key_check;").fetchall()
    print("Violations with FK ON:", violations)
except Exception as e:
    print("FK Check error:", e)

print("\n=== CHECKING AspNetUsers ===")
users = cur.execute("SELECT Id, UserName, Email, IsOwner FROM AspNetUsers;").fetchall()
print("Users:", users)

print("\n=== CHECKING AspNetUserRoles ===")
roles = cur.execute("SELECT * FROM AspNetUserRoles;").fetchall()
print("UserRoles:", roles)

con.close()
EOF
"""

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
