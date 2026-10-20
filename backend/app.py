# /backend/app.py
from flask import Flask
from flask_cors import CORS
from gevent.pywsgi import WSGIServer
from routes import api_blueprint  # Aggregated blueprint

app = Flask(__name__)
CORS(app)

# Register consolidated routes
app.register_blueprint(api_blueprint, url_prefix='/api')

import os
if __name__ == '__main__':
    # Initialize high-performance WSGI Server
    port = int(os.environ.get("PORT", 8080))
    http_server = WSGIServer(('0.0.0.0', port), app)
    http_server.serve_forever()
