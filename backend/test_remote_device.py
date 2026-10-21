import sys
import os
import json
import time

sys.path.append(os.path.dirname(os.path.abspath(__file__)))
from ssh_handler import SSHHandler
from probe_handler import ProbeHandler
from port_scanner import PortScanner

TARGET_HOST = "10.20.110.7"
TARGET_PORT = 5001
SSH_USER = "root"
SSH_PASS = "Absolut1."

def print_banner(text):
    print("\n" + "=" * 65)
    print(f"  {text}")
    print("=" * 65)

def run_diagnostics():
    print_banner("1. TESTING SSH CONNECTIVITY & CREDENTIALS")
    print(f"[*] Attempting SSH connection to {SSH_USER}@{TARGET_HOST} (Pass: {SSH_PASS})...")
    
    ssh_test = SSHHandler.execute_command(
        host=TARGET_HOST, 
        user=SSH_USER, 
        password=SSH_PASS, 
        command="whoami && hostname && uptime",
        timeout=6.0
    )

    if ssh_test.get("success"):
        print(f"[+] SSH SUCCESS! Connected as:\n    {ssh_test.get('stdout').strip()}")
    else:
        print(f"[-] SSH FAILED: {ssh_test.get('stderr')}")
        print("    (Note: If running on a remote AnyDesk machine, ensure network route to 10.20.110.7 is active)")

    print_banner("2. INSPECTING 'AbsolutDashboard' (PORT 5001) VIA SSH")
    if ssh_test.get("success"):
        inspect_cmd = """
        echo '--- Process Info ---'
        ps aux | grep -i AbsolutDashboar | grep -v grep
        echo '--- Cmdline & Path ---'
        cat /proc/$(pgrep -o AbsolutDashboar)/cmdline 2>/dev/null | tr '\\0' ' ' || echo 'N/A'
        echo ''
        echo '--- Local Curl Probe (Port 5001) ---'
        curl -Is http://127.0.0.1:5001/ || echo 'Curl 5001 failed'
        echo '--- HTML/JSON Preview (Port 5001) ---'
        curl -s http://127.0.0.1:5001/ | head -n 25 || true
        """
        proc_res = SSHHandler.execute_command(
            host=TARGET_HOST,
            user=SSH_USER,
            password=SSH_PASS,
            command=inspect_cmd,
            timeout=8.0
        )
        print(proc_res.get("stdout"))
    else:
        print("[!] Skipping remote internal check because SSH is not reachable from this environment.")

    print_banner("3. TESTING REMOTE HTTP / cURL PROBE ON PORT 5001")
    url = f"http://{TARGET_HOST}:{TARGET_PORT}/"
    print(f"[*] Sending HTTP GET to {url} ...")
    http_res = ProbeHandler.execute_request(url, timeout=3.5)
    print(f"< Status: {http_res.get('status_code')} {http_res.get('reason')}")
    if http_res.get("elapsed_ms") is not None:
        print(f"< Latency: {http_res.get('elapsed_ms')} ms")
    if http_res.get("content_type"):
        print(f"< Content-Type: {http_res.get('content_type')}")
    print("\n--- Response Body Preview ---")
    body_sample = (http_res.get("body") or "")[:400]
    print(body_sample)

    print_banner("4. QUICK TCP PORT SCAN (10.20.110.7)")
    print("[*] Probing common ports (22, 80, 443, 5001, 5201, 8080)...")
    ports_to_test = [22, 80, 443, 5001, 5201, 8080]
    open_ports = PortScanner.scan_ports(TARGET_HOST, ports=ports_to_test)
    if open_ports:
        print(f"[+] Found {len(open_ports)} open port(s):")
        for p in open_ports:
            print(f"    • Port {p['port']}/TCP ({p['service']}) is OPEN")
    else:
        print(f"[-] No ports reachable on {TARGET_HOST} from current network.")

    print("\n" + "=" * 65)
    print("  DIAGNOSTICS FINISHED")
    print("=" * 65 + "\n")

if __name__ == "__main__":
    run_diagnostics()
