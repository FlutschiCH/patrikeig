import sys
sys.path.append('backend')
from ssh_handler import SSHHandler
from probe_handler import ProbeHandler

print("=== 1. Testing SSH Auth with Absolut1. ===")
res_os = SSHHandler.execute_command(host='10.20.110.7', user='root', password='Absolut1.', command='cat /etc/os-release | grep PRETTY_NAME')
print("OS:", res_os.get('stdout') or res_os.get('stderr'))

print("\n=== 2. Discovering AbsolutDashboard process ===")
res_proc = SSHHandler.execute_command(host='10.20.110.7', user='root', password='Absolut1.', command='ps aux | grep -i AbsolutDashboar | grep -v grep')
print("PROC:", res_proc.get('stdout'))

res_cwd = SSHHandler.execute_command(host='10.20.110.7', user='root', password='Absolut1.', command='ls -la /proc/$(pgrep -o AbsolutDashboar)/cwd')
print("CWD:", res_cwd.get('stdout'))

print("\n=== 3. Testing Local HTTP on Port 5001 from 10.20.110.7 ===")
res_curl = SSHHandler.execute_command(host='10.20.110.7', user='root', password='Absolut1.', command='curl -I http://127.0.0.1:5001/ ; curl -s http://127.0.0.1:5001/ | head -n 20')
print("HTTP 5001 (Local):", res_curl.get('stdout'))

print("\n=== 4. Testing Remote HTTP to 10.20.110.7:5001 ===")
res_remote = ProbeHandler.execute_request('http://10.20.110.7:5001/')
print("HTTP 5001 (Remote from Windows):", res_remote.get('status_code'), res_remote.get('reason'), res_remote.get('body')[:200] if res_remote.get('body') else '')
