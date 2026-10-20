from flask import Blueprint, jsonify, request
from ping_handler import PingHandler

ping_bp = Blueprint('ping', __name__)
ping_handler = PingHandler()

@ping_bp.route('/ping', methods=['POST'])
def run_ping():
    payload = request.get_json(silent=True) or {}
    host = payload.get('host', '10.20.110.7')
    count = int(payload.get('count', 4))
    res = ping_handler.ping_host(host, count=count)
    return jsonify(res)
