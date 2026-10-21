from flask import Blueprint, jsonify, request
from ssh_handler import SSHHandler

ssh_bp = Blueprint('ssh', __name__)
ssh_handler = SSHHandler()

@ssh_bp.route('/ssh/exec', methods=['POST'])
def run_ssh():
    payload = request.get_json(silent=True) or {}
    host = payload.get('host', '10.20.110.7')
    user = payload.get('user', 'root')
    password = payload.get('password')
    command = payload.get('command', 'ss -tulpn')
    timeout = float(payload.get('timeout', 5.0))

    res = ssh_handler.execute_command(host=host, user=user, password=password, command=command, timeout=timeout)
    return jsonify(res)
