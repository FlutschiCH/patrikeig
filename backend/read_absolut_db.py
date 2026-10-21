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
    try:
        process = subprocess.Popen('clip', stdin=subprocess.PIPE, shell=True)
        process.communicate(text.encode('utf-8'))
        print("\n" + "*" * 65)
        print("  [SUCCESS] All output copied automatically to your clipboard!")
        print("  You can now simply press CTRL + V anywhere to paste.")
        print("*" * 65 + "\n")
    except Exception as e:
        print(f"[!] Clipboard copy failed: {e}")

def locate_database():
    output = []

    print_banner("1. FINDING ALL .DB / SQLITE FILES ON SYSTEM", output)
    cmd1 = """
    echo '=== Searching for app.db & *.db across /opt and /root ==='
    find /opt /var /root /etc -type f \\( -name "*.db" -o -name "*.sqlite*" \\) 2>/dev/null
    echo ''
    echo '=== Open file handles of AbsolutDashboard process ==='
    ls -l /proc/$(pgrep -o AbsolutDashboar)/fd/ 2>/dev/null | grep -E '\\.db|\\.sqlite|Data|log'
    echo ''
    echo '=== Current Working Directory of AbsolutDashboard ==='
    readlink -f /proc/$(pgrep -o AbsolutDashboar)/cwd
    """
    res1 = SSHHandler.execute_command(host=TARGET_HOST, user=SSH_USER, password=SSH_PASS, command=cmd1)
    stdout1 = res1.get("stdout", "")
    print(stdout1)
    output.append(stdout1)

    print_banner("2. INSPECTING FOUND DATABASE TABLES & CONTENT", output)
    cmd2 = """
    DB_PATH=$(ls -l /proc/$(pgrep -o AbsolutDashboar)/fd/ 2>/dev/null | grep -o '/.*\\.db' | head -n 1)
    if [ -z "$DB_PATH" ]; then
        DB_PATH=$(find /opt/absolutdashboard-test /var/opt -name "*.db" 2>/dev/null | head -n 1)
    fi
    echo "Detected DB Path: $DB_PATH"
    
    if [ -n "$DB_PATH" ] && [ -f "$DB_PATH" ]; then
        python3 -c "
import sqlite3
con = sqlite3.connect('$DB_PATH')
cur = con.cursor()
cur.execute('SELECT name FROM sqlite_master WHERE type=\\'table\\';')
tables = [t[0] for t in cur.fetchall()]
print('Tables:', tables)
for tbl in tables:
    if not tbl.startswith('__') and not tbl.startswith('sqlite_'):
        print(f'\\n>>> Table: {tbl} <<<')
        cur.execute(f'PRAGMA table_info({tbl});')
        print('Columns:', [c[1] for c in cur.fetchall()])
        cur.execute(f'SELECT * FROM {tbl} LIMIT 10;')
        for row in cur.fetchall():
            print(row)
"
    else
        echo "No SQLite database file found on disk."
    fi
    """
    res2 = SSHHandler.execute_command(host=TARGET_HOST, user=SSH_USER, password=SSH_PASS, command=cmd2)
    stdout2 = res2.get("stdout", "")
    print(stdout2)
    output.append(stdout2)

    full_text = "\n".join(output)
    copy_to_windows_clipboard(full_text)

if __name__ == "__main__":
    locate_database()
