from flask import Flask, render_template, url_for, redirect   # url_for 함수, redirect 함수 추가
from flask_mqtt import Mqtt
from flask_socketio import SocketIO
from gpiozero import LEDBoard, Servo

import json
import datetime as dt
from collections import OrderedDict

app = Flask(__name__)
app.config['MQTT_BROKER_URL'] = '192.168.200.105'
app.config['MQTT_BROKER_PORT'] = 1883
app.config['MQTT_REFRSH_TIME'] = 1.0
mqtt = Mqtt(app)

leds = LEDBoard(18, 23, 24)              # leds 객체 생성 BCM
# servo = Servo(17)                       # 17번 연결

led_states = {                           # 전체 led의 현황 표시용
    'red':0,
    'green':0,
    'blue':0
}

@mqtt.on_connect()
def handle_connect(client, userdata, flags, rc): 
    print("Connected with result code {0}".format(str(rc)))
    # mqtt.subscribe("machine01/4002/", qos=1)

@mqtt.on_message()
def handle_message(client, userdata, message):
    data = dict(
        topic=message.topic,
        payload=message.payload.decode()
    )
    SocketIO.emit('mqtt_message', data=data)

@mqtt.on_log()
def handle_logging(client, userdata, level, buf):
    print(level, buf)

@app.route('/')                       # 기본주소('/')로 들어오면
def home():
    return render_template('index.html', led_states = led_states)   #index.html에 전체 led현황을 함께 전달 

@app.route('/<color>/<int:state>')                                # 개별 led를 켜고 끄는 주소
def led_switch(color, state):                                    # 개별 led ON, OFF 함수
    led_states[color] = state  
    leds.value=tuple(led_states.values())
    currtime = dt.datetime.now().strftime('%Y-%m-%d %H:%M:%S')

    raw_data = OrderedDict()
    raw_data["dev_addr"] = '4001'
    raw_data["currtime"] = currtime
    raw_data["color"] = color
    raw_data["state"] = state
    pub_data = json.dumps(raw_data, ensure_ascii=False, indent='\t')

    mqtt.publish("machine01/4001/", pub_data, 1)
    return redirect(url_for('home'))                           # leds의 색상변경이 완료되면 redirect함수를 통해 기본주소('/')으로 이동

@app.route('/<int:state>')
def servo_run(state):
    print(state)
    # if state == 0:
    #     servo.min()
    # elif state == 1:
    #     servo.mid()
    return redirect(url_for('home'))

if __name__ == '__main__':
    app.run(debug=True, port=8080, host='0.0.0.0')
    