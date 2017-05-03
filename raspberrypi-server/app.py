from flask import *
from w1thermsensor import W1ThermSensor
import RPi.GPIO as gpio
import time

trig = 23
echo = 24

gpio.setmode(gpio.BCM)
gpio.setwarnings(False)
gpio.setup(trig,gpio.OUT)
gpio.setup(echo,gpio.IN)

app = Flask(__name__)

@app.route('/check', methods=['GET'])
def check():
	gpio.setup(trig,False)
	time.sleep(1)
	return jsonify({'error':False})


@app.route('/ultrasonic', methods=['GET'])
def ultrasonic():
	gpio.output(trig,True)
	time.sleep(0.00001)
	gpio.output(trig,False)
	while(gpio.input(echo)==0):
		pulse_start = time.time()
	while(gpio.input(echo)==1):
		pulse_end = time.time()
	distance = (pulse_end - pulse_start) * 17150
	distance = round(distance,2)
	return jsonify({'value':distance})

@app.route('/temperature', methods=['GET'])
def temperature():
        for sensor in W1ThermSensor.get_available_sensors():
		return jsonify({'value':sensor.get_temperature()})

if __name__ == '__main__':
	app.run(host='0.0.0.0', port=5000, debug=True)
