from flask import Blueprint, jsonify, request
from sql_handler import SQLHandler

sql_bp = Blueprint('sql', __name__)
sql_handler = SQLHandler()

@sql_bp.route('/sql/upsert', methods=['POST'])
def upsert_status():
    payload = request.get_json(silent=True) or {}
    device_id = payload.get('device_id')
    name = payload.get('name', 'Unknown')
    ip = payload.get('ip', '')
    url = payload.get('url', '')
    status_code = payload.get('status_code', 0)
    status_text = payload.get('status_text', 'UNKNOWN')
    banner_blob = payload.get('banner_blob', '').encode('utf-8') if payload.get('banner_blob') else None

    if not device_id:
        return jsonify({'error': 'device_id required'}), 400

    success = sql_handler.upsert_device_status(device_id, name, ip, url, status_code, status_text, banner_blob)
    return jsonify({'success': success})
