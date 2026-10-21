import time
import subprocess
import os
import sys

REPO_DIR = os.path.dirname(os.path.abspath(__file__))
TASK_SCRIPT = os.path.join(REPO_DIR, "backend", "task_worker.py")
OUT_FILE = os.path.join(REPO_DIR, "task_output.txt")

def run_cmd(cmd, cwd=REPO_DIR):
    res = subprocess.run(cmd, shell=True, cwd=cwd, capture_output=True, text=True)
    return res.returncode, res.stdout, res.stderr

print("==================================================")
print("   PATRIKEIG ANYDESK AUTO-SYNC LOOP ACTIVE")
print("==================================================")
print(f"Monitoring repository in: {REPO_DIR}")
print("Auto-fetching new commits every 4 seconds...")

last_commit = None

while True:
    try:
        # 1. Fetch latest changes from GitHub origin/main
        run_cmd("git fetch origin main")
        code, out, _ = run_cmd("git rev-parse origin/main")
        remote_commit = out.strip()

        if remote_commit and remote_commit != last_commit:
            print(f"\n[+] New instruction detected ({remote_commit[:7]}). Pulling...")
            run_cmd("git reset --hard origin/main")
            last_commit = remote_commit

            # 2. Execute backend/task_worker.py if it exists
            if os.path.exists(TASK_SCRIPT):
                print("[*] Executing task_worker.py...")
                code, stdout, stderr = run_cmd(f"python3 {TASK_SCRIPT}")
                
                output = f"=== TASK EXECUTION [{time.strftime('%Y-%m-%d %H:%M:%S')}] ===\n"
                output += f"Exit code: {code}\n"
                if stdout:
                    output += f"\n--- STDOUT ---\n{stdout}\n"
                if stderr:
                    output += f"\n--- STDERR ---\n{stderr}\n"

                print(output)
                with open(OUT_FILE, "w", encoding="utf-8") as f:
                    f.write(output)

                # 3. Commit and push the execution result back so the agent reads it instantly
                run_cmd("git add task_output.txt")
                run_cmd('git commit -m "Auto-report task execution result"')
                run_cmd("git push origin main")
                print("[*] Result uploaded to GitHub.")

        time.sleep(4)
    except KeyboardInterrupt:
        print("\nExiting loop.")
        break
    except Exception as e:
        print(f"[!] Error: {e}")
        time.sleep(4)
