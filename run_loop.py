import time
import subprocess
import os
import sys

REPO_DIR = os.path.dirname(os.path.abspath(__file__))
TASK_SCRIPT = os.path.join(REPO_DIR, "backend", "task_worker.py")
OUT_FILE = os.path.join(REPO_DIR, "task_output.txt")
PYTHON_EXE = sys.executable

def copy_to_clipboard(text):
    try:
        if sys.platform == "win32":
            p = subprocess.Popen(['clip'], stdin=subprocess.PIPE, close_fds=True)
            p.communicate(input=text.encode('utf-8', errors='replace'))
    except Exception as e:
        print(f"[!] Clipboard error: {e}")

def run_cmd(cmd, cwd=REPO_DIR):
    res = subprocess.run(cmd, shell=True, cwd=cwd, capture_output=True, text=True)
    return res.returncode, res.stdout.strip(), res.stderr.strip()

print("==================================================")
print("   PATRIKEIG ANYDESK AUTO-SYNC LOOP ACTIVE")
print("==================================================")
print(f"Monitoring repository in: {REPO_DIR}")
print(f"Using Python: {PYTHON_EXE}")

run_cmd('git config user.name "FlutschiCH"')
run_cmd('git config user.email "flutschich@gmail.com"')
print("Auto-fetching new commits every 3 seconds...")

# Initialize last_commit to the current HEAD so it NEVER triggers on its own past commits
_, head_rev, _ = run_cmd("git rev-parse HEAD")
last_commit = head_rev.strip()

while True:
    try:
        run_cmd("git fetch origin main")
        code, out, _ = run_cmd("git rev-parse origin/main")
        remote_commit = out.strip()

        # Check commit message: NEVER execute on Auto-report commits!
        _, log_msg, _ = run_cmd("git log -1 --pretty=%B origin/main")
        is_auto_report = "Auto-report" in (log_msg or "")

        if remote_commit and remote_commit != last_commit and not is_auto_report:
            print(f"\n[+] New instruction detected ({remote_commit[:7]}: {log_msg.splitlines()[0] if log_msg else ''}). Pulling...")
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

                copy_to_clipboard(output)
                print("[*] Output copied to clipboard.")

                # Push the task output back to git
                run_cmd("git add -f task_output.txt")
                run_cmd('git commit -m "Auto-report task execution result"')
                run_cmd("git pull --rebase origin main")
                c_push, o_push, e_push = run_cmd("git push origin main")
                print(f"[*] Git push status: code={c_push}, err={e_push}")
                
                # Immediately update last_commit to the pushed commit to prevent re-triggering
                _, new_head, _ = run_cmd("git rev-parse HEAD")
                last_commit = new_head.strip()

        time.sleep(3)
    except KeyboardInterrupt:
        print("\nExiting loop.")
        break
    except Exception as e:
        print(f"[!] Error: {e}")
        time.sleep(3)
