"""
Routes and views for the flask application.
"""

from datetime import datetime
from flask import render_template
from flask import request
from FlaskWebProject1 import app
import json
data = {"temperature": None, "timestamp": str (datetime.now()) }

@app.route('/')
def home():
    """Renders the home page."""
    global data
    return render_template(
        'index.html',
        title='RasPi Live',
        year=2017,
        data=data
    )
    
@app.route('/api/get')
def get():
    return str(123)
    
@app.route('/api/check')
def check():
    """Return connection status"""
    global data
    
    if (data.get("temperature") != None and data.get("timestamp") != None):
        res = {'error': False}
        return app.response_class(
            response=json.dumps(res),
            mimetype='application/json'
        )
    else:
        res = {'error': True}
        return app.response_class(
            response=json.dumps(res),
            mimetype='application/json'
        )

@app.route('/api/temperature', methods=['GET'])
def getTemperature():
    global data
    if (data.get("temperature") != None and data.get("timestamp") != None):
        res = {"error": False, "data": data}
        return app.response_class(
            response=json.dumps(res),
            mimetype='application/json'
        )
    else:
        res = {"error": True, "data": {} }
        return app.response_class(
            response=json.dumps(res),
            mimetype='application/json'
        )

@app.route('/api/temperature', methods=['POST'])
def setTemperature():
    if (request.headers.get('access-key', None) == 'raspi'):
        global data
        requestdata = request.get_json()
        data['temperature'] = requestdata['temperature']
        data['timestamp'] = str (datetime.now())
        res = {'error': False, 'data': data}
        return app.response_class(
            response=json.dumps(res),
            mimetype='application/json'
        )
    else:
        res = {'error': True, 'data': data}
        return app.response_class(
            response=json.dumps(res),
            mimetype='application/json'
        )

