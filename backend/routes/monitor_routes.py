from flask import Blueprint, jsonify, request
from monitor_handler import MonitorHandler

monitor_bp = Blueprint('monitor', __name__)
monitor_handler = MonitorHandler()

@monitor_bp.route('/banner', methods=['POST'])
def get_banner():
    payload = request.get_json(silent=True) or {}
    ip = payload.get('ip', '10.20.110.7')
    banner = monitor_handler.get_banner(ip)
    return jsonify({'banner': banner, 'ip': ip})

@monitor_bp.route('/devices', methods=['POST'])
def get_devices():
    payload = request.get_json(silent=True) or {}
    custom_devices = payload.get('devices')
    results = monitor_handler.inspect_devices(custom_devices)
    return jsonify({'devices': results})

@monitor_bp.route('/status', methods=['POST', 'GET'])
def server_status():
    return jsonify({
        'status': 'online',
        'service': 'terminal-monitor-backend',
        'code': 200
    })
