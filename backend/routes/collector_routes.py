from flask import Blueprint, jsonify, request
from collector_handler import CollectorHandler

collector_bp = Blueprint('collector', __name__)
collector_handler = CollectorHandler()

@collector_bp.route('/collector/queries', methods=['POST', 'GET'])
def get_queries():
    results = collector_handler.get_all_queries_status()
    return jsonify({'queries': results, 'total': len(results)})

@collector_bp.route('/collector/execute', methods=['POST'])
def execute_query():
    payload = request.get_json(silent=True) or {}
    query_id = payload.get('query_id')
    if not query_id:
        return jsonify({'error': 'query_id is required'}), 400
    res = collector_handler.execute_query(query_id)
    return jsonify({'result': res})

@collector_bp.route('/collector/grab_router', methods=['POST'])
def grab_router():
    payload = request.get_json(silent=True) or {}
    target_url = payload.get('url', 'http://192.168.1.1')
    data = collector_handler.grab_router_html(target_url)
    return jsonify({'data': data})
