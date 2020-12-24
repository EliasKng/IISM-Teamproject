import os
import json
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from Visualizations import Visualization
from BarChart import BarChart
from PieChart import PieChart 
from ColumnChart import ColumnChart
from ScatterPlot import ScatterPlot

class VisHandler(): 
    def __init__(self, vis_object): 
        self.vis_object = vis_object


    def change_vistype(self, current_vis, target_vis): 
        keywords = current_vis.keywords

        #currentvis==BarChart 
        if isinstance(current_vis, BarChart) and target_vis=="ColumnChart":
            self.vis_object = ColumnChart(current_vis.dataframe, current_vis.get_y_encoding(), current_vis.get_x_encoding(), keywords)
            return self.vis_object
        elif isinstance(current_vis, BarChart) and target_vis=="PieChart":
            self.vis_object = PieChart(current_vis.dataframe, current_vis.get_x_encoding(), current_vis.get_y_encoding(), keywords)
            return self.vis_object
        #currentvis==ColumnChart 
        if isinstance(current_vis, ColumnChart) and target_vis=="BarChart":
            BarChart(current_vis.dataframe, current_vis.get_y_encoding(), current_vis.get_x_encoding(), keywords)
            return self.vis_object
        elif isinstance(current_vis, ColumnChart) and target_vis=="PieChart":
            self.vis_object = BarChart(current_vis.dataframe, current_vis.get_y_encoding(), current_vis.get_x_encoding(), keywords)
            return self.vis_object
        #currentvis==PieChart
        if isinstance(current_vis, PieChart) and target_vis=="BarChart":
            self.vis_object = BarChart(current_vis.dataframe, current_vis.get_color_encoding(), current_vis.get_theta_encoding(), keywords)
            return self.vis_object
        elif isinstance(current_vis, PieChart) and target_vis=="ColumnChart":
            self.vis_object = BarChart(current_vis.dataframe, current_vis.get_theta_encoding(), current_vis.get_color_encoding(), keywords)
            return self.vis_object

    #returns parsed JSON for frontend
    def jsonify_vis(self): 
        result = (self.vis_object.get_data()).reset_index().to_json(orient="split")
        parsed = json.loads(result)
        print(self.vis_object.type)
        parsed.update({"-type": self.vis_object.type})
        return json.dumps(parsed, indent=4)

'''
#test data
keywords = {"Product" : ["==", "Carretera"]}
x_encoding = {"field": "Country", "type": "nominal"}
y_encoding = {"aggregate": "mean", "field": "Sales", "type": "quantitative"}


#Import CSV Data
working_dataframe = pd.read_csv(os.path.join(".", os.path.dirname(os.path.abspath(__file__)), "examples", "assets", "data", "FinancialSample.csv"))


#create Objects
b1 = BarChart(working_dataframe, x_encoding, y_encoding, keywords)
vishandler1 =  VisHandler(b1)
result = vishandler1.vis_object.get_data().to_json(orient="split")
parsed = 
print(vishandler1.vis_object.get_data())
print(result)
'''