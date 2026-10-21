import tkinter as tk
from tkinter import ttk, scrolledtext
import threading
from ping_handler import PingHandler
from probe_handler import ProbeHandler
from port_scanner import PortScanner
from ssh_handler import SSHHandler

class DebugGUI:
    def __init__(self, root, port=8080):
        self.root = root
        self.port = port
        self.root.title("Multi-System Explorer (SSH, cURL, PortScanner, Ping)")
        self.root.geometry("980x740")
        self.root.configure(bg="#0d1117")

        style = ttk.Style()
        style.theme_use('clam')
        style.configure(".", background="#0d1117", foreground="#c9d1d9")

        # Header Frame
        header = tk.Frame(root, bg="#161b22", padx=12, pady=8)
        header.pack(fill=tk.X)

        title_lbl = tk.Label(header, text="Network & Device Deep Explorer", font=("Segoe UI", 12, "bold"), bg="#161b22", fg="#58a6ff")
        title_lbl.pack(anchor="w")

        info_lbl = tk.Label(header, text=f"WSGIServer Port: {self.port} | Remote Linux: 10.20.110.7 | Gateway: 192.168.1.1", font=("Segoe UI", 9), bg="#161b22", fg="#3fb950")
        info_lbl.pack(anchor="w")

        # Toolbar 1: IP Target, Ping & Port Scan
        bar1 = tk.Frame(root, bg="#0d1117", padx=12, pady=5)
        bar1.pack(fill=tk.X)

        tk.Label(bar1, text="Host IP:", bg="#0d1117", fg="#c9d1d9", font=("Consolas", 10)).pack(side=tk.LEFT)
        self.ip_entry = tk.Entry(bar1, bg="#161b22", fg="#58a6ff", insertbackground="#58a6ff", font=("Consolas", 10), width=15)
        self.ip_entry.insert(0, "10.20.110.7")
        self.ip_entry.pack(side=tk.LEFT, padx=5)

        self.ping_btn = tk.Button(bar1, text="⚡ Ping", bg="#238636", fg="#ffffff", font=("Segoe UI", 9, "bold"), relief=tk.FLAT, padx=8, command=self.start_ping)
        self.ping_btn.pack(side=tk.LEFT, padx=3)

        self.port_btn = tk.Button(bar1, text="🔌 Scan Ports", bg="#d29922", fg="#ffffff", font=("Segoe UI", 9, "bold"), relief=tk.FLAT, padx=8, command=self.start_port_scan)
        self.port_btn.pack(side=tk.LEFT, padx=3)

        # Quick preset buttons
        self.pre1_btn = tk.Button(bar1, text="Set 10.20.110.7", bg="#21262d", fg="#8b949e", font=("Segoe UI", 8), relief=tk.FLAT, padx=5, command=lambda: self.set_target("10.20.110.7"))
        self.pre1_btn.pack(side=tk.LEFT, padx=2)

        self.pre2_btn = tk.Button(bar1, text="Set 192.168.1.1", bg="#21262d", fg="#8b949e", font=("Segoe UI", 8), relief=tk.FLAT, padx=5, command=lambda: self.set_target("192.168.1.1"))
        self.pre2_btn.pack(side=tk.LEFT, padx=2)

        self.copy_btn = tk.Button(bar1, text="📋 Copy Log", bg="#1f6feb", fg="#ffffff", font=("Segoe UI", 9, "bold"), relief=tk.FLAT, padx=8, command=self.copy_to_clipboard)
        self.copy_btn.pack(side=tk.RIGHT, padx=4)

        self.clear_btn = tk.Button(bar1, text="Clear", bg="#21262d", fg="#8b949e", font=("Segoe UI", 9), relief=tk.FLAT, padx=8, command=self.clear_log)
        self.clear_btn.pack(side=tk.RIGHT, padx=2)

        # Toolbar 2: SSH Remote Command Console
        ssh_bar = tk.Frame(root, bg="#161b22", padx=12, pady=6)
        ssh_bar.pack(fill=tk.X)

        tk.Label(ssh_bar, text="SSH User:", bg="#161b22", fg="#8b949e", font=("Consolas", 9)).pack(side=tk.LEFT)
        self.user_entry = tk.Entry(ssh_bar, bg="#0d1117", fg="#c9d1d9", font=("Consolas", 9), width=8)
        self.user_entry.insert(0, "root")
        self.user_entry.pack(side=tk.LEFT, padx=3)

        tk.Label(ssh_bar, text="Pass:", bg="#161b22", fg="#8b949e", font=("Consolas", 9)).pack(side=tk.LEFT)
        self.pass_entry = tk.Entry(ssh_bar, bg="#0d1117", fg="#c9d1d9", show="*", font=("Consolas", 9), width=10)
        self.pass_entry.pack(side=tk.LEFT, padx=3)

        tk.Label(ssh_bar, text="Cmd:", bg="#161b22", fg="#58a6ff", font=("Consolas", 9, "bold")).pack(side=tk.LEFT, padx=(6, 2))
        self.cmd_var = tk.StringVar(value="ss -tulpn")
        cmd_dropdown = ttk.Combobox(ssh_bar, textvariable=self.cmd_var, values=[
            "ss -tulpn",
            "ip a",
            "curl -I http://127.0.0.1",
            "docker ps",
            "ps aux | grep -E 'python|node|nginx|apache|http'",
            "cat /etc/os-release",
            "uptime"
        ], width=28)
        cmd_dropdown.pack(side=tk.LEFT, padx=3)

        self.ssh_btn = tk.Button(ssh_bar, text="💻 Exec SSH", bg="#238636", fg="#ffffff", font=("Segoe UI", 9, "bold"), relief=tk.FLAT, padx=8, command=self.start_ssh)
        self.ssh_btn.pack(side=tk.LEFT, padx=4)

        # Toolbar 3: cURL HTTP/HTTPS Probe
        curl_bar = tk.Frame(root, bg="#0d1117", padx=12, pady=6)
        curl_bar.pack(fill=tk.X)

        self.method_var = tk.StringVar(value="GET")
        method_dropdown = ttk.Combobox(curl_bar, textvariable=self.method_var, values=["GET", "POST", "HEAD", "OPTIONS"], width=7, state="readonly")
        method_dropdown.pack(side=tk.LEFT, padx=2)

        self.url_entry = tk.Entry(curl_bar, bg="#161b22", fg="#58a6ff", insertbackground="#58a6ff", font=("Consolas", 9), width=32)
        self.url_entry.insert(0, "http://192.168.1.1")
        self.url_entry.pack(side=tk.LEFT, padx=5, fill=tk.X, expand=True)

        self.curl_btn = tk.Button(curl_bar, text="🌐 Send cURL", bg="#1f6feb", fg="#ffffff", font=("Segoe UI", 9, "bold"), relief=tk.FLAT, padx=8, command=self.start_curl)
        self.curl_btn.pack(side=tk.LEFT, padx=3)

        self.scan_btn = tk.Button(curl_bar, text="🔍 Scan HTTP Paths", bg="#8957e5", fg="#ffffff", font=("Segoe UI", 9), relief=tk.FLAT, padx=8, command=self.start_scan)
        self.scan_btn.pack(side=tk.LEFT, padx=3)

        # Terminal Log Area
        term_frame = tk.Frame(root, bg="#0d1117", padx=12, pady=6)
        term_frame.pack(fill=tk.BOTH, expand=True)

        self.text_area = scrolledtext.ScrolledText(term_frame, bg="#05080e", fg="#c9d1d9", insertbackground="#3fb950", font=("Consolas", 9), relief=tk.SOLID, borderwidth=1)
        self.text_area.pack(fill=tk.BOTH, expand=True)

        self.log("[*] System Explorer Ready.")
        self.log("[*] Suggested tests:")
        self.log("    1. Enter root password -> Click 'Exec SSH' with 'ss -tulpn' to see all internal listening ports on 10.20.110.7.")
        self.log("    2. Click 'Set 192.168.1.1' -> Click 'Scan Ports' or 'Send cURL' to test the gateway.\n")

    def log(self, message):
        self.text_area.insert(tk.END, message + "\n")
        self.text_area.see(tk.END)

    def clear_log(self):
        self.text_area.delete('1.0', tk.END)

    def set_target(self, ip):
        self.ip_entry.delete(0, tk.END)
        self.ip_entry.insert(0, ip)
        self.url_entry.delete(0, tk.END)
        self.url_entry.insert(0, f"http://{ip}")
        self.log(f"[*] Target switched to {ip}")

    def copy_to_clipboard(self):
        text = self.text_area.get('1.0', tk.END).strip()
        self.root.clipboard_clear()
        self.root.clipboard_append(text)
        prev_text = self.copy_btn.cget("text")
        self.copy_btn.config(text="✓ Copied!", bg="#238636")
        self.root.after(1200, lambda: self.copy_btn.config(text=prev_text, bg="#1f6feb"))

    def start_ping(self):
        target = self.ip_entry.get().strip() or "10.20.110.7"
        self.ping_btn.config(state=tk.DISABLED, text="Pinging...")
        self.log(f"[*] Executing ping against {target}...")

        def _worker():
            res = PingHandler.ping_host(target)
            self.root.after(0, lambda: self._on_ping_finished(res))

        threading.Thread(target=_worker, daemon=True).start()

    def _on_ping_finished(self, res):
        self.ping_btn.config(state=tk.NORMAL, text="⚡ Ping")
        if res.get("success"):
            self.log(f"[+] Ping SUCCESS for {res.get('host')}:")
        else:
            self.log(f"[-] Ping FAILED for {res.get('host')}:")
        self.log(res.get("output", "") + "\n")

    def start_port_scan(self):
        target = self.ip_entry.get().strip() or "10.20.110.7"
        self.port_btn.config(state=tk.DISABLED, text="Scanning...")
        self.log(f"[*] Scanning common ports on {target}...")

        def _worker():
            open_ports = PortScanner.scan_ports(target)
            self.root.after(0, lambda: self._on_port_scan_finished(target, open_ports))

        threading.Thread(target=_worker, daemon=True).start()

    def _on_port_scan_finished(self, target, open_ports):
        self.port_btn.config(state=tk.NORMAL, text="🔌 Scan Ports")
        if open_ports:
            self.log(f"[+] Open ports found on {target}:")
            for p in open_ports:
                self.log(f"    • Port {p['port']}/TCP ({p['service']}) is OPEN")
        else:
            self.log(f"[-] No standard TCP ports open on {target}.")
        self.log("")

    def start_ssh(self):
        host = self.ip_entry.get().strip() or "10.20.110.7"
        user = self.user_entry.get().strip() or "root"
        password = self.pass_entry.get()
        command = self.cmd_var.get().strip() or "ss -tulpn"

        self.ssh_btn.config(state=tk.DISABLED, text="Running...")
        self.log(f"[*] SSH -> {user}@{host}: $ {command}")

        def _worker():
            res = SSHHandler.execute_command(host=host, user=user, password=password, command=command)
            self.root.after(0, lambda: self._on_ssh_finished(res))

        threading.Thread(target=_worker, daemon=True).start()

    def _on_ssh_finished(self, res):
        self.ssh_btn.config(state=tk.NORMAL, text="💻 Exec SSH")
        if res.get("success"):
            self.log("[+] Command Output:")
            self.log(res.get("stdout", "(empty output)") + "\n")
        else:
            self.log(f"[-] Command Error (Exit Code {res.get('exit_status')}):")
            self.log(res.get("stderr", "Unknown error") + "\n")

    def start_curl(self):
        url = self.url_entry.get().strip()
        method = self.method_var.get()
        if not url:
            self.log("[!] Please enter a URL.")
            return

        self.curl_btn.config(state=tk.DISABLED, text="Sending...")
        self.log(f"[*] > {method} {url} ...")

        def _worker():
            res = ProbeHandler.execute_request(url, method=method)
            self.root.after(0, lambda: self._on_curl_finished(res))

        threading.Thread(target=_worker, daemon=True).start()

    def _on_curl_finished(self, res):
        self.curl_btn.config(state=tk.NORMAL, text="🌐 Send cURL")
        self.log("------------------------------------------------------------")
        self.log(f"< HTTP Status: {res.get('status_code')} {res.get('reason')}")
        if res.get('elapsed_ms') is not None:
            self.log(f"< Response Time: {res.get('elapsed_ms')} ms")
        if res.get('content_type'):
            self.log(f"< Content-Type: {res.get('content_type')}")
        
        self.log("\n--- Headers ---")
        for k, v in res.get("headers", {}).items():
            self.log(f"{k}: {v}")

        self.log("\n--- Body ---")
        self.log(res.get("body", "") + "\n")
        self.log("------------------------------------------------------------\n")

    def start_scan(self):
        url = self.url_entry.get().strip()
        if not url:
            self.log("[!] Please enter a URL.")
            return

        self.scan_btn.config(state=tk.DISABLED, text="Scanning...")
        self.log(f"[*] Scanning standard endpoints on {url}...")

        def _worker():
            res = ProbeHandler.scan_common_paths(url)
            self.root.after(0, lambda: self._on_scan_finished(url, res))

        threading.Thread(target=_worker, daemon=True).start()

    def _on_scan_finished(self, url, items):
        self.scan_btn.config(state=tk.NORMAL, text="🔍 Scan HTTP Paths")
        self.log(f"[+] Scan completed for {url}. Found {len(items)} responding endpoints:")
        if not items:
            self.log("    (No responsive standard endpoints found)")
        for item in items:
            self.log(f"    • {item['status_code']} {item['reason']} -> {item['target']} ({item['content_type']})")
        self.log("")
