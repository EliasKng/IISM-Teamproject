from flask import Flask, jsonify
from flask_cors import CORS


#import json
#data = json.load(open('backendOutput.json', 'r'))

# configuration
DEBUG = True

# instantiate the app
app = Flask(__name__)
app.config.from_object(__name__)

# enable CORS
CORS(app, resources={r'/*': {'origins': '*'}})

# list of books
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

#data
""" columns_output = ["Country", "Sales"]
data_output = [["Canada", 2894762.65],["France",6539875.15]]
type_output = 'ColumnChart' """

@app.route('/data', methods=['GET'])
def all_data(): 
    return jsonify('''{
    "columns": [
        "Country",
        "Sales"
    ],
    "data": [
        [
            "Canada",
            2894762.65
        ],
        [
            "France",
            6539875.15
        ],
        [
            "Germany",
            1741522.0214285713
        ],
        [
            "Mexico",
            1119275.1
        ],
        [
            "United States of America",
            3281771.992857143
        ]
    ],
    "type": "BarChart"
    }​​​​​​​''')

   # return deserialize_object(session["final_vis_data"]).jsonify_vis()


@app.route('/books', methods=['GET'])
def all_books():
    return jsonify({
        'status': 'success',
        'books': BOOKS
    })

# sanity check route
@app.route('/ping', methods=['GET'])
def ping_pong():
    return jsonify('pong!')


if __name__ == '__main__':
    app.run()