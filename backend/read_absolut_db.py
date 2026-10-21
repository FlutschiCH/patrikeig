import sys
import os

sys.path.append(os.path.dirname(os.path.abspath(__file__)))
from ssh_handler import SSHHandler

TARGET_HOST = "10.20.110.7"
SSH_USER = "root"
SSH_PASS = "Godzilla_12"

def print_banner(text):
    print("\n" + "=" * 65)
    print(f"  {text}")
    print("=" * 65)

def check_db_and_routes():
    print_banner("1. CHECKING SQLITE DATABASE FILE & TABLES")
    cmd1 = """
    ls -lh /opt/absolutdashboard-test/Data/
    echo ''
    echo '=== Tables in app.db ==='
    sqlite3 /opt/absolutdashboard-test/Data/app.db ".tables" 2>/dev/null || python3 -c "import sqlite3; con=sqlite3.connect('/opt/absolutdashboard-test/Data/app.db'); print(con.execute(\\"SELECT name FROM sqlite_master WHERE type='table';\\").fetchall())"
    """
    res1 = SSHHandler.execute_command(host=TARGET_HOST, user=SSH_USER, password=SSH_PASS, command=cmd1)
    print(res1.get("stdout"))

    print_banner("2. CHECKING CONFIGURED DEVICES & QUERIES IN DATABASE")
    cmd2 = """
    python3 -c "
import sqlite3, json
try:
    con = sqlite3.connect('/opt/absolutdashboard-test/Data/app.db')
    cursor = con.cursor()
    cursor.execute('SELECT name FROM sqlite_master WHERE type=\\'table\\';')
    tables = [t[0] for t in cursor.fetchall()]
    print('Available Tables:', tables)
    for tbl in tables:
        if any(k in tbl.lower() for k in ['device', 'quer', 'metric', 'poll', 'snmp', 'config', 'sensor']):
            print(f'\\n--- Top 5 Rows in Table: {tbl} ---')
            cursor.execute(f'PRAGMA table_info({tbl});')
            cols = [col[1] for col in cursor.fetchall()]
            print('Columns:', cols)
            cursor.execute(f'SELECT * FROM {tbl} LIMIT 5;')
            rows = cursor.fetchall()
            for r in rows:
                print(r)
except Exception as e:
    print('DB Query error:', e)
"
    """
    res2 = SSHHandler.execute_command(host=TARGET_HOST, user=SSH_USER, password=SSH_PASS, command=cmd2)
    print(res2.get("stdout"))

    print_banner("3. LIVE EXCEPTION LOG (WHY 500 ON HTTP REQUEST)")
    cmd3 = """
    echo '--- Triggering curl while watching journalctl ---'
    curl -s http://127.0.0.1:5001/ > /dev/null
    journalctl -u absolutdashboard-test -n 25 --no-pager
    """
    res3 = SSHHandler.execute_command(host=TARGET_HOST, user=SSH_USER, password=SSH_PASS, command=cmd3)
    print(res3.get("stdout"))

if __name__ == "__main__":
    check_db_and_routes()
