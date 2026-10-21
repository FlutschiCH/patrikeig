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

    # Check the exact Devices table and inspect parents / foreign relations
    py_code = """
import sqlite3

con = sqlite3.connect('/var/lib/absolutdashboard/app.db')
cur = con.cursor()

print('=== DEVICES COLUMNS ===')
cols = [c[1] for c in cur.execute('PRAGMA table_info(Devices);').fetchall()]
print(cols)

print('\\n=== CHECKING ParentDeviceId RELATIONSHIPS ===')
# Are there any devices whose ParentDeviceId points to a non-existent device?
if 'ParentDeviceId' in cols:
    bad_parents = cur.execute('SELECT Id, Name, ParentDeviceId FROM Devices WHERE ParentDeviceId IS NOT NULL AND ParentDeviceId NOT IN (SELECT Id FROM Devices);').fetchall()
    print('Devices with non-existent ParentDeviceId:', bad_parents)

# Are there any Devices with NULL or invalid CategoryId?
bad_cats = cur.execute('SELECT Id, Name, CategoryId FROM Devices WHERE CategoryId IS NULL OR CategoryId NOT IN (SELECT Id FROM DeviceCategories);').fetchall()
print('Devices with invalid CategoryId:', bad_cats)

# Are there any Devices with NULL or invalid TemplateId?
bad_tmpls = cur.execute('SELECT Id, Name, TemplateId FROM Devices WHERE TemplateId IS NULL OR TemplateId NOT IN (SELECT Id FROM DeviceTemplates);').fetchall()
print('Devices with invalid TemplateId:', bad_tmpls)

print('\\n=== TOTAL DEVICES DETAILS ===')
for d in cur.execute('SELECT Id, Name, CategoryId, TemplateId, Enabled FROM Devices;').fetchall():
    print(' ', d)

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
