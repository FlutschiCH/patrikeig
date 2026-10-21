import subprocess
import os

def run(cmd):
    res = subprocess.run(cmd, shell=True, text=True, capture_output=True)
    return res.stdout.strip() + "\n" + res.stderr.strip()

print("=== REMOTE FIX EXECUTION FROM RUN_LOOP ===")

# Run the full repair directly on the AnyDesk machine
cmd = """
mkdir -p /var/lib/absolutdashboard/keys &&
cp -f /root/.aspnet/DataProtection-Keys/*.xml /var/lib/absolutdashboard/keys/ 2>/dev/null || true;
chown -R absolutsrf:absolutsrf /var/lib/absolutdashboard &&
chmod 750 /var/lib/absolutdashboard &&
chmod 700 /var/lib/absolutdashboard/keys &&
chmod 600 /var/lib/absolutdashboard/keys/*.xml 2>/dev/null || true;
rm -f /var/lib/absolutdashboard/.dp-cache.st /etc/absolutdashboard/.dp-cache.st /opt/absolutdashboard/.dp-cache.st &&
systemctl restart absolutdashboard &&
sleep 4 &&
echo '=== SERVICE STATUS ===' &&
systemctl status absolutdashboard --no-pager &&
echo '=== LOCAL HTTP TEST ===' &&
curl -i -s http://127.0.0.1:5000 &&
echo '=== DETAILED TRACE ===' &&
journalctl -u absolutdashboard -n 30 --no-pager
"""

print(run(cmd))
