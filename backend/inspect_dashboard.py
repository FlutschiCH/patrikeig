import sys
import os
import json

sys.path.append(os.path.dirname(os.path.abspath(__file__)))
from ssh_handler import SSHHandler

TARGET_HOST = "10.20.110.7"
SSH_USER = "root"
SSH_PASS = "Godzilla_12"

def print_banner(text):
    print("\n" + "=" * 65)
    print(f"  {text}")
    print("=" * 65)

def inspect_app():
    print_banner("1. INSPECTING APP DIRECTORY & CONFIG FILES")
    cmd1 = """
    ls -la /opt/absolutdashboard-test/
    echo '=== appsettings.json ==='
    cat /opt/absolutdashboard-test/appsettings.json 2>/dev/null || cat /opt/absolutdashboard-test/appsettings.Production.json 2>/dev/null || echo 'appsettings not found'
    """
    res1 = SSHHandler.execute_command(host=TARGET_HOST, user=SSH_USER, password=SSH_PASS, command=cmd1)
    print(res1.get("stdout"))

    print_banner("2. CHECKING SYSTEMD SERVICE & LOGS (WHY 500 ERROR)")
    cmd2 = """
    echo '=== Service Status ==='
    systemctl status absolutdashboard* 2>/dev/null || journalctl -u absolutdashboard* -n 25 --no-pager 2>/dev/null || echo 'No systemd unit'
    echo '=== Recent Logs / Crash Info ==='
    journalctl _PID=$(pgrep -o AbsolutDashboar) -n 30 --no-pager 2>/dev/null || echo 'No journal for pid'
    """
    res2 = SSHHandler.execute_command(host=TARGET_HOST, user=SSH_USER, password=SSH_PASS, command=cmd2)
    print(res2.get("stdout"))

    print_banner("3. DISCOVERING CONTROLLERS & API ROUTES")
    cmd3 = """
    cd /opt/absolutdashboard-test/
    echo '=== DLLs & Structure ==='
    ls *.dll 2>/dev/null | head -n 30
    echo ''
    echo '=== Searching for Route strings / endpoints in DLLs ==='
    strings AbsolutDashboard.Web.dll 2>/dev/null | grep -E '^api/|^/api/|Controller' | head -n 40
    """
    res3 = SSHHandler.execute_command(host=TARGET_HOST, user=SSH_USER, password=SSH_PASS, command=cmd3)
    print(res3.get("stdout"))

    print_banner("4. TESTING HTTPS PORT 5001 & OTHER SUB-PATHS")
    cmd4 = """
    echo '--- curl https://127.0.0.1:5001/ -k ---'
    curl -k -Is https://127.0.0.1:5001/ || echo 'https probe failed'
    echo ''
    echo '--- curl http://127.0.0.1:5001/swagger ---'
    curl -Is http://127.0.0.1:5001/swagger/ || echo 'No swagger on root'
    echo '--- curl http://127.0.0.1:5001/api/status ---'
    curl -Is http://127.0.0.1:5001/api/status || echo 'No /api/status'
    echo '--- curl http://127.0.0.1:5001/health ---'
    curl -Is http://127.0.0.1:5001/health || echo 'No /health'
    """
    res4 = SSHHandler.execute_command(host=TARGET_HOST, user=SSH_USER, password=SSH_PASS, command=cmd4)
    print(res4.get("stdout"))

if __name__ == "__main__":
    inspect_app()
