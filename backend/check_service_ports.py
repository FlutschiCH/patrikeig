import sys
import os
import paramiko

REMOTE_HOST = "10.20.110.7"
REMOTE_USER = "root"
REMOTE_PASS = "Absolut1."

client = paramiko.SSHClient()
client.set_missing_host_key_policy(paramiko.AutoAddPolicy())

try:
    client.connect(hostname=REMOTE_HOST, port=22, username=REMOTE_USER, password=REMOTE_PASS, timeout=8)
    
    cmd = """
    echo "=== 1. Active Listening Ports ==="
    ss -tulpn | grep -E '5000|5001|Absolut' || echo 'No ports matching 5000/5001'
    
    echo ""
    echo "=== 2. absolutdashboard journalctl ==="
    journalctl -u absolutdashboard -n 25 --no-pager
    
    echo ""
    echo "=== 3. absolutdashboard-test journalctl ==="
    journalctl -u absolutdashboard-test -n 25 --no-pager
    """
    
    stdin, stdout, stderr = client.exec_command(cmd, timeout=15)
    print(stdout.read().decode('utf-8', errors='replace'))
    print(stderr.read().decode('utf-8', errors='replace'))
    client.close()
except Exception as e:
    print(f"Error: {e}")
