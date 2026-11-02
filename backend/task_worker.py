import os
import sys
import subprocess

if sys.platform == "win32":
    sys.stdout.reconfigure(encoding="utf-8", errors="replace")
    sys.stderr.reconfigure(encoding="utf-8", errors="replace")

def run():
    print("=== 1. LOCAL PING FROM 10.20.110.98 ===")
    targets = [
        "10.20.110.7",   # Dashboard server
        "10.20.110.4",   # Proxmox host
        "10.20.110.5",   # 5G Core VM
        "10.20.110.8",   # BBU
        "10.20.110.1",   # Possible router gateway
        "10.20.110.254"  # Possible switch gateway
    ]
    for ip in targets:
        cmd = f"ping -n 2 -w 1000 -S 10.20.110.98 {ip}"
        res = subprocess.run(cmd, shell=True, capture_output=True, text=True, encoding="utf-8", errors="replace")
        ok = "TTL=" in (res.stdout or "")
        print(f"Ping -> {ip:15} : {'ONLINE [OK]' if ok else 'TIMEOUT / UNREACHABLE'}")

    print("\n=== 2. CHECK ARP TABLE FOR 10.20.110.x NEIGHBORS ===")
    arp_res = subprocess.run("arp -a", shell=True, capture_output=True, text=True, encoding="utf-8", errors="replace")
    for line in (arp_res.stdout or "").splitlines():
        if "10.20.110." in line:
            print(" ", line.strip())

    print("\n=== 3. CHECK LINUX DASHBOARD HOST (10.20.110.7) VIA SSH ===")
    try:
        import paramiko
        client = paramiko.SSHClient()
        client.set_missing_host_key_policy(paramiko.AutoAddPolicy())
        client.connect("10.20.110.7", port=22, username="root", password="Absolut1.", timeout=5)
        _, stdout, _ = client.exec_command("ip addr; ip route; ip neigh")
        out = stdout.read().decode("utf-8", errors="replace")
        client.close()
        print("--- Linux 10.20.110.7 Interfaces & Routes ---")
        print(out)
    except Exception as e:
        print(f"SSH to 10.20.110.7 failed: {e}")

if __name__ == "__main__":
    run()
