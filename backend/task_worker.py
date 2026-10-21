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

    # Check the exact encrypted fields: SnmpCommunityEnc, SshPasswordEnc
    # When DataProtection keys were restored, can these fields actually be decrypted?
    py_code = """
import sqlite3

con = sqlite3.connect('/var/lib/absolutdashboard/app.db')
cur = con.cursor()

print('=== ENCRYPTED CREDENTIALS CHECK ===')
devices_with_creds = cur.execute('SELECT Id, Name, SshUsername, SshPasswordEnc, SnmpCommunityEnc FROM Devices WHERE SshPasswordEnc IS NOT NULL OR SnmpCommunityEnc IS NOT NULL;').fetchall()
print(f'Devices with encrypted creds: {len(devices_with_creds)}')
for d in devices_with_creds[:5]:
    pwd_len = len(d[3]) if d[3] else 0
    snmp_len = len(d[4]) if d[4] else 0
    print(f'  Device {d[0]} ({d[1]}): SshUser={d[2]}, PwdLen={pwd_len}, SnmpLen={snmp_len}')

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
