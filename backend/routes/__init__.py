from flask import Blueprint
from .projects_routes import projects_bp
from .sql_routes import sql_bp
from .monitor_routes import monitor_bp
from .collector_routes import collector_bp
from .ping_routes import ping_bp
from .probe_routes import probe_bp

api_blueprint = Blueprint('api', __name__)
api_blueprint.register_blueprint(projects_bp)
api_blueprint.register_blueprint(sql_bp)
api_blueprint.register_blueprint(monitor_bp)
api_blueprint.register_blueprint(collector_bp)
api_blueprint.register_blueprint(ping_bp)
api_blueprint.register_blueprint(probe_bp)
