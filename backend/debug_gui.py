import tkinter as tk
from tkinter import ttk, scrolledtext
import threading
from ping_handler import PingHandler
from probe_handler import ProbeHandler
from port_scanner import PortScanner

class DebugGUI:
    def __init__(self, root, port=8080):
        self.root = root
        self.port = port
        self.root.title("Backend Diagnostics Console (cURL, Ports & Ping)")
        self.root.geometry("880x660")
        self.root.configure(bg="#0d1117")

        # Styling
        style = ttk.Style()
        style.theme_use('clam')
        style.configure(".", background="#0d1117", foreground="#c9d1d9")

        # Header Frame
        header = tk.Frame(root, bg="#161b22", padx=12, pady=10)
        header.pack(fill=tk.X)

        title_lbl = tk.Label(header, text="Network Diagnostics & HTTP Probe (cURL)", font=("Segoe UI", 12, "bold"), bg="#161b22", fg="#58a6ff")
        title_lbl.pack(anchor="w")

        info_lbl = tk.Label(header, text=f"WSGIServer on port {self.port} | Remote: 10.20.110.7 | Gateway: 192.168.1.1", font=("Segoe UI", 9), bg="#161b22", fg="#3fb950")
        info_lbl.pack(anchor="w")

        # Toolbar 1: Ping & Quick Port Scanner
        ping_bar = tk.Frame(root, bg="#0d1117", padx=12, pady=6)
        ping_bar.pack(fill=tk.X)

        tk.Label(ping_bar, text="Host IP:", bg="#0d1117", fg="#c9d1d9", font=("Consolas", 10)).pack(side=tk.LEFT)
        self.ip_entry = tk.Entry(ping_bar, bg="#161b22", fg="#58a6ff", insertbackground="#58a6ff", font=("Consolas", 10), width=16)
        self.ip_entry.insert(0, "10.20.110.7")
        self.ip_entry.pack(side=tk.LEFT, padx=6)

        self.ping_btn = tk.Button(ping_bar, text="⚡ Ping", bg="#238636", fg="#ffffff", font=("Segoe UI", 9, "bold"), activebackground="#2ea043", relief=tk.FLAT, padx=8, command=self.start_ping)
        self.ping_btn.pack(side=tk.LEFT, padx=4)

        self.port_btn = tk.Button(ping_bar, text="🔌 Scan Open Ports", bg="#d29922", fg="#ffffff", font=("Segoe UI", 9, "bold"), activebackground="#e3b341", relief=tk.FLAT, padx=8, command=self.start_port_scan)
        self.port_btn.pack(side=tk.LEFT, padx=4)

        # Right-side action buttons
        self.clear_btn = tk.Button(ping_bar, text="Clear Log", bg="#21262d", fg="#8b949e", font=("Segoe UI", 9), activebackground="#30363d", relief=tk.FLAT, padx=8, command=self.clear_log)
        self.clear_btn.pack(side=tk.RIGHT, padx=4)

        self.copy_btn = tk.Button(ping_bar, text="📋 Copy to Clipboard", bg="#1f6feb", fg="#ffffff", font=("Segoe UI", 9, "bold"), activebackground="#388bfd", relief=tk.FLAT, padx=8, command=self.copy_to_clipboard)
        self.copy_btn.pack(side=tk.RIGHT, padx=4)

        # Toolbar 2: cURL HTTP/HTTPS Probe
        curl_bar = tk.Frame(root, bg="#161b22", padx=12, pady=8)
        curl_bar.pack(fill=tk.X)

        tk.Label(curl_bar, text="Method:", bg="#161b22", fg="#8b949e", font=("Consolas", 9)).pack(side=tk.LEFT)
        self.method_var = tk.StringVar(value="GET")
        method_dropdown = ttk.Combobox(curl_bar, textvariable=self.method_var, values=["GET", "POST", "HEAD", "OPTIONS"], width=7, state="readonly")
        method_dropdown.pack(side=tk.LEFT, padx=6)

        tk.Label(curl_bar, text="Target URL:", bg="#161b22", fg="#c9d1d9", font=("Consolas", 10)).pack(side=tk.LEFT)
        self.url_entry = tk.Entry(curl_bar, bg="#0d1117", fg="#58a6ff", insertbackground="#58a6ff", font=("Consolas", 10), width=32)
        self.url_entry.insert(0, "https://10.20.110.7")
        self.url_entry.pack(side=tk.LEFT, padx=6, fill=tk.X, expand=True)

        self.curl_btn = tk.Button(curl_bar, text="🌐 Send cURL", bg="#1f6feb", fg="#ffffff", font=("Segoe UI", 9, "bold"), activebackground="#388bfd", relief=tk.FLAT, padx=10, command=self.start_curl)
        self.curl_btn.pack(side=tk.LEFT, padx=4)

        self.scan_btn = tk.Button(curl_bar, text="🔍 Scan Paths", bg="#8957e5", fg="#ffffff", font=("Segoe UI", 9), activebackground="#a371f7", relief=tk.FLAT, padx=8, command=self.start_scan)
        self.scan_btn.pack(side=tk.LEFT, padx=4)

        # Terminal Display Area
        term_frame = tk.Frame(root, bg="#0d1117", padx=12, pady=6)
        term_frame.pack(fill=tk.BOTH, expand=True)

        self.text_area = scrolledtext.ScrolledText(term_frame, bg="#05080e", fg="#c9d1d9", insertbackground="#3fb950", font=("Consolas", 9), relief=tk.SOLID, borderwidth=1)
        self.text_area.pack(fill=tk.BOTH, expand=True)

        self.log("[*] Debug Controller loaded.")
        self.log("[*] Tip: Ping succeeded on 10.20.110.7 but port 80 refused HTTP. Use 'Scan Open Ports' or try 'https://10.20.110.7'.\n")

    def log(self, message):
        self.text_area.insert(tk.END, message + "\n")
        self.text_area.see(tk.END)

    def clear_log(self):
        self.text_area.delete('1.0', tk.END)

    def copy_to_clipboard(self):
        text = self.text_area.get('1.0', tk.END).strip()
        self.root.clipboard_clear()
        self.root.clipboard_append(text)
        
        # Brief visual feedback on button
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
        self.port_btn.config(state=tk.NORMAL, text="🔌 Scan Open Ports")
        if open_ports:
            self.log(f"[+] Open ports found on {target}:")
            for p in open_ports:
                self.log(f"    • Port {p['port']}/TCP ({p['service']}) is OPEN")
        else:
            self.log(f"[-] No standard TCP ports found open on {target}.")
        self.log("")

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
        self.scan_btn.config(state=tk.NORMAL, text="🔍 Scan Paths")
        self.log(f"[+] Scan completed for {url}. Found {len(items)} responding endpoints:")
        if not items:
            self.log("    (No responsive standard endpoints found)")
        for item in items:
            self.log(f"    • {item['status_code']} {item['reason']} -> {item['target']} ({item['content_type']})")
        self.log("")
