import os
import sys
import subprocess
import paramiko

REMOTE_HOST = os.environ.get("REMOTE_SSH_HOST", "10.20.110.7")
REMOTE_USER = os.environ.get("REMOTE_SSH_USER", "root")
REMOTE_PASS = os.environ.get("REMOTE_SSH_PASSWORD", "Absolut1.")

def copy_to_clipboard(text):
    try:
        process = subprocess.Popen(['clip'], stdin=subprocess.PIPE, close_fds=True)
        process.communicate(input=text.encode('utf-8', errors='replace'))
    except Exception as e:
        print(f"[!] Could not copy to clipboard: {e}")

def run_remote_fix():
    output_buffer = []

    def log(msg):
        print(msg)
        output_buffer.append(msg)

    log(f"[*] Connecting via SSH to {REMOTE_USER}@{REMOTE_HOST}...")
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
        log(f"[!] SSH connection failed ({e}). Skipping repair.")
        copy_to_clipboard("\n".join(output_buffer))
        return

    # Check the test instance's app.db vs production app.db
    cmd = (
        "echo '=== COMPARING TEST VS PROD APP.DB ===' && "
        "ls -la /var/lib/absolutdashboard-test/app.db /var/lib/absolutdashboard/app.db && "
        "echo '=== STARTING TEST SERVICE (PORT 5001) ===' && "
        "systemctl start absolutdashboard-test && "
        "sleep 4 && "
        "curl -i -s http://127.0.0.1:5001 | head -n 15"
    )

    log("[*] Testing the test instance on port 5001...")
    _, stdout, stderr = client.exec_command(cmd, timeout=30)
    out = stdout.read().decode('utf-8', errors='replace')
    err = stderr.read().decode('utf-8', errors='replace')
    client.close()

    if out:
        log(out)
    if err:
        log(err)

    full_text = "\n".join(output_buffer)
    copy_to_clipboard(full_text)
    print("\n[SUCCESS] The complete execution output has been copied to your clipboard!")

if __name__ == "__main__":
    run_remote_fix()
