import os
import sys
import paramiko

if sys.platform == "win32":
    sys.stdout.reconfigure(encoding="utf-8", errors="replace")
    sys.stderr.reconfigure(encoding="utf-8", errors="replace")

def run():
    c7 = paramiko.SSHClient()
    c7.set_missing_host_key_policy(paramiko.AutoAddPolicy())
    c7.connect("10.20.110.7", port=22, username="root", password="Absolut1.", timeout=5)

    py_check_core_vm = """
import subprocess

# Test whether 10.20.110.5 responds on port 22 with user admin or h3c
print("=== CHECK 10.20.110.5 vs 10.20.111.30 ===")
res = subprocess.run(['nc', '-zv', '-w', '2', '10.20.110.5', '22'], capture_output=True, text=True)
print("10.20.110.5:22 ->", res.stderr.strip() or res.stdout.strip())

res30 = subprocess.run(['nc', '-zv', '-w', '2', '10.20.111.30', '22'], capture_output=True, text=True)
print("10.20.111.30:22 ->", res30.stderr.strip() or res30.stdout.strip())
"""
    import base64
    b64 = base64.b64encode(py_check_core_vm.encode()).decode()
    _, stdout, stderr = c7.exec_command(f'python3 -c "import base64; exec(base64.b64decode(\'{b64}\').decode())"')
    print(stdout.read().decode('utf-8', errors='replace'))
    c7.close()

if __name__ == "__main__":
    run()
