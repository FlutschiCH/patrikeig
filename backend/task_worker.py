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

    # Let's restore NTP / current system time and verify the service continues to return 200 OK!
    py_code = """
import subprocess
import time

print("1. Re-enabling NTP synchronization...")
subprocess.run(['timedatectl', 'set-ntp', 'true'], check=True)
time.sleep(2)

date_r = subprocess.run(['timedatectl', 'status'], capture_output=True, text=True)
print("Current timedatectl status:")
print(date_r.stdout)

print("2. Restarting absolutdashboard service with current time...")
subprocess.run(['systemctl', 'restart', 'absolutdashboard'], check=True)
time.sleep(3)

print("3. Testing HTTP response with current time...")
r = subprocess.run(['curl', '-i', '-L', 'http://127.0.0.1:5000/'], capture_output=True, text=True)
print("HTTP Status line:", r.stdout.splitlines()[0] if r.stdout else "No output")
for line in r.stdout.splitlines():
    if "HTTP/" in line or "Location:" in line:
        print(" ", line)

print("4. Testing external IP access http://10.20.110.7:5000/ ...")
r_ext = subprocess.run(['curl', '-i', '-s', 'http://10.20.110.7:5000/'], capture_output=True, text=True)
for line in r_ext.stdout.splitlines()[:5]:
    print(" ", line)
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
