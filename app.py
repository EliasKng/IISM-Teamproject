import os
import sys
import json
import requests

sys.path.append(os.path.join(".", os.path.dirname(os.path.abspath(__file__)), "Visualization"))

import numpy as np
import pandas as pd
from Filter import prepare_dataframe, calculate_theta, final_prep
from BarChart import BarChart
from ColumnChart import ColumnChart
from PieChart import PieChart
from ScatterPlot import ScatterPlot
from VisHandler import VisHandler
from flask import Flask, jsonify, request, session
from flask_cors import CORS
from nl4dv import NL4DV
from nl4dv_parser import nl4dv_output_parser
from jsonPrinter import jsonPrettyPrinter

#NL4DV
def nl4dvQueryAnalyzerFinancialsDataset(query) :
    full_path = os.path.abspath(__file__)
    nl4dv_instance = NL4DV(data_url = os.path.join(".", os.path.dirname(os.path.abspath(__file__)), "examples", "assets", "data", "FinancialSample.csv"))
    dependency_parser_config = {"name": "corenlp", "model": os.path.join(".", os.path.dirname(os.path.abspath(__file__)), "examples","assets","jars","stanford-english-corenlp-2018-10-05-models.jar"),"parser": os.path.join(".", os.path.dirname(os.path.abspath(__file__)), "examples","assets","jars","stanford-parser.jar")}
    nl4dv_instance.set_dependency_parser(config=dependency_parser_config)
    queryInput = query
    return nl4dv_instance.analyze_query(queryInput)

#test data
#keywords = {"Country": ["in", ["France", "Canada"]], "Segment": ["in", ["Government", "Enterprise"]]}
#x_encoding = {"field": "Country", "type": "quantitative"}
#y_encoding = {"aggregate":"sum", "field": "Sales", "type": "quantitative"}


#Import XLXS Data
#working_dataframe = pd.read_csv(os.path.join(".", os.path.dirname(os.path.abspath(__file__)), "examples", "assets", "data", "FinancialSample.csv"))
working_dataframe = pd.read_excel(os.path.join(".", os.path.dirname(os.path.abspath(__file__)), "examples", "assets", "data", "FinancialSample.xlsx"), engine='openpyxl')


#create and modify Objects
#b1 = BarChart(working_dataframe, x_encoding, y_encoding, keywords)
#final_vis =  VisHandler(b1)
#final_vis.vis_object.set_aggregate(None, "mean")



#configuration
DEBUG = True

# instantiate the app
app = Flask(__name__)
app.secret_key = "wbkVWnT0zV!8oQJeiWjDYG6StVzLUG2PwQTr5NHP6$&R^i0huxdt4#eNUXbbsoFOsV*AeLLAsN8jG$lZGsQcJHXuR@hh8RWZoPHWK6d116CZCy^NaMIaz5ukTM3gVopOE5Le8vIC8piX4Eynu10zC0"
app.config.from_object(__name__)

# enable CORS
CORS(app, resources={r'/*': {'origins': '*'}})


#"main"
@app.route('/nl4dv', methods=['GET', 'POST'])
def nl4dv_query():
    if request.method == "POST": 
        post_data = request.get_json()
        nl4dv_results = nl4dv_output_parser(nl4dvQueryAnalyzerFinancialsDataset(post_data["query"]))
        #decide which object to create 
        if nl4dv_results["vis_type"] == "bar": 
            bar_chart = BarChart(working_dataframe, nl4dv_results["encoding"]["x"], nl4dv_results["encoding"]["y"])
            final_vis =  VisHandler(bar_chart)
            session["all_data"] = final_vis.jsonify_vis()
            session["final_vis_data"] = final_vis.__dict__
            print(session["final_vis_data"])
        if nl4dv_results["vis_type"] == "arc":
            pie_chart = PieChart(working_dataframe, nl4dv_results["encoding"]["color"], nl4dv_results["encoding"]["theta"])
            final_vis =  VisHandler(pie_chart)
            session["all_data"] = final_vis.jsonify_vis()
            session["final_vis_data"] = json.dumps(final_vis.__dict__, lambda o: o.__dict__, indent=4)
        if nl4dv_results["vis_type"] == "point":
            scatter_plot = ScatterPlot(working_dataframe, nl4dv_results["encoding"]["x"], nl4dv_results["encoding"]["y"])
            final_vis = VisHandler(scatter_plot)
            session["all_data"] = final_vis.jsonify_vis()
            session["final_vis_data"] = json.dumps(final_vis.__dict__, lambda o: o.__dict__, indent=4)

    
    return jsonify(nl4dv_results)

@app.route('/data', methods=['GET'])
def all_data():
    if "all_data" in session: 
        return (session["all_data"])
    else: 
        return jsonify({"Status" : "Fail"})


if __name__ == '__main__':
    app.run()