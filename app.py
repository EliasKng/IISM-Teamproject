import os
import sys
import json
import requests

sys.path.append(os.path.join(".", os.path.dirname(os.path.abspath(__file__)), "Visualization"))

import numpy as np
import pandas as pd
from Filter import prepare_dataframe, calculate_theta, final_prep
from EyeTracker import EyeTracker, is_in_polygon
from BarChart import BarChart
from ColumnChart import ColumnChart
from PieChart import PieChart
from ScatterPlot import ScatterPlot
from VisHandler import VisHandler
from flask import Flask, jsonify, request, session, make_response
from flask_cors import CORS, cross_origin
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

#Import XLXS Data
#working_dataframe = pd.read_csv(os.path.join(".", os.path.dirname(os.path.abspath(__file__)), "examples", "assets", "data", "FinancialSample.csv"))
working_dataframe = pd.read_excel(os.path.join(".", os.path.dirname(os.path.abspath(__file__)), "examples", "assets", "data", "FinancialSample.xlsx"), engine='openpyxl')


#help function to create object 
def deserialize_object(final_vis_data): 
    if final_vis_data["type"] == "BarChart": 
        bar_chart = BarChart(working_dataframe, final_vis_data["x_encoding"], final_vis_data["y_encoding"], final_vis_data["keywords"])
        return VisHandler(bar_chart)
    if final_vis_data["type"] == "ColumnChart": 
        column_chart = ColumnChart(working_dataframe, final_vis_data["x_encoding"], final_vis_data["y_encoding"], final_vis_data["keywords"])
        return VisHandler(column_chart)
    if  final_vis_data["type"] == "PieChart":
        pie_chart = PieChart(working_dataframe, final_vis_data["color_encoding"], final_vis_data["theta_encoding"], final_vis_data["keywords"])
        return VisHandler(pie_chart)
    if  final_vis_data["type"] == "ScatterPlot":
        scatter_plot = ScatterPlot(working_dataframe, final_vis_data["x_encoding"], final_vis_data["y_encoding"], final_vis_data["keywords"])
        return VisHandler(scatter_plot)


#*********************************************configure app ********************************************************************
#configuration
DEBUG = True

# instantiate the app
app = Flask(__name__)
app.secret_key = "wbkVWnT0zV!6StVzLUG2PwQTr"
app.config.from_object(__name__)


# enable CORS
CORS(app, supports_credentials=True, headers=['Content-Type'], expose_headers=['Access-Control-Allow-Origin'])


#********************************************************************************************************************************

#change object
@app.route('/change', methods=['GET', 'POST'])
def change_object():
    if request.method == "POST": 
        post_data = request.get_json()
        temp_vis = deserialize_object(session["final_vis_data"])
        temp_vis.change_vistype(post_data["target_vis"])
        session["final_vis_data"] = temp_vis.serialize_object()
        return temp_vis.jsonify_vis()


#**************************KEYWORDS********************************************
#set keywords 
@app.route('/keywords/set', methods=['GET', 'POST'])
def set_keywords():
    if request.method == "POST":
        post_data = request.get_json()
        temp_vis = deserialize_object(session["final_vis_data"])
        temp_vis.vis_object.set_keywords(post_data["keywords"])
        session["final_vis_data"] = temp_vis.serialize_object()
        response = make_response(temp_vis.jsonify_vis(), 200)
        response.headers['Access-Control-Allow-Credentials'] = 'true'
        response.headers['Access-Control-Allow-Origin'] = 'http://localhost:3000'
        return response


#add/update number filter
@app.route('/keywords/add-number', methods=['GET', 'POST'])
def add_number():
    if request.method == "POST":
        post_data = request.get_json()
        add_number_filter = {post_data["column"] : [post_data["comparisonOperator"], [post_data["number"]]]}
        temp_vis = deserialize_object(session["final_vis_data"])
        temp_vis.vis_object.add_keyword(add_number_filter)
        session["final_vis_data"] = temp_vis.serialize_object()
        return temp_vis.jsonify_vis()

