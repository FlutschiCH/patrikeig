# /backend/app.py
from flask import Flask
from flask_cors import CORS
from gevent.pywsgi import WSGIServer
from routes import api_blueprint  # Aggregated blueprint
import os
import threading

app = Flask(__name__)
CORS(app)

# Register consolidated routes
app.register_blueprint(api_blueprint, url_prefix='/api')

def run_gui(port):
    try:
        import tkinter as tk
        from debug_gui import DebugGUI
        root = tk.Tk()
        gui = DebugGUI(root, port)
        root.mainloop()
    except Exception as e:
        print(f"Tkinter GUI could not be initialized (headless/container mode): {e}")

if __name__ == '__main__':
    port = int(os.environ.get("PORT", 8080))
    
    # Launch high-performance WSGI server in background daemon thread
    http_server = WSGIServer(('0.0.0.0', port), app)
    server_thread = threading.Thread(target=http_server.serve_forever, daemon=True)
    server_thread.start()
    print(f"WSGIServer serving on port {port}...")

    # Launch Tkinter GUI on main thread for local interactive debugging
    run_gui(port)
