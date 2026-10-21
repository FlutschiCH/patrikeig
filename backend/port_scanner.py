import socket
import concurrent.futures

class PortScanner:
    COMMON_PORTS = [
        (22, "SSH"),
        (80, "HTTP"),
        (443, "HTTPS"),
        (8080, "HTTP-Alt / Admin"),
        (8443, "HTTPS-Alt / Admin"),
        (8000, "HTTP-Dev"),
        (8888, "HTTP-Alt"),
        (3000, "Web App"),
        (5000, "Flask / UPnP"),
        (161, "SNMP (UDP)"),
        (53, "DNS"),
        (21, "FTP"),
        (23, "Telnet")
    ]

    @staticmethod
    def check_tcp_port(ip, port, timeout=0.8):
        sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        sock.settimeout(timeout)
        try:
            res = sock.connect_ex((ip, port))
            sock.close()
            return port, (res == 0)
        except Exception:
            return port, False

    @classmethod
    def scan_ports(cls, ip, ports=None):
        if ports is None:
            ports = [p[0] for p in cls.COMMON_PORTS]
        
        open_ports = []
        with concurrent.futures.ThreadPoolExecutor(max_workers=20) as executor:
            future_to_port = {executor.submit(cls.check_tcp_port, ip, p): p for p in ports}
            for future in concurrent.futures.as_completed(future_to_port):
                port, is_open = future.result()
                if is_open:
                    service_desc = next((desc for p, desc in cls.COMMON_PORTS if p == port), "Unknown")
                    open_ports.append({"port": port, "service": service_desc})
        
        open_ports.sort(key=lambda x: x["port"])
        return open_ports
