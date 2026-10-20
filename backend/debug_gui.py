import tkinter as tk
from tkinter import ttk, scrolledtext
import threading
from ping_handler import PingHandler

class DebugGUI:
    def __init__(self, root, port=8080):
        self.root = root
        self.port = port
        self.root.title("Backend Debug Console - patrikeig")
        self.root.geometry("640x520")
        self.root.configure(bg="#0d1117")

        # Styling
        style = ttk.Style()
        style.theme_use('clam')
        style.configure(".", background="#0d1117", foreground="#c9d1d9")

        # Header Info
        header_frame = tk.Frame(root, bg="#161b22", padx=12, pady=10)
        header_frame.pack(fill=tk.X)

        title_lbl = tk.Label(header_frame, text="WSGI Server & Diagnostics Control", font=("Segoe UI", 12, "bold"), bg="#161b22", fg="#58a6ff")
        title_lbl.pack(anchor="w")

        status_lbl = tk.Label(header_frame, text=f"Status: ONLINE on port {self.port} | Remote Target: 10.20.110.7", font=("Segoe UI", 9), bg="#161b22", fg="#3fb950")
        status_lbl.pack(anchor="w")

        # Control Toolbar
        tool_frame = tk.Frame(root, bg="#0d1117", padx=12, pady=10)
        tool_frame.pack(fill=tk.X)

        tk.Label(tool_frame, text="Target IP:", bg="#0d1117", fg="#c9d1d9", font=("Consolas", 10)).pack(side=tk.LEFT)
        self.ip_entry = tk.Entry(tool_frame, bg="#161b22", fg="#58a6ff", insertbackground="#58a6ff", font=("Consolas", 10), width=16)
        self.ip_entry.insert(0, "10.20.110.7")
        self.ip_entry.pack(side=tk.LEFT, padx=6)

        self.ping_btn = tk.Button(tool_frame, text="⚡ Ping Host", bg="#238636", fg="#ffffff", font=("Segoe UI", 9, "bold"), activebackground="#2ea043", relief=tk.FLAT, padx=10, command=self.start_ping)
        self.ping_btn.pack(side=tk.LEFT, padx=4)

        self.clear_btn = tk.Button(tool_frame, text="Clear Log", bg="#21262d", fg="#8b949e", font=("Segoe UI", 9), activebackground="#30363d", relief=tk.FLAT, padx=8, command=self.clear_log)
        self.clear_btn.pack(side=tk.LEFT, padx=4)

        # Scrolled Terminal Output
        out_frame = tk.Frame(root, bg="#0d1117", padx=12, pady=6)
        out_frame.pack(fill=tk.BOTH, expand=True)

        self.text_area = scrolledtext.ScrolledText(out_frame, bg="#05080e", fg="#c9d1d9", insertbackground="#3fb950", font=("Consolas", 9), relief=tk.SOLID, borderwidth=1)
        self.text_area.pack(fill=tk.BOTH, expand=True)

        self.log(f"[*] WSGI Server initialized on port {self.port}.")
        self.log("[*] Debug Tkinter Controller ready.")
        self.log("[*] Ready to test ping probe against 10.20.110.7...\n")

    def log(self, message):
        self.text_area.insert(tk.END, message + "\n")
        self.text_area.see(tk.END)

    def clear_log(self):
        self.text_area.delete('1.0', tk.END)

    def start_ping(self):
        target = self.ip_entry.get().strip() or "10.20.110.7"
        self.ping_btn.config(state=tk.DISABLED, text="Pinging...")
        self.log(f"[*] Executing ping against {target}...")

        def _worker():
            res = PingHandler.ping_host(target)
            self.root.after(0, lambda: self._on_ping_finished(res))

        threading.Thread(target=_worker, daemon=True).start()

    def _on_ping_finished(self, res):
        self.ping_btn.config(state=tk.NORMAL, text="⚡ Ping Host")
        if res.get("success"):
            self.log(f"[+] Ping SUCCESS for {res.get('host')}:")
        else:
            self.log(f"[-] Ping FAILED for {res.get('host')}:")
        self.log(res.get("output", "") + "\n")
