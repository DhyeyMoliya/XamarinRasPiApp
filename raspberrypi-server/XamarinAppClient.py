import http.client
import json
import RPi.GPIO as GPIO
import dht11
import time

# initialize GPIO
GPIO.setwarnings(False)
GPIO.setmode(GPIO.BCM)
GPIO.cleanup()

# read data using pin 14
instance = dht11.DHT11(pin = 4)

while True:
	con = http.client.HTTPConnection("raspi.azurewebsites.net")
	result = instance.read()
	if result.is_valid():
		con.request("POST","/api/humidity",body=json.dumps({"humidity":result.humidity}),headers={"access-key":"raspi", "Content-Type":"application/json"})
		resp = con.getresponse()
		print(resp.status, resp.reason, resp.read())
		con.request("POST","/api/temperature",body=json.dumps({"temperature":result.temperature}),headers={"access-key":"raspi", "Content-Type":"application/json"})
		resp = con.getresponse()
		print(resp.status, resp.reason, resp.read())
		#print("Temperature: " + str(result.temperature) + " C")
		#print("Humidity: " + str(result.humidity))
	con.close()
	time.sleep(0.2)
