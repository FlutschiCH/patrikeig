from flask import Blueprint, jsonify, request
from fiveg_handler import FiveGHandler

fiveg_bp = Blueprint('fiveg', __name__)
fiveg_handler = FiveGHandler()

@fiveg_bp.route('/fiveg/data', methods=['POST', 'GET'])
def get_5g_data():
    data = fiveg_handler.get_dashboard_data()
    return jsonify(data)