#add/update nominal filter
@app.route('/keywords/add-word', methods=['GET', 'POST'])
def add_word():
    if request.method == "POST":
        post_data = request.get_json()
        print(post_data)
        add_word_filter = {post_data["column"] : ["in", post_data["values"]]}
        temp_vis = deserialize_object(session["final_vis_data"])
        temp_vis.vis_object.add_keyword(add_word_filter)
        session["final_vis_data"] = temp_vis.serialize_object()
        return  temp_vis.jsonify_vis()


#delete all keywords/ all applied fiter 
@app.route('/keywords/delete/all', methods=['GET', 'POST'])
def delete_all_keywords():
    if request.method == "POST":
        temp_vis = deserialize_object(session["final_vis_data"])
        temp_vis.vis_object.set_keywords({})
        session["final_vis_data"] = temp_vis.serialize_object()
        return temp_vis.jsonify_vis()

#delete single keyword / applied filter
@app.route('/keywords/delete', methods=['GET', 'POST'])
def delete_single_keyword():
    if request.method == "POST":
        post_data = request.get_json()
        temp_vis = deserialize_object(session["final_vis_data"])
        temp_vis.vis_object.delete_keyword(post_data["keywords"])
        session["final_vis_data"] = temp_vis.serialize_object()
        return temp_vis.jsonify_vis()

#************************************************************************************
#set/change fields 
@app.route('/change-fields', methods=['GET', 'POST'])
def change_fields():
    if request.method == "POST":
        post_data = request.get_json()
        temp_vis = deserialize_object(session["final_vis_data"])
        if post_data["x-color"] and post_data["y-theta"]=="null":
            temp_vis.vis_object.set_fields(post_data["x-color"])
        else:
            temp_vis.vis_object.set_fields(None, post_data["y-theta"])
        session["final_vis_data"] = temp_vis.serialize_object()
        return temp_vis.jsonify_vis()


#set/change aggregate 
@app.route('/change-aggregate', methods=['GET', 'POST'])
def change_aggregate():
    if request.method == "POST":
        post_data = request.get_json()
        temp_vis = deserialize_object(session["final_vis_data"])
        if post_data["x-color"] and post_data["y-theta"]=="null":
            temp_vis.vis_object.set_aggregate(post_data["x-color"])
        else:
            temp_vis.vis_object.set_aggregate(None, post_data["y-theta"])
        session["final_vis_data"] = temp_vis.serialize_object()
    return  temp_vis.jsonify_vis()

#nl4dv
@app.route('/nl4dv', methods=[ 'GET', 'POST'])
def nl4dv_query():
    if request.method == "POST": 
        post_data = request.get_json()
        nl4dv_results = nl4dv_output_parser(nl4dvQueryAnalyzerFinancialsDataset(post_data["query"]))
        #decide which object to create 
        if nl4dv_results["vis_type"] == "bar": 
            bar_chart = BarChart(working_dataframe, nl4dv_results["encoding"]["x"], nl4dv_results["encoding"]["y"])
            final_vis =  VisHandler(bar_chart)
            session["final_vis_data"] = final_vis.serialize_object()
        if nl4dv_results["vis_type"] == "arc":
            pie_chart = PieChart(working_dataframe, nl4dv_results["encoding"]["color"], nl4dv_results["encoding"]["theta"])
            final_vis =  VisHandler(pie_chart)
            session["final_vis_data"] = final_vis.serialize_object()
        if nl4dv_results["vis_type"] == "point":
            print(nl4dv_results)
            scatter_plot = ScatterPlot(working_dataframe, nl4dv_results["encoding"]["x"], nl4dv_results["encoding"]["y"])
            final_vis = VisHandler(scatter_plot)
            session["final_vis_data"] = final_vis.serialize_object()
    return final_vis.jsonify_vis()



#eye-tracker
@app.route('/eye-tracker', methods=['GET', 'POST'])
def eye_tracker():
    if request.method == "POST": 
        post_data = request.get_json()
        eye_tracker = EyeTracker(post_data["elements"])
        return jsonify(eye_tracker.execute())



#Data for frontend
@app.route('/data', methods=['GET'])
def all_data():
    if  session.get("final_vis_data") is not None:
        return deserialize_object(session["final_vis_data"]).jsonify_vis()
    else: 
        return jsonify("null")

if __name__ == '__main__':
    app.run()
