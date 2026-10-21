import sys
import os
import subprocess

sys.path.append(os.path.dirname(os.path.abspath(__file__)))
from ssh_handler import SSHHandler

TARGET_HOST = "10.20.110.7"
SSH_USER = "root"
SSH_PASS = "Godzilla_12"

def print_banner(text, accumulator):
    b = "\n" + "=" * 65 + "\n" + f"  {text}\n" + "=" * 65 + "\n"
    print(b)
    accumulator.append(b)

def copy_to_windows_clipboard(text):
    """Copies text directly to Windows clipboard via clip command."""
    try:
        process = subprocess.Popen('clip', stdin=subprocess.PIPE, shell=True)
        process.communicate(text.encode('utf-8'))
        print("\n" + "*" * 65)
        print("  [SUCCESS] All output copied automatically to your clipboard!")
        print("  You can now simply press CTRL + V anywhere to paste.")
        print("*" * 65 + "\n")
    except Exception as e:
        print(f"[!] Clipboard copy failed: {e}")

def check_db_and_routes():
    output = []

    print_banner("1. CHECKING SQLITE DATABASE FILE & TABLES", output)
    cmd1 = """
    ls -lh /opt/absolutdashboard-test/Data/
    echo ''
    echo '=== Tables in app.db ==='
    python3 -c "import sqlite3; con=sqlite3.connect('/opt/absolutdashboard-test/Data/app.db'); print(con.execute(\\"SELECT name FROM sqlite_master WHERE type='table';\\").fetchall())"
    """
    res1 = SSHHandler.execute_command(host=TARGET_HOST, user=SSH_USER, password=SSH_PASS, command=cmd1)
    stdout1 = res1.get("stdout", "")
    print(stdout1)
    output.append(stdout1)

    print_banner("2. CHECKING CONFIGURED DEVICES & QUERIES IN DATABASE", output)
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
        if any(k in tbl.lower() for k in ['device', 'quer', 'metric', 'poll', 'snmp', 'config', 'sensor', 'item']):
            print(f'\\n--- Table Schema: {tbl} ---')
            cursor.execute(f'PRAGMA table_info({tbl});')
            cols = [col[1] for col in cursor.fetchall()]
            print('Columns:', cols)
            print(f'--- All Rows in Table: {tbl} ---')
            cursor.execute(f'SELECT * FROM {tbl};')
            rows = cursor.fetchall()
            for r in rows:
                print(r)
except Exception as e:
    print('DB Query error:', e)
"
    """
    res2 = SSHHandler.execute_command(host=TARGET_HOST, user=SSH_USER, password=SSH_PASS, command=cmd2)
    stdout2 = res2.get("stdout", "")
    print(stdout2)
    output.append(stdout2)

    print_banner("3. LIVE EXCEPTION LOG (KESTREL 500 REASON)", output)
    cmd3 = """
    curl -s http://127.0.0.1:5001/ > /dev/null
    journalctl -u absolutdashboard-test -n 20 --no-pager
    """
    res3 = SSHHandler.execute_command(host=TARGET_HOST, user=SSH_USER, password=SSH_PASS, command=cmd3)
    stdout3 = res3.get("stdout", "")
    print(stdout3)
    output.append(stdout3)

    # Automatically copy everything to clipboard
    full_text = "\n".join(output)
    copy_to_windows_clipboard(full_text)

if __name__ == "__main__":
    check_db_and_routes()
