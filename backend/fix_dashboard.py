import os
import sys
import glob
import shutil
import subprocess

STATE_KEYS_DIR = "/var/lib/absolutdashboard/keys"
ROOT_KEYS_DIR = "/root/.aspnet/DataProtection-Keys"
STATE_DIR = "/var/lib/absolutdashboard"
ETC_DIR = "/etc/absolutdashboard"
OPT_DIR = "/opt/absolutdashboard"
SERVICE = "absolutdashboard"

def run_cmd(cmd):
    """Executes a shell command and returns stdout."""
    res = subprocess.run(cmd, shell=True, text=True, capture_output=True)
    return res.returncode == 0, res.stdout.strip(), res.stderr.strip()

def repair():
    print("=== AbsolutDashboard Automated Repair ===")
    
    if os.geteuid() != 0:
        print("[!] Please run as root (sudo python3 fix_dashboard.py).")
        sys.exit(1)
        
    os.makedirs(STATE_KEYS_DIR, exist_ok=True)

    # 1. Consolidate all DataProtection XML keys across the system
    print("[*] Searching and syncing DataProtection keys...")
    key_sources = [
        ROOT_KEYS_DIR,
        "/var/lib/absolutdashboard-test/keys",
        "/home/absolutsrf/.aspnet/DataProtection-Keys"
    ]
    
    found_keys = []
    for src in key_sources:
        if os.path.exists(src):
            for key_file in glob.glob(os.path.join(src, "*.xml")):
                dest_file = os.path.join(STATE_KEYS_DIR, os.path.basename(key_file))
                shutil.copy2(key_file, dest_file)
                found_keys.append(os.path.basename(key_file))
                print(f"    Synced key: {os.path.basename(key_file)} from {src}")

    # 2. Reset permissions on key files and state directory
    print("[*] Correcting permissions...")
    run_cmd(f"chown -R absolutsrf:absolutsrf {STATE_DIR}")
    run_cmd(f"chmod 750 {STATE_DIR}")
    run_cmd(f"chmod 700 {STATE_KEYS_DIR}")
    run_cmd(f"chmod 600 {STATE_KEYS_DIR}/*.xml")

    # 3. Clean stale dp cache state files so it rebinds cleanly
    print("[*] Clearing stale token state files...")
    for st in [f"{STATE_DIR}/.dp-cache.st", f"{ETC_DIR}/.dp-cache.st", f"{OPT_DIR}/.dp-cache.st"]:
        if os.path.exists(st):
            os.remove(st)
            print(f"    Removed {st}")

    # 4. Restart service
    print(f"[*] Restarting {SERVICE}...")
    run_cmd(f"systemctl restart {SERVICE}")
    
    # 5. Verify local HTTP
    print("[*] Testing HTTP response...")
    success, out, _ = run_cmd("sleep 2 && curl -i http://127.0.0.1:5000")
    print(out)
    if "200 OK" in out or "302" in out:
        print("\n[SUCCESS] AbsolutDashboard is up and responding!")
    else:
        print("\n[INFO] Response received above.")

if __name__ == "__main__":
    repair()
