import subprocess
import platform
import threading

class PingHandler:
    @staticmethod
    def ping_host(host="10.20.110.7", count=4, timeout_sec=2):
        """Pings a target host and returns status, latency, and raw output."""
        is_windows = platform.system().lower() == "windows"
        cmd = ["ping", "-n" if is_windows else "-c", str(count), "-w" if is_windows else "-W", str(timeout_sec * 1000 if is_windows else timeout_sec), host]
        
        try:
            res = subprocess.run(cmd, stdout=subprocess.PIPE, stderr=subprocess.PIPE, text=True, timeout=timeout_sec * count + 5)
            output = res.stdout + ("\n" + res.stderr if res.stderr else "")
            success = res.returncode == 0 and ("TTL=" in output or "ttl=" in output or "bytes from" in output)
            return {
                "host": host,
                "success": success,
                "output": output.strip()
            }
        except subprocess.TimeoutExpired:
            return {
                "host": host,
                "success": False,
                "output": f"Ping timeout exceeded ({timeout_sec}s per probe)"
            }
        except Exception as e:
            return {
                "host": host,
                "success": False,
                "output": f"Error running ping: {str(e)}"
            }
