from flask import Flask, jsonify, request
from flask_cors import CORS
import json


# configuration
DEBUG = True

# instantiate the app
app = Flask(__name__)
app.config.from_object(__name__)

# enable CORS
CORS(app, resources={r'/*': {'origins': '*'}})

BOOKS = [
    {
        'title': 'On the Road',
        'author': 'Jack Kerouac',
        'read': True
    },
    {
        'title': 'Harry Potter and the Philosopher\'s Stone',
        'author': 'J. K. Rowling',
        'read': False
    },
    {
        'title': 'Green Eggs and Ham',
        'author': 'Dr. Seuss',
        'read': True
    }
]

CHARDATA = [
        {   'name': '1992', 
            'total' : '4748' 
            },
        {   'name': '1993', 
            'total': '5526' 
        },
        {   'name': '1994', 
            'total': '8574' 
        },
        {   'name': '1995', 
            'total': '15805' 
        },
        {   'name': '1996', 
            'total': '14582'
        },
        {   'name': '1997', 
            'total': '26694' 
        },
        {   'name': '1998', 
            'total': '35205' 
        },
        {   'name': '1999', 
            'total': '45944' 
        },
        {   'name': '2000', 
            'total': '78595' 
        },
        {   'name': '2001', 
            'total': '78530' 
        },
        {   'name': '2002', 
            'total': '45407'
        },
        {   'name': '2003', 
            'total': '54044' 
        },
        {   'name': '2004', 
            'total': '69165' 
        },
        {   'name': '2005', 
            'total': '61798' 
        },
        {   'name': '2006', 
            'total': '63686' 
        },
]

PIEDATA ={
    "data": [
        [
            "Canada",
            66.8999622162
        ],
        [
            "France",
            151.1410272044
        ],
        [
            "Germany",
            40.2477755585
        ],
        [
            "Mexico",
            25.8672198564
        ],
        [
            "United States of America",
            75.8440151646
        ]
    ],
    "type": "PieChart",
    "columns": {
        "color": "Country",
        "theta": "Sales"
    }
}

# sanity check route
@app.route('/ping', methods=['GET'])
def ping_pong():
    return jsonify('pong!')
    
@app.route('/books', methods=['GET', 'POST'])
def all_books():
    response_object = {'status': 'success'}
    if request.method == 'POST':
        post_data = request.get_json()
        BOOKS.append({
            'title': post_data.get('title'),
            'author': post_data.get('author'),
            'read': post_data.get('read')
        })
        response_object['message'] = 'Book added!'
    else:
        response_object['books'] = BOOKS
    return jsonify(response_object)

@app.route('/data', methods=['GET'])
def all_data():
    response_object = {'status': 'success'}
    with open("C:\\Users\\User\\Desktop\\bücher app\\Server\\SCATTERDATA.json") as myfile:
        data=json.load(myfile)
    response_object['data'] = data
    return jsonify(response_object)


if __name__ == '__main__':
    app.run()

