import requests
import json
import urllib3

# Suppress self-signed certificate warnings for local appliances
urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)

class ProbeHandler:
    COMMON_PATHS = [
        "/",
        "/api",
        "/api/status",
        "/api/system",
        "/api/system/status",
        "/api/v1/status",
        "/status",
        "/status.cgi",
        "/status.json",
        "/info",
        "/info.html",
        "/sysinfo",
        "/currentsetting.htm",
        "/metrics",
        "/openapi.json",
        "/swagger.json",
        "/robots.txt",
        "/cgi-bin/luci",
        "/ubus",
        "/tr64desc.xml"
    ]

    @staticmethod
    def execute_request(url, method="GET", headers=None, payload=None, timeout=3.5):
        """Executes an HTTP/HTTPS probe with cURL-like verbose details."""
        if not url.startswith("http://") and not url.startswith("https://"):
            url = f"http://{url}"

        req_headers = headers if headers is not None else {
            "User-Agent": "curl/8.4.0 (patrikeig-debugger)"
        }

        try:
            resp = requests.request(
                method=method.upper(),
                url=url,
                headers=req_headers,
                data=payload,
                timeout=timeout,
                verify=False,
                allow_redirects=True
            )

            # Format response body nicely
            content_type = resp.headers.get("Content-Type", "")
            raw_text = resp.text

            formatted_body = raw_text
            if "application/json" in content_type:
                try:
                    formatted_body = json.dumps(resp.json(), indent=2)
                except Exception:
                    pass
            elif len(raw_text) > 4000:
                formatted_body = raw_text[:4000] + "\n... [TRUNCATED]"

            return {
                "url": url,
                "method": method.upper(),
                "status_code": resp.status_code,
                "reason": resp.reason,
                "headers": dict(resp.headers),
                "elapsed_ms": int(resp.elapsed.total_seconds() * 1000),
                "content_type": content_type,
                "body": formatted_body,
                "success": True
            }

        except requests.exceptions.Timeout:
            return {
                "url": url,
                "method": method.upper(),
                "status_code": 408,
                "reason": "Request Timeout",
                "headers": {},
                "elapsed_ms": None,
                "content_type": "",
                "body": "Connection timed out after {0}s.".format(timeout),
                "success": False
            }
        except requests.exceptions.ConnectionError as e:
            return {
                "url": url,
                "method": method.upper(),
                "status_code": 503,
                "reason": "Connection Refused / Unreachable",
                "headers": {},
                "elapsed_ms": None,
                "content_type": "",
                "body": f"Connection error: {str(e)}",
                "success": False
            }
        except Exception as e:
            return {
                "url": url,
                "method": method.upper(),
                "status_code": 500,
                "reason": "Internal Error",
                "headers": {},
                "elapsed_ms": None,
                "content_type": "",
                "body": str(e),
                "success": False
            }

    @staticmethod
    def scan_common_paths(base_url, timeout=1.5):
        """Scans standard endpoints and returns responsive ones."""
        if not base_url.startswith("http://") and not base_url.startswith("https://"):
            base_url = f"http://{base_url}".rstrip("/")

        results = []
        for path in ProbeHandler.COMMON_PATHS:
            target = f"{base_url}{path}"
            res = ProbeHandler.execute_request(target, method="GET", timeout=timeout)
            if res["status_code"] not in [404, 503, 408]:
                results.append({
                    "path": path,
                    "target": target,
                    "status_code": res["status_code"],
                    "reason": res["reason"],
                    "content_type": res["content_type"]
                })
        return results
