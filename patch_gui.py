import tkinter as tk
from tkinter import ttk, scrolledtext, messagebox
import threading
import os
import sys
import paramiko
import base64

# Default known device IPs discovered in MobaXterm and network
PRESET_IPS = [
    "10.20.110.7",
    "10.20.110.1",
    "10.20.110.4",
    "10.20.110.5",
    "10.20.110.6",
    "10.20.110.15",
    "10.20.110.22",
    "10.20.110.30",
    "10.20.110.96",
    "10.20.110.100",
    "192.168.1.1"
]

class PatchToolGUI:
    def __init__(self, root):
        self.root = root
        self.root.title("AbsolutDashboard Multi-Device Patch & Deploy Tool")
        self.root.geometry("860x650")
        self.root.minsize(720, 520)
        self.root.configure(bg="#0d1117")

        style = ttk.Style()
        style.theme_use('clam')
        style.configure(".", background="#0d1117", foreground="#c9d1d9")

        # Header Frame
        header = tk.Frame(root, bg="#161b22", padx=16, pady=12)
        header.pack(fill=tk.X)

        title_lbl = tk.Label(header, text="AbsolutDashboard Multi-Device License Bypass Tool", font=("Segoe UI", 12, "bold"), bg="#161b22", fg="#58a6ff")
        title_lbl.pack(anchor="w")

        sub_lbl = tk.Label(header, text="Standalone Portable Mode — Select or enter device IP and apply the bypass directly via SSH", font=("Segoe UI", 9), bg="#161b22", fg="#8b949e")
        sub_lbl.pack(anchor="w")

        # Connection Config Frame
        conn_frame = tk.Frame(root, bg="#161b22", padx=16, pady=10)
        conn_frame.pack(fill=tk.X, pady=(1, 6))

        # Row 1: Target IP Dropdown / Input
        r1 = tk.Frame(conn_frame, bg="#161b22")
        r1.pack(fill=tk.X, pady=2)

        tk.Label(r1, text="Target Device IP:", font=("Consolas", 10, "bold"), bg="#161b22", fg="#58a6ff", width=18, anchor="w").pack(side=tk.LEFT)
        self.ip_combo = ttk.Combobox(r1, values=PRESET_IPS, font=("Consolas", 10), width=20)
        self.ip_combo.set("10.20.110.7")
        self.ip_combo.pack(side=tk.LEFT, padx=6)

        tk.Label(r1, text="SSH Port:", font=("Consolas", 9), bg="#161b22", fg="#8b949e").pack(side=tk.LEFT, padx=(12, 4))
        self.port_entry = tk.Entry(r1, bg="#0d1117", fg="#c9d1d9", font=("Consolas", 10), width=6)
        self.port_entry.insert(0, "22")
        self.port_entry.pack(side=tk.LEFT)

        # Row 2: Credentials
        r2 = tk.Frame(conn_frame, bg="#161b22")
        r2.pack(fill=tk.X, pady=6)

        tk.Label(r2, text="SSH Username:", font=("Consolas", 10), bg="#161b22", fg="#c9d1d9", width=18, anchor="w").pack(side=tk.LEFT)
        self.user_entry = tk.Entry(r2, bg="#0d1117", fg="#c9d1d9", font=("Consolas", 10), width=14)
        self.user_entry.insert(0, "root")
        self.user_entry.pack(side=tk.LEFT, padx=6)

        tk.Label(r2, text="Password:", font=("Consolas", 10), bg="#161b22", fg="#c9d1d9").pack(side=tk.LEFT, padx=(12, 4))
        self.pass_entry = tk.Entry(r2, bg="#0d1117", fg="#c9d1d9", font=("Consolas", 10), width=16, show="*")
        self.pass_entry.insert(0, "Absolut1.")
        self.pass_entry.pack(side=tk.LEFT, padx=4)

        # Action Buttons Toolbar
        btn_bar = tk.Frame(root, bg="#0d1117", padx=16, pady=8)
        btn_bar.pack(fill=tk.X)

        self.patch_btn = tk.Button(btn_bar, text="🚀 Apply Bypass Patch", bg="#238636", fg="#ffffff", font=("Segoe UI", 10, "bold"), activebackground="#2ea043", relief=tk.FLAT, padx=12, pady=4, command=self.start_patch)
        self.patch_btn.pack(side=tk.LEFT, padx=(0, 6))

        self.test_btn = tk.Button(btn_bar, text="🔍 Test Status & Ports", bg="#1f6feb", fg="#ffffff", font=("Segoe UI", 9, "bold"), activebackground="#388bfd", relief=tk.FLAT, padx=10, pady=4, command=self.start_status_check)
        self.test_btn.pack(side=tk.LEFT, padx=4)

        self.restart_btn = tk.Button(btn_bar, text="🔄 Restart Service", bg="#d29922", fg="#ffffff", font=("Segoe UI", 9), activebackground="#e3b341", relief=tk.FLAT, padx=10, pady=4, command=self.start_restart)
        self.restart_btn.pack(side=tk.LEFT, padx=4)

        self.copy_btn = tk.Button(btn_bar, text="📋 Copy Log", bg="#21262d", fg="#c9d1d9", font=("Segoe UI", 9), relief=tk.FLAT, padx=10, pady=4, command=self.copy_log)
        self.copy_btn.pack(side=tk.RIGHT, padx=4)

        self.clear_btn = tk.Button(btn_bar, text="Clear", bg="#21262d", fg="#8b949e", font=("Segoe UI", 9), relief=tk.FLAT, padx=8, pady=4, command=self.clear_log)
        self.clear_btn.pack(side=tk.RIGHT, padx=2)

        # Scrolled Log Area
        log_frame = tk.Frame(root, bg="#0d1117", padx=16, pady=4)
        log_frame.pack(fill=tk.BOTH, expand=True)

        self.log_area = scrolledtext.ScrolledText(log_frame, bg="#05080e", fg="#c9d1d9", insertbackground="#3fb950", font=("Consolas", 9), relief=tk.SOLID, borderwidth=1)
        self.log_area.pack(fill=tk.BOTH, expand=True)

        self.log("[*] Tool initialized in standalone portable mode.")
        self.log("[*] Select or enter target IP from the dropdown above, then click 'Apply Bypass Patch'.")
        self.log("[*] The tool automatically backs up the original DLL before applying the bypass.\n")

    def log(self, text):
        self.log_area.insert(tk.END, text + "\n")
        self.log_area.see(tk.END)

    def clear_log(self):
        self.log_area.delete('1.0', tk.END)

    def copy_log(self):
        txt = self.log_area.get('1.0', tk.END).strip()
        self.root.clipboard_clear()
        self.root.clipboard_append(txt)
        prev = self.copy_btn.cget("text")
        self.copy_btn.config(text="✓ Copied!", bg="#238636")
        self.root.after(1200, lambda: self.copy_btn.config(text=prev, bg="#21262d"))

    def get_ssh_client(self):
        ip = self.ip_combo.get().strip()
        port = int(self.port_entry.get().strip() or "22")
        user = self.user_entry.get().strip() or "root"
        password = self.pass_entry.get()

        client = paramiko.SSHClient()
        client.set_missing_host_key_policy(paramiko.AutoAddPolicy())
        client.connect(hostname=ip, port=port, username=user, password=password, timeout=8)
        return client, ip

    def start_patch(self):
        self.patch_btn.config(state=tk.DISABLED, text="Patching...")
        ip = self.ip_combo.get().strip()
        self.log(f"============================================================")
        self.log(f"[*] Starting License Bypass Deployment on {ip}...")

        def _worker():
            try:
                client, target_ip = self.get_ssh_client()
                self.root.after(0, lambda: self.log(f"[+] SSH connection established to {target_ip}"))

                # Remote script to search, backup, and patch all copies of AbsolutDashboard.Web.dll
                py_code = """
import os
import subprocess
import glob

# Find all instances of AbsolutDashboard.Web.dll
possible_paths = [
    '/opt/absolutdashboard/AbsolutDashboard.Web.dll',
    '/opt/absolutdashboard-test/AbsolutDashboard.Web.dll'
]
possible_paths.extend(glob.glob('/opt/*/AbsolutDashboard.Web.dll'))
possible_paths.extend(glob.glob('/home/*/deploy/*/AbsolutDashboard.Web.dll'))
possible_paths = sorted(list(set(p for p in possible_paths if os.path.exists(p))))

print(f'[*] Discovered DLL targets: {len(possible_paths)}')
offset = 18387
patched_body = bytes([0x0a, 0x03, 0x2a]) + bytes([0x00] * 22)

for target in possible_paths:
    backup = target + '.bak'
    if not os.path.exists(backup):
        with open(target, 'rb') as f:
            with open(backup, 'wb') as fb:
                fb.write(f.read())
        print(f'[+] Created backup: {backup}')
    else:
        print(f'[*] Existing backup found: {backup}')

    with open(target, 'rb') as f:
        dll = bytearray(f.read())

    # Check header
    if len(dll) > offset + 25:
        dll[offset:offset+25] = patched_body
        with open(target, 'wb') as f:
            f.write(dll)
        print(f'[+] Successfully patched {target} (IntegrityGate bypassed)')

# Restart all absolutdashboard services
svcs = ['absolutdashboard', 'absolutdashboard-test']
for svc in svcs:
    subprocess.run(['systemctl', 'restart', svc], capture_output=True)

import time
time.sleep(3)

print('\\n=== SERVICE STATUS ===')
for svc in svcs:
    res = subprocess.run(['systemctl', 'is-active', svc], capture_output=True, text=True)
    print(f'Service {svc}: {res.stdout.strip()}')

print('\\n=== LISTENING PORTS ===')
r = subprocess.run(['ss', '-tulpn'], capture_output=True, text=True)
for line in r.stdout.splitlines():
    if any(k in line for k in ['5000', '5001', 'Absolut']):
        print('  ' + line)

print('\\n=== HTTP PROBE (127.0.0.1) ===')
for port in [5000, 5001]:
    r = subprocess.run(['curl', '-i', '-s', f'http://127.0.0.1:{port}/'], capture_output=True, text=True)
    first_line = r.stdout.splitlines()[0] if r.stdout else 'Waiting for Kestrel initialization'
    print(f'Port {port}: {first_line}')
"""
                b64 = base64.b64encode(py_code.encode('utf-8')).decode('ascii')
                cmd = f'python3 -c "import base64; exec(base64.b64decode(\'{b64}\').decode(\'utf-8\'))"'

                stdin, stdout, stderr = client.exec_command(cmd, timeout=30)
                out = stdout.read().decode('utf-8', errors='replace')
                err = stderr.read().decode('utf-8', errors='replace')
                client.close()

                self.root.after(0, lambda: self._on_patch_done(out, err))
            except Exception as e:
                self.root.after(0, lambda: self._on_error(f"SSH/Execution failed: {str(e)}"))

        threading.Thread(target=_worker, daemon=True).start()

    def _on_patch_done(self, out, err):
        self.patch_btn.config(state=tk.NORMAL, text="🚀 Apply Bypass Patch")
        if out:
            self.log(out)
        if err:
            self.log(f"[!] STDERR: {err}")
        self.log(f"[*] Patch deployment routine completed.\n")

    def _on_error(self, err_msg):
        self.patch_btn.config(state=tk.NORMAL, text="🚀 Apply Bypass Patch")
        self.test_btn.config(state=tk.NORMAL, text="🔍 Test Status & Ports")
        self.restart_btn.config(state=tk.NORMAL, text="🔄 Restart Service")
        self.log(f"[-] ERROR: {err_msg}\n")
        messagebox.showerror("Error", err_msg)

    def start_status_check(self):
        self.test_btn.config(state=tk.DISABLED, text="Testing...")
        ip = self.ip_combo.get().strip()
        self.log(f"[*] Checking active services and ports on {ip}...")

        def _worker():
            try:
                client, target_ip = self.get_ssh_client()
                cmd = """
                echo "=== Services ==="
                systemctl is-active absolutdashboard absolutdashboard-test
                echo "=== Open Ports ==="
                ss -tulpn | grep -E '5000|5001|Absolut|iperf' || echo 'No ports matching 5000/5001'
                echo "=== HTTP Test ==="
                curl -i -s http://127.0.0.1:5000/ | head -n 3
                curl -i -s http://127.0.0.1:5001/ | head -n 3
                """
                stdin, stdout, stderr = client.exec_command(cmd, timeout=12)
                out = stdout.read().decode('utf-8', errors='replace')
                client.close()
                self.root.after(0, lambda: self._on_status_done(out))
            except Exception as e:
                self.root.after(0, lambda: self._on_error(str(e)))

        threading.Thread(target=_worker, daemon=True).start()

    def _on_status_done(self, out):
        self.test_btn.config(state=tk.NORMAL, text="🔍 Test Status & Ports")
        self.log(out + "\n")

    def start_restart(self):
        self.restart_btn.config(state=tk.DISABLED, text="Restarting...")
        ip = self.ip_combo.get().strip()
        self.log(f"[*] Restarting AbsolutDashboard services on {ip}...")

        def _worker():
            try:
                client, _ = self.get_ssh_client()
                cmd = "systemctl restart absolutdashboard absolutdashboard-test && sleep 2 && systemctl is-active absolutdashboard absolutdashboard-test"
                stdin, stdout, stderr = client.exec_command(cmd, timeout=15)
                out = stdout.read().decode('utf-8', errors='replace')
                client.close()
                self.root.after(0, lambda: self._on_restart_done(out))
            except Exception as e:
                self.root.after(0, lambda: self._on_error(str(e)))

        threading.Thread(target=_worker, daemon=True).start()

    def _on_restart_done(self, out):
        self.restart_btn.config(state=tk.NORMAL, text="🔄 Restart Service")
        self.log(f"[+] Services restarted:\n{out}\n")

if __name__ == "__main__":
    root = tk.Tk()
    app = PatchToolGUI(root)
    root.mainloop()
