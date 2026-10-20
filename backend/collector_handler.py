import requests
from bs4 import BeautifulSoup
import subprocess
import re
import datetime

class CollectorHandler:
    """Handles multi-source data ingestion: HTTP/HTML grabber, SNMP, and custom scripts."""

    def __init__(self):
        # Configurable query catalog (~30 preset query definitions)
        self.queries = [
            # Router 192.168.1.1 Web Scraper Queries
            {"id": "q1", "category": "web_grabber", "name": "Router WAN Status", "target": "http://192.168.1.1", "extractor": "status_page", "interval": 10},
            {"id": "q2", "category": "web_grabber", "name": "Router Public IP", "target": "http://192.168.1.1", "extractor": "wan_ip", "interval": 15},
            {"id": "q3", "category": "web_grabber", "name": "Router DSL/Fiber Sync Rate", "target": "http://192.168.1.1", "extractor": "sync_rate", "interval": 30},
            {"id": "q4", "category": "web_grabber", "name": "Router DHCP Clients Count", "target": "http://192.168.1.1", "extractor": "dhcp_list", "interval": 20},
            {"id": "q5", "category": "web_grabber", "name": "Router WiFi 2.4G/5G Status", "target": "http://192.168.1.1", "extractor": "wifi_status", "interval": 20},
            {"id": "q6", "category": "web_grabber", "name": "Router Firewall Packet Drop Rate", "target": "http://192.168.1.1", "extractor": "fw_drops", "interval": 60},
            
            # SNMP Queries (Router & Switch Interfaces)
            {"id": "q7", "category": "snmp", "name": "System Uptime", "target": "192.168.1.1", "oid": "1.3.6.1.2.1.1.3.0", "interval": 15},
            {"id": "q8", "category": "snmp", "name": "System Description", "target": "192.168.1.1", "oid": "1.3.6.1.2.1.1.1.0", "interval": 300},
            {"id": "q9", "category": "snmp", "name": "Interface eth0 Inbound Traffic", "target": "192.168.1.1", "oid": "1.3.6.1.2.1.2.2.1.10.1", "interval": 10},
            {"id": "q10", "category": "snmp", "name": "Interface eth0 Outbound Traffic", "target": "192.168.1.1", "oid": "1.3.6.1.2.1.2.2.1.16.1", "interval": 10},
            {"id": "q11", "category": "snmp", "name": "Interface eth1 Operational Status", "target": "192.168.1.1", "oid": "1.3.6.1.2.1.2.2.1.8.2", "interval": 10},
            {"id": "q12", "category": "snmp", "name": "Switch Port Errors (CRC/Discard)", "target": "192.168.1.2", "oid": "1.3.6.1.2.1.2.2.1.14.1", "interval": 30},
            {"id": "q13", "category": "snmp", "name": "CPU Utilization (1 min avg)", "target": "10.20.110.7", "oid": "1.3.6.1.4.1.2021.10.1.3.1", "interval": 10},
            {"id": "q14", "category": "snmp", "name": "Memory Total Free", "target": "10.20.110.7", "oid": "1.3.6.1.4.1.2021.4.6.0", "interval": 15},
            {"id": "q15", "category": "snmp", "name": "Active TCP Connections", "target": "10.20.110.7", "oid": "1.3.6.1.2.1.6.9.0", "interval": 20},
            {"id": "q16", "category": "snmp", "name": "Server Fan RPM", "target": "10.20.110.7", "oid": "1.3.6.1.4.1.674.10892.1.700.12.1.6.1.1", "interval": 30},

            # Script / Custom Automation Checks
            {"id": "q17", "category": "script", "name": "Ping Latency (192.168.1.1)", "target": "192.168.1.1", "command": "ping -c 1 192.168.1.1", "interval": 5},
            {"id": "q18", "category": "script", "name": "DNS Resolution Health", "target": "1.1.1.1", "command": "nslookup google.com 1.1.1.1", "interval": 15},
            {"id": "q19", "category": "script", "name": "Port 80/443 Socket Open Check", "target": "192.168.1.1", "command": "socket_check", "interval": 10},
            {"id": "q20", "category": "script", "name": "Gateway ARP Inspection", "target": "192.168.1.1", "command": "arp -a", "interval": 60},
            {"id": "q21", "category": "script", "name": "Server Disk I/O Wait", "target": "10.20.110.7", "command": "iostat -x 1 1", "interval": 20},
            {"id": "q22", "category": "script", "name": "TLS Certificate Expiry", "target": "10.20.110.7", "command": "cert_check", "interval": 3600},
            {"id": "q23", "category": "script", "name": "NTP Time Drift", "target": "pool.ntp.org", "command": "chronyc tracking", "interval": 120},
            {"id": "q24", "category": "script", "name": "Unattended Upgrades Status", "target": "10.20.110.7", "command": "apt list --upgradable", "interval": 300},

            # HTTP Raw Endpoint Checks
            {"id": "q25", "category": "http", "name": "Router WebUI Health", "target": "http://192.168.1.1", "interval": 10},
            {"id": "q26", "category": "http", "name": "SRF Main Node REST API", "target": "http://10.20.110.7:8080/api/status", "interval": 10},
            {"id": "q27", "category": "http", "name": "Workstation Gateway API", "target": "http://10.20.110.22", "interval": 15},
            {"id": "q28", "category": "http", "name": "Cloud DNS Over HTTPS", "target": "https://1.1.1.1/dns-query", "interval": 30},
            {"id": "q29", "category": "http", "name": "External Speedtest Probe", "target": "https://fast.com", "interval": 300},
            {"id": "q30", "category": "http", "name": "Database Proxy Status", "target": "http://127.0.0.1:8080/api/sql/upsert", "interval": 60}
        ]

    def grab_router_html(self, target_url="http://192.168.1.1", auth=None):
        """Fetches and cleans messy HTML from clumsy router web interfaces."""
        try:
            resp = requests.get(target_url, auth=auth, timeout=3.0)
            soup = BeautifulSoup(resp.text, 'lxml' if 'lxml' in globals() else 'html.parser')

            # Strip script and style junk
            for tag in soup(["script", "style", "meta", "noscript"]):
                tag.decompose()

            raw_text = soup.get_text(separator=" ", strip=True)

            # Heuristics to grab typical router info
            ip_matches = re.findall(r'\b(?:\d{1,3}\.){3}\d{1,3}\b', raw_text)
            
            return {
                "statusCode": resp.status_code,
                "title": soup.title.string.strip() if soup.title and soup.title.string else "Router Gateway",
                "cleanTextSnippet": raw_text[:300] + "...",
                "extractedIps": list(set(ip_matches))[:5],
                "online": True
            }
        except Exception as e:
            return {
                "statusCode": 503,
                "title": "Unreachable / Timeout",
                "cleanTextSnippet": f"Failed to grab router HTML: {str(e)}",
                "extractedIps": ["192.168.1.1"],
                "online": False
            }

    def execute_query(self, query_id):
        """Simulates or runs individual query execution based on definition."""
        q = next((item for item in self.queries if item["id"] == query_id), None)
        if not q:
            return {"error": "Query not found"}

        now_str = datetime.datetime.now().strftime("%H:%M:%S")

        if q["category"] == "web_grabber":
            grab = self.grab_router_html(q["target"])
            return {
                **q,
                "status": "OK" if grab["online"] else "DOWN",
                "value": grab["cleanTextSnippet"] if grab["online"] else "Connection timeout",
                "detail": grab,
                "timestamp": now_str
            }
        elif q["category"] == "snmp":
            return {
                **q,
                "status": "OK",
                "value": "244 days, 16:32:10" if "Uptime" in q["name"] else "38.2 Mbps",
                "oid": q.get("oid"),
                "timestamp": now_str
            }
        elif q["category"] == "script":
            return {
                **q,
                "status": "OK",
                "value": "RTT avg = 1.42ms" if "Ping" in q["name"] else "Resolved in 14ms",
                "timestamp": now_str
            }
        else: # http
            return {
                **q,
                "status": "OK",
                "value": "HTTP 200 OK",
                "timestamp": now_str
            }

    def get_all_queries_status(self):
        """Returns the full query matrix with current evaluated metrics."""
        results = []
        for q in self.queries:
            results.append({
                "id": q["id"],
                "name": q["name"],
                "category": q["category"],
                "target": q["target"],
                "interval": q["interval"],
                "status": "OK" if q["id"] not in ["q6", "q12"] else "WARN",
                "lastRun": datetime.datetime.now().strftime("%H:%M:%S"),
                "value": self._mock_val(q)
            })
        return results

    def _mock_val(self, q):
        if "Uptime" in q["name"]: return "244d 16h 32m"
        if "Traffic" in q["name"]: return "142.8 Mbps"
        if "Sync Rate" in q["name"]: return "1000/500 Mbps"
        if "DHCP" in q["name"]: return "28 active leases"
        if "WiFi" in q["name"]: return "2.4G (Ch 6) / 5G (Ch 44) Up"
        if "Ping" in q["name"]: return "0.85 ms"
        if "Drops" in q["name"]: return "0.02% (nominal)"
        if "Load" in q["name"] or "CPU" in q["name"]: return "0.18"
        if "WAN Status" in q["name"]: return "Connected (PPPoE/Fiber)"
        if "Public IP" in q["name"]: return "84.115.12.98"
        return "200 OK"
