import requests
import datetime
import socket

class MonitorHandler:
    DEFAULT_DEVICES = [
        {"id": "srf-main", "name": "Main Server (Ubuntu 26.04)", "ip": "10.20.110.7", "url": "http://10.20.110.7"},
        {"id": "client-gw", "name": "Workstation Gateway", "ip": "10.20.110.22", "url": "http://10.20.110.22"},
        {"id": "mgmt-console", "name": "Management Portal", "ip": "10.20.110.1", "url": "http://10.20.110.1"},
        {"id": "dns-primary", "name": "DNS / Gateway Service", "ip": "1.1.1.1", "url": "https://1.1.1.1"},
        {"id": "local-api", "name": "Local API Service", "ip": "127.0.0.1", "url": "http://127.0.0.1:8080/api/status"}
    ]

    MOBA_BANNER_TEMPLATE = """ ┌──────────────────────────────────────────────────────────────────────┐
    │                 • MobaXterm Personal Edition v26.4 •                 │
    │               (SSH client, X server and network tools)               │
    │                                                                      │
    │ ⮞ SSH session to root@{ip}                                    │
    │   • Direct SSH      :  ✓                                             │
    │   • SSH compression :  ✗                                             │
    │   • SSH-browser     :  ✓                                             │
    │   • X11-forwarding  :  ✓  (remote display is forwarded through SSH)  │
    │                                                                      │
    │ ⮞ For more info, ctrl+click on help or visit our website.            │
    └──────────────────────────────────────────────────────────────────────┘

Welcome to Ubuntu 26.04 LTS (GNU/Linux 7.0.0-34-generic x86_64)

 * Documentation:  https://docs.ubuntu.com
 * Management:     https://landscape.canonical.com
 * Support:        https://ubuntu.com/pro

 System information as of {system_time}

  System load:  0.18               Processes:              144
  Usage of /:   44.3% of 23.45GB   Users logged in:        0
  Memory usage: 3%                 IPv4 address for ens18: {ip}
  Swap usage:   0%

 * Canonical Workshop gives developers fast, composable, reproducible, and
   secure developer environments that are perfect for agentic workflows.

   https://ubuntu.com/workshop

Expanded Security Maintenance for Applications is not enabled.

79 updates can be applied immediately.
11 of these updates are standard security updates.
To see these additional updates run: apt list --upgradable

Enable ESM Apps to receive additional future security updates.
See https://ubuntu.com/esm or run: sudo pro status


*** System restart required ***
Last login: Tue Oct 20 17:09:41 2026 from 10.20.110.22
root@absolutsrf:~#"""

    def get_banner(self, target_ip="10.20.110.7"):
        formatted_time = datetime.datetime.now().strftime("%a %b %d %I:%M:%S %p %Z %Y")
        return self.MOBA_BANNER_TEMPLATE.format(ip=target_ip, system_time=formatted_time)

    def check_http_status(self, url, timeout=1.5):
        try:
            resp = requests.get(url, timeout=timeout, allow_redirects=True)
            return {
                "statusCode": resp.status_code,
                "statusText": f"{resp.status_code} {resp.reason}",
                "isOnline": True,
                "latencyMs": int(resp.elapsed.total_seconds() * 1000)
            }
        except requests.exceptions.Timeout:
            return {
                "statusCode": 408,
                "statusText": "Request Timeout",
                "isOnline": False,
                "latencyMs": None
            }
        except requests.exceptions.ConnectionError:
            return {
                "statusCode": 503,
                "statusText": "Connection Refused / Unreachable",
                "isOnline": False,
                "latencyMs": None
            }
        except Exception as e:
            return {
                "statusCode": 500,
                "statusText": str(e),
                "isOnline": False,
                "latencyMs": None
            }

    def inspect_devices(self, custom_devices=None):
        devices = custom_devices if custom_devices is not None else self.DEFAULT_DEVICES
        results = []
        for dev in devices:
            status = self.check_http_status(dev.get("url", ""))
            results.append({
                "id": dev.get("id"),
                "name": dev.get("name"),
                "ip": dev.get("ip"),
                "url": dev.get("url"),
                "statusCode": status["statusCode"],
                "statusText": status["statusText"],
                "isOnline": status["isOnline"],
                "latencyMs": status["latencyMs"],
                "lastChecked": datetime.datetime.utcnow().isoformat() + "Z"
            })
        return results
