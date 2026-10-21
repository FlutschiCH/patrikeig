import time
import subprocess
import os
import sys

REPO_DIR = os.path.dirname(os.path.abspath(__file__))
TASK_SCRIPT = os.path.join(REPO_DIR, "backend", "task_worker.py")
OUT_FILE = os.path.join(REPO_DIR, "task_output.txt")
PYTHON_EXE = sys.executable

def copy_to_clipboard(text):
    """Copies text to Windows clipboard via clip command."""
    try:
        if sys.platform == "win32":
            p = subprocess.Popen(['clip'], stdin=subprocess.PIPE, close_fds=True)
            p.communicate(input=text.encode('utf-8', errors='replace'))
    except Exception as e:
        print(f"[!] Clipboard error: {e}")

def run_cmd(cmd, cwd=REPO_DIR):
    res = subprocess.run(cmd, shell=True, cwd=cwd, capture_output=True, text=True)
    return res.returncode, res.stdout, res.stderr

print("==================================================")
print("   PATRIKEIG ANYDESK AUTO-SYNC LOOP ACTIVE")
print("==================================================")
print(f"Monitoring repository in: {REPO_DIR}")
print(f"Using Python: {PYTHON_EXE}")
print("Auto-fetching new commits every 4 seconds...")

last_commit = None

while True:
    try:
        run_cmd("git fetch origin main")
        code, out, _ = run_cmd("git rev-parse origin/main")
        remote_commit = out.strip()

        if remote_commit and remote_commit != last_commit:
            print(f"\n[+] New instruction detected ({remote_commit[:7]}). Pulling...")
            run_cmd("git reset --hard origin/main")
            last_commit = remote_commit

            if os.path.exists(TASK_SCRIPT):
                print("[*] Executing task_worker.py...")
                code, stdout, stderr = run_cmd(f'"{PYTHON_EXE}" "{TASK_SCRIPT}"')
                
                output = f"=== TASK EXECUTION [{time.strftime('%Y-%m-%d %H:%M:%S')}] ===\n"
                output += f"Exit code: {code}\n"
                if stdout:
                    output += f"\n--- STDOUT ---\n{stdout}\n"
                if stderr:
                    output += f"\n--- STDERR ---\n{stderr}\n"

                print(output)
                with open(OUT_FILE, "w", encoding="utf-8") as f:
                    f.write(output)

                # Always copy to clipboard immediately!
                copy_to_clipboard(output)
                print("[*] OUTPUT COPIED TO CLIPBOARD!")

                # Try pushing to git, but don't hang if unauthenticated
                run_cmd("git add task_output.txt")
                run_cmd('git commit -m "Auto-report task execution result"')
                run_cmd("git push origin main")

        time.sleep(4)
    except KeyboardInterrupt:
        print("\nExiting loop.")
        break
    except Exception as e:
        print(f"[!] Error: {e}")
        time.sleep(4)
