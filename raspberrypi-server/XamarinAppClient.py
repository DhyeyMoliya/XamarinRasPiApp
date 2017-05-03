import http.client
import json
import time
from w1thermsensor import W1ThermSensor

while True:
        for sensor in W1ThermSensor.get_available_sensors():
                con = http.client.HTTPConnection("raspi.azurewebsites.net")
                con.request("POST","/api/temperature",body=json.dumps({"tempe$
                resp = con.getresponse()
                print(resp.status, resp.reason, resp.read())
                con.close()
                time.sleep(0.2)
