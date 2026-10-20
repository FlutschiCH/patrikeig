from flask import Blueprint, jsonify, request
from probe_handler import ProbeHandler

probe_bp = Blueprint('probe', __name__)
probe_handler = ProbeHandler()

@probe_bp.route('/probe/curl', methods=['POST'])
def probe_curl():
    payload = request.get_json(silent=True) or {}
    url = payload.get('url', 'http://10.20.110.7')
    method = payload.get('method', 'GET')
    headers = payload.get('headers')
    body = payload.get('body')
    timeout = float(payload.get('timeout', 3.5))

    res = probe_handler.execute_request(url, method=method, headers=headers, payload=body, timeout=timeout)
    return jsonify(res)

@probe_bp.route('/probe/scan', methods=['POST'])
def probe_scan():
    payload = request.get_json(silent=True) or {}
    base_url = payload.get('base_url', 'http://10.20.110.7')
    found = probe_handler.scan_common_paths(base_url)
    return jsonify({'base_url': base_url, 'found_endpoints': found})
