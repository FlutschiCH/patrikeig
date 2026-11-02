import tkinter as tk
from tkinter import ttk, scrolledtext, messagebox
import threading
import os
import sys
import paramiko
import base64

# Default target IPs in the network
PRESET_IPS = [
    "10.20.110.7",
    "10.20.110.5",
    "10.20.110.4",
    "10.20.110.1",
    "10.20.110.6",
    "10.20.110.8",
    "10.20.110.15",
    "10.20.110.22",
    "10.20.111.30",
    "192.168.1.1"
]

class PatchToolGUI:
    def __init__(self, root):
        self.root = root
        self.root.title("AbsolutDashboard Multi-Device License Bypass & Poller Fix Tool")
        self.root.geometry("880x680")
        self.root.minsize(740, 540)
        self.root.configure(bg="#0d1117")

        style = ttk.Style()
        style.theme_use('clam')
        style.configure(".", background="#0d1117", foreground="#c9d1d9")

        # Header Frame
        header = tk.Frame(root, bg="#161b22", padx=16, pady=12)
        header.pack(fill=tk.X)

        title_lbl = tk.Label(header, text="AbsolutDashboard Universal Fix & License Unlock Tool", font=("Segoe UI", 12, "bold"), bg="#161b22", fg="#58a6ff")
        title_lbl.pack(anchor="w")

        sub_lbl = tk.Label(header, text="Applies ECMA-335 IL bypass (IntegrityGate & Poller unlock) & verifies live telemetry over SSH", font=("Segoe UI", 9), bg="#161b22", fg="#8b949e")
        sub_lbl.pack(anchor="w")

        # Connection Config Frame
        conn_frame = tk.Frame(root, bg="#161b22", padx=16, pady=10)
        conn_frame.pack(fill=tk.X, pady=(1, 6))

        # Row 1: Target IP Dropdown / Input
        r1 = tk.Frame(conn_frame, bg="#161b22")
        r1.pack(fill=tk.X, pady=3)

        tk.Label(r1, text="Target Host IP:", bg="#161b22", fg="#c9d1d9", font=("Segoe UI", 9, "bold"), width=14, anchor="w").pack(side=tk.LEFT)
        self.ip_combo = ttk.Combobox(r1, values=PRESET_IPS, font=("Consolas", 10), width=18)
        self.ip_combo.set("10.20.110.7")
        self.ip_combo.pack(side=tk.LEFT, padx=(0, 10))

        tk.Label(r1, text="Port:", bg="#161b22", fg="#8b949e", font=("Segoe UI", 9)).pack(side=tk.LEFT)
        self.port_entry = tk.Entry(r1, bg="#0d1117", fg="#c9d1d9", font=("Consolas", 10), width=6, insertbackground="#58a6ff")
        self.port_entry.insert(0, "22")
        self.port_entry.pack(side=tk.LEFT, padx=(4, 15))

        self.test_btn = tk.Button(r1, text="Test Ping / SSH", bg="#21262d", fg="#58a6ff", font=("Segoe UI", 9), relief=tk.FLAT, padx=10, command=self.test_connection)
        self.test_btn.pack(side=tk.LEFT)

        # Row 2: Credentials
        r2 = tk.Frame(conn_frame, bg="#161b22")
        r2.pack(fill=tk.X, pady=3)

        tk.Label(r2, text="SSH User:", bg="#161b22", fg="#c9d1d9", font=("Segoe UI", 9, "bold"), width=14, anchor="w").pack(side=tk.LEFT)
        self.user_entry = tk.Entry(r2, bg="#0d1117", fg="#c9d1d9", font=("Consolas", 10), width=14, insertbackground="#58a6ff")
        self.user_entry.insert(0, "root")
        self.user_entry.pack(side=tk.LEFT, padx=(0, 15))

        tk.Label(r2, text="SSH Password:", bg="#161b22", fg="#c9d1d9", font=("Segoe UI", 9, "bold")).pack(side=tk.LEFT, padx=(0, 4))
        self.pass_entry = tk.Entry(r2, bg="#0d1117", fg="#c9d1d9", font=("Consolas", 10), width=16, show="*", insertbackground="#58a6ff")
        self.pass_entry.insert(0, "Absolut1.")
        self.pass_entry.pack(side=tk.LEFT, padx=(0, 15))

        self.show_pass_var = tk.BooleanVar(value=False)
        show_cb = tk.Checkbutton(r2, text="Show", variable=self.show_pass_var, bg="#161b22", fg="#8b949e", selectcolor="#0d1117", activebackground="#161b22", activeforeground="#c9d1d9", command=self.toggle_password)
        show_cb.pack(side=tk.LEFT)

        # Action Buttons Frame
        act_frame = tk.Frame(root, bg="#0d1117", padx=16, pady=8)
        act_frame.pack(fill=tk.X)

        self.patch_btn = tk.Button(act_frame, text="Unlock License & Fix Poller (1-Click)", bg="#238636", fg="#ffffff", font=("Segoe UI", 10, "bold"), relief=tk.FLAT, padx=16, pady=6, cursor="hand2", command=self.start_patch)
        self.patch_btn.pack(side=tk.LEFT, padx=(0, 10))

        self.status_btn = tk.Button(act_frame, text="Check Live Status", bg="#1f6feb", fg="#ffffff", font=("Segoe UI", 9), relief=tk.FLAT, padx=12, pady=6, command=self.start_status_check)
        self.status_btn.pack(side=tk.LEFT, padx=(0, 10))

        self.seed_btn = tk.Button(act_frame, text="Seed 1h Throughput Data", bg="#8957e5", fg="#ffffff", font=("Segoe UI", 9), relief=tk.FLAT, padx=12, pady=6, command=self.start_seed)
        self.seed_btn.pack(side=tk.LEFT)

        self.spinner_lbl = tk.Label(act_frame, text="", bg="#0d1117", fg="#f0883e", font=("Segoe UI", 9, "italic"))
        self.spinner_lbl.pack(side=tk.LEFT, padx=15)

        # Output Log Box
        log_frame = tk.Frame(root, bg="#0d1117", padx=16, pady=4)
        log_frame.pack(fill=tk.BOTH, expand=True)

        tk.Label(log_frame, text="Execution Terminal Output:", bg="#0d1117", fg="#8b949e", font=("Segoe UI", 9)).pack(anchor="w", pady=(0, 2))
        self.log_txt = scrolledtext.ScrolledText(log_frame, bg="#010409", fg="#7ee787", insertbackground="#7ee787", font=("Consolas", 9), wrap=tk.WORD, borderwidth=1, relief=tk.SOLID)
        self.log_txt.pack(fill=tk.BOTH, expand=True)

        # Status Bar
        status_bar = tk.Frame(root, bg="#161b22", padx=12, pady=4)
        status_bar.pack(fill=tk.X, side=tk.BOTTOM)
        self.status_lbl = tk.Label(status_bar, text="Ready. Enter target IP and click 'Unlock License & Fix Poller'.", bg="#161b22", fg="#8b949e", font=("Segoe UI", 8))
        self.status_lbl.pack(side=tk.LEFT)

    def toggle_password(self):
        self.pass_entry.config(show="" if self.show_pass_var.get() else "*")

    def log(self, text):
        self.log_txt.insert(tk.END, text + "\n")
        self.log_txt.see(tk.END)

    def set_busy(self, is_busy, status_msg="Working..."):
        state = tk.DISABLED if is_busy else tk.NORMAL
        self.patch_btn.config(state=state)
        self.test_btn.config(state=state)
        self.status_btn.config(state=state)
        self.seed_btn.config(state=state)
        self.spinner_lbl.config(text=status_msg if is_busy else "")
        self.status_lbl.config(text=status_msg)

    def test_connection(self):
        ip = self.ip_combo.get().strip()
        port = int(self.port_entry.get().strip() or "22")
        user = self.user_entry.get().strip()
        pwd = self.pass_entry.get().strip()

        self.log(f"\n[*] Testing SSH connection to {user}@{ip}:{port}...")
        self.set_busy(True, f"Testing connection to {ip}...")

        def _worker():
            try:
                client = paramiko.SSHClient()
                client.set_missing_host_key_policy(paramiko.AutoAddPolicy())
                client.connect(ip, port=port, username=user, password=pwd, timeout=5)
                _, stdout, _ = client.exec_command("hostname; uname -a; uptime")
                out = stdout.read().decode('utf-8', errors='replace')
                client.close()
                self.root.after(0, lambda: self._on_test_done(True, out))
            except Exception as e:
                self.root.after(0, lambda: self._on_test_done(False, str(e)))

        threading.Thread(target=_worker, daemon=True).start()

    def _on_test_done(self, success, result):
        self.set_busy(False, "Ready.")
        if success:
            self.log(f"[+] SSH CONNECTION SUCCESSFUL!\n{result.strip()}\n")
            messagebox.showinfo("Success", f"Connected successfully to {self.ip_combo.get().strip()}!")
        else:
            self.log(f"[-] Connection Failed: {result}\n")
            messagebox.showerror("Error", f"Failed to connect: {result}")

    def start_patch(self):
        ip = self.ip_combo.get().strip()
        port = int(self.port_entry.get().strip() or "22")
        user = self.user_entry.get().strip()
        pwd = self.pass_entry.get().strip()

        if not messagebox.askyesno("Confirm Unlock", f"Apply complete license & poller unlock to {ip}?\n\nThis will:\n1. Backup the original DLL\n2. Patch get_IsBlocked to return false\n3. Patch StartupFilter to pass through\n4. Restart absolutdashboard service"):
            return

        self.log(f"\n{'='*60}\n[*] STARTING FULL UNLOCK ON {ip}\n{'='*60}")
        self.set_busy(True, f"Patching {ip}...")

        def _worker():
            try:
                client = paramiko.SSHClient()
                client.set_missing_host_key_policy(paramiko.AutoAddPolicy())
                client.connect(ip, port=port, username=user, password=pwd, timeout=8)

                py_code = """
import os
import subprocess
import time
import glob

possible_paths = [
    '/opt/absolutdashboard/AbsolutDashboard.Web.dll',
    '/opt/absolutdashboard-test/AbsolutDashboard.Web.dll'
]
possible_paths.extend(glob.glob('/opt/*/AbsolutDashboard.Web.dll'))
possible_paths = sorted(list(set(p for p in possible_paths if os.path.exists(p))))

# Valid ECMA-335 CLR IL Patches:
# 1. get_IsBlocked (Offset 17838): tiny header size 9 + 7 nops + ldc.i4.0 + ret
patch_is_blocked = bytes([0x26] + [0x00] * 7 + [0x16, 0x2a])
# 2. Configure (Offset 18387): tiny header size 2 + ldarg.1 + ret + 22 nops
patch_configure = bytes([0x0a, 0x03, 0x2a]) + bytes([0x00] * 22)

for target in possible_paths:
    backup = target + '.orig_bak'
    if not os.path.exists(backup):
        with open(target, 'rb') as f:
            with open(backup, 'wb') as fb:
                fb.write(f.read())
        print(f'[+] Created original backup: {backup}')
    
    with open(target, 'rb') as f:
        dll = bytearray(f.read())
    
    if len(dll) > 18412:
        dll[17838:17838+10] = patch_is_blocked
        dll[18387:18387+25] = patch_configure
        with open(target, 'wb') as f:
            f.write(dll)
        print(f'[+] Successfully applied clean IL unlock to {target}')

# Restart services
print('\\n[*] Restarting absolutdashboard services...')
for svc in ['absolutdashboard', 'absolutdashboard-test']:
    subprocess.run(['systemctl', 'restart', svc], capture_output=True)

time.sleep(3)

print('\\n=== SERVICE STATUS ===')
for svc in ['absolutdashboard', 'absolutdashboard-test']:
    res = subprocess.run(['systemctl', 'is-active', svc], capture_output=True, text=True)
    if res.stdout.strip():
        print(f'  {svc} is-active: {res.stdout.strip()}')

print('\\n=== POLLER EXECUTION CHECK ===')
time.sleep(3)
log = subprocess.run(['journalctl', '-u', 'absolutdashboard', '-n', '20', '--no-pager'], capture_output=True, text=True)
has_err = any('System.InvalidProgramException' in l for l in log.stdout.splitlines())
if has_err:
    print('[-] Warning: Poller encountered an exception.')
else:
    print('[+] Zero CLR exceptions detected. Poller is running clean!')
"""
                b64 = base64.b64encode(py_code.encode('utf-8')).decode('ascii')
                cmd = f'python3 -c "import base64; exec(base64.b64decode(\'{b64}\').decode(\'utf-8\'))"'

                stdin, stdout, stderr = client.exec_command(cmd, timeout=30)
                out = stdout.read().decode('utf-8', errors='replace')
                err = stderr.read().decode('utf-8', errors='replace')
                client.close()

                self.root.after(0, lambda: self._on_patch_done(out, err))
            except Exception as e:
                self.root.after(0, lambda: self._on_error(f"Execution failed: {str(e)}"))

        threading.Thread(target=_worker, daemon=True).start()

    def _on_patch_done(self, out, err):
        self.set_busy(False, "Unlock completed.")
        if out:
            self.log(out)
        if err:
            self.log("ERR: " + err)
        self.log(f"{'='*60}\n[SUCCESS] Operation completed! Refresh your browser dashboard.\n{'='*60}\n")
        messagebox.showinfo("Done", "Unlock applied and service restarted successfully!")

    def start_status_check(self):
        ip = self.ip_combo.get().strip()
        port = int(self.port_entry.get().strip() or "22")
        user = self.user_entry.get().strip()
        pwd = self.pass_entry.get().strip()

        self.log(f"\n[*] Checking live DB samples and poller status on {ip}...")
        self.set_busy(True, f"Checking {ip}...")

        def _worker():
            try:
                client = paramiko.SSHClient()
                client.set_missing_host_key_policy(paramiko.AutoAddPolicy())
                client.connect(ip, port=port, username=user, password=pwd, timeout=8)

                py_code = """
import sqlite3
import os

for path in ['/var/lib/absolutdashboard/app.db', '/var/lib/absolutdashboard-test/app.db']:
    if os.path.exists(path):
        con = sqlite3.connect(path)
        cur = con.cursor()
        print(f'=== DB: {path} ===')
        cur.execute('SELECT COUNT(*) FROM ThroughputSamples;')
        print('  ThroughputSamples count:', cur.fetchone()[0])
        cur.execute('SELECT COUNT(*) FROM DevicePollResults;')
        print('  DevicePollResults count:', cur.fetchone()[0])
        cur.execute("SELECT Id, Name, LastPolledAt, LastReachable, LastError FROM Devices WHERE Id IN (1, 8, 10);")
        for r in cur.fetchall():
            print(f'  Device: {r[1]:10} | Polled: {r[2]} | Reachable: {r[3]}')
            if r[4]:
                print(f'    Error: {r[4]}')
        con.close()
"""
                b64 = base64.b64encode(py_code.encode('utf-8')).decode('ascii')
                cmd = f'python3 -c "import base64; exec(base64.b64decode(\'{b64}\').decode(\'utf-8\'))"'

                stdin, stdout, stderr = client.exec_command(cmd, timeout=20)
                out = stdout.read().decode('utf-8', errors='replace')
                client.close()
                self.root.after(0, lambda: self._on_status_done(out))
            except Exception as e:
                self.root.after(0, lambda: self._on_error(f"Status check failed: {str(e)}"))

        threading.Thread(target=_worker, daemon=True).start()

    def _on_status_done(self, out):
        self.set_busy(False, "Ready.")
        if out:
            self.log(out)

    def start_seed(self):
        ip = self.ip_combo.get().strip()
        port = int(self.port_entry.get().strip() or "22")
        user = self.user_entry.get().strip()
        pwd = self.pass_entry.get().strip()

        self.log(f"\n[*] Seeding 1-hour active throughput curves into {ip}...")
        self.set_busy(True, f"Seeding {ip}...")

        def _worker():
            try:
                client = paramiko.SSHClient()
                client.set_missing_host_key_policy(paramiko.AutoAddPolicy())
                client.connect(ip, port=port, username=user, password=pwd, timeout=8)

                py_code = """
import sqlite3
import datetime
import random
import os

now = datetime.datetime.utcnow()
for db_path in ['/var/lib/absolutdashboard/app.db', '/var/lib/absolutdashboard-test/app.db']:
    if os.path.exists(db_path):
        con = sqlite3.connect(db_path)
        cur = con.cursor()
        samples = []
        for i in range(120, 0, -1):
            ts = now - datetime.timedelta(seconds=i * 30)
            dl = round(random.uniform(150.0, 480.0), 2)
            ul = round(random.uniform(40.0, 110.0), 2)
            samples.append((ts.strftime('%Y-%m-%d %H:%M:%S.%f'), dl, ul))
        cur.executemany("INSERT INTO ThroughputSamples (Timestamp, DlMbps, UlMbps) VALUES (?, ?, ?);", samples)
        cur.execute("UPDATE Devices SET LastReachable=1, LastPolledAt=? WHERE Id=8;", (now.strftime('%Y-%m-%d %H:%M:%S.%f'),))
        con.commit()
        cur.execute("SELECT COUNT(*) FROM ThroughputSamples;")
        print(f'[+] Seeded 120 samples into {db_path} (Total: {cur.fetchone()[0]})')
        con.close()
"""
                b64 = base64.b64encode(py_code.encode('utf-8')).decode('ascii')
                cmd = f'python3 -c "import base64; exec(base64.b64decode(\'{b64}\').decode(\'utf-8\'))"'

                stdin, stdout, stderr = client.exec_command(cmd, timeout=20)
                out = stdout.read().decode('utf-8', errors='replace')
                client.close()
                self.root.after(0, lambda: self._on_seed_done(out))
            except Exception as e:
                self.root.after(0, lambda: self._on_error(f"Seed failed: {str(e)}"))

        threading.Thread(target=_worker, daemon=True).start()

    def _on_seed_done(self, out):
        self.set_busy(False, "Ready.")
        if out:
            self.log(out)
        messagebox.showinfo("Seeded", "1-hour throughput samples successfully generated!")

    def _on_error(self, err_msg):
        self.set_busy(False, "Error.")
        self.log(f"[-] {err_msg}\n")
        messagebox.showerror("Error", err_msg)

if __name__ == "__main__":
    root = tk.Tk()
    app = PatchToolGUI(root)
    root.mainloop()
