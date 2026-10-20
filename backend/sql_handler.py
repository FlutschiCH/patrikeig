import threading
import mysql.connector
from mysql.connector import pooling
import os

class SQLHandler:
    _lock = threading.Semaphore(1)

    def __init__(self):
        self.remote_config = {
            'host': os.environ.get('DB_REMOTE_HOST', '10.20.110.7'),
            'user': os.environ.get('DB_USER', 'root'),
            'password': os.environ.get('DB_PASSWORD', ''),
            'database': os.environ.get('DB_NAME', 'terminal_monitor'),
            'connection_timeout': 3
        }
        self.local_config = {
            'host': os.environ.get('DB_LOCAL_HOST', '127.0.0.1'),
            'user': os.environ.get('DB_USER', 'root'),
            'password': os.environ.get('DB_PASSWORD', ''),
            'database': os.environ.get('DB_NAME', 'terminal_monitor'),
            'connection_timeout': 3
        }
        self.active_config = self.remote_config

    def get_connection(self):
        with SQLHandler._lock:
            try:
                return mysql.connector.connect(**self.remote_config)
            except Exception:
                try:
                    return mysql.connector.connect(**self.local_config)
                except Exception:
                    return None

    def upsert_device_status(self, device_id, name, ip, url, status_code, status_text, raw_banner=None):
        with SQLHandler._lock:
            conn = self.get_connection()
            if not conn:
                return False
            try:
                cursor = conn.cursor()
                query = """
                INSERT INTO device_status (device_id, name, ip, url, status_code, status_text, banner_blob, updated_at)
                VALUES (%s, %s, %s, %s, %s, %s, %s, NOW())
                ON DUPLICATE KEY UPDATE
                    name = VALUES(name),
                    ip = VALUES(ip),
                    url = VALUES(url),
                    status_code = VALUES(status_code),
                    status_text = VALUES(status_text),
                    banner_blob = VALUES(banner_blob),
                    updated_at = NOW();
                """
                cursor.execute(query, (device_id, name, ip, url, status_code, status_text, raw_banner))
                conn.commit()
                cursor.close()
                conn.close()
                return True
            except Exception:
                if conn:
                    conn.close()
                return False
