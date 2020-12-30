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


    def change_vistype(self, target_vis): 
        keywords = self.vis_object.keywords
        #currentvis==BarChart 
        if isinstance(self.vis_object, BarChart) and target_vis=="ColumnChart":
            self.vis_object = ColumnChart(self.vis_object.dataframe, self.vis_object.get_y_encoding(), self.vis_object.get_x_encoding(), keywords)
        elif isinstance(self.vis_object, BarChart) and target_vis=="PieChart":
            self.vis_object = PieChart(self.vis_object.dataframe, self.vis_object.get_x_encoding(), self.vis_object.get_y_encoding(), keywords)
        elif isinstance(self.vis_object, BarChart) and target_vis=="ScatterPlot":
            self.vis_object = ScatterPlot(self.vis_object.dataframe, self.vis_object.get_x_encoding(), self.vis_object.get_y_encoding(), keywords)
        #currentvis==ColumnChart 
        if isinstance(self.vis_object, ColumnChart) and target_vis=="BarChart":
            BarChart(self.vis_object.dataframe, self.vis_object.get_y_encoding(), self.vis_object.get_x_encoding(), keywords)
        elif isinstance(self.vis_object, ColumnChart) and target_vis=="PieChart":
            self.vis_object = BarChart(self.vis_object.dataframe, self.vis_object.get_y_encoding(), self.vis_object.get_x_encoding(), keywords)
        elif isinstance(self.vis_object, ColumnChart) and target_vis=="ScatterPlot":
            self.vis_object = ScatterPlot(self.vis_object.dataframe, self.vis_object.get_x_encoding(), self.vis_object.get_y_encoding(), keywords)
        #currentvis==PieChart
        if isinstance(self.vis_object, PieChart) and target_vis=="BarChart":
            self.vis_object = BarChart(self.vis_object.dataframe, self.vis_object.get_color_encoding(), self.vis_object.get_theta_encoding(), keywords)
        elif isinstance(self.vis_object, PieChart) and target_vis=="ColumnChart":
            self.vis_object = BarChart(self.vis_object.dataframe, self.vis_object.get_theta_encoding(), self.vis_object.get_color_encoding(), keywords)
        elif isinstance(self.vis_object, PieChart) and target_vis=="ScatterPlot":
            self.vis_object = ScatterPlot(self.vis_object.dataframe, self.vis_object.get_theta_encoding(), self.vis_object.get_color_encoding(), keywords) 
        #currentvis=Scatterplot 
        if isinstance(self.vis_object, ScatterPlot) and target_vis=="ColumnChart":
            if self.vis_object.get_y_encoding()["type"] == "nominal" and self.vis_object.get_x_encoding()["type"] == "quantitative":
                     x_encoding = self.vis_object.get_x_encoding()
                     x_encoding["aggregate"] = "sum"
                     self.vis_object = BarChart(self.vis_object.dataframe, self.vis_object.get_y_encoding(), x_encoding, keywords)
            elif self.vis_object.get_x_encoding()["type"] == "nominal" and self.vis_object.get_y_encoding()["type"] == "quantitative":
                    y_encoding = self.vis_object.get_y_encoding()
                    y_encoding["aggregate"] = "sum"
                    self.vis_object = BarChart(self.vis_object.dataframe, y_encoding, self.vis_object.get_x_encoding(), keywords)
            else: 
                self.vis_object = BarChart(self.vis_object.dataframe, self.vis_object.get_x_encoding(), self.vis_object.get_y_encoding(), keywords)
        elif isinstance(self.vis_object, ScatterPlot) and target_vis=="PieChart":
            if self.vis_object.get_x_encoding()["type"] == "nominal" and self.vis_object.get_y_encoding()["type"] == "quantitative":
                     y_encoding = self.vis_object.get_y_encoding()
                     y_encoding["aggregate"] = "sum"
                     self.vis_object = PieChart(self.vis_object.dataframe, self.vis_object.get_x_encoding(), y_encoding, keywords)
            elif self.vis_object.get_y_encoding()["type"] == "nominal" and self.vis_object.get_x_encoding()["type"] == "quantitative":
                    x_encoding = self.vis_object.get_x_encoding()
                    x_encoding["aggregate"] = "sum"
                    self.vis_object = PieChart(self.vis_object.dataframe, x_encoding, self.vis_object.get_y_encoding(), keywords)
        elif isinstance(self.vis_object, ScatterPlot) and target_vis=="BarChart":
            if self.vis_object.get_x_encoding()["type"] == "nominal" and self.vis_object.get_y_encoding()["type"] == "quantitative":
                     y_encoding = self.vis_object.get_y_encoding()
                     y_encoding["aggregate"] = "sum"
                     self.vis_object = BarChart(self.vis_object.dataframe, self.vis_object.get_x_encoding(), y_encoding, keywords)
            elif self.vis_object.get_y_encoding()["type"] == "nominal" and self.vis_object.get_x_encoding()["type"] == "quantitative":
                    x_encoding = self.vis_object.get_x_encoding()
                    x_encoding["aggregate"] = "sum"
                    self.vis_object = BarChart(self.vis_object.dataframe, x_encoding, self.vis_object.get_y_encoding(), keywords)
            else: 
                self.vis_object = BarChart(self.vis_object.dataframe, self.vis_object.get_x_encoding(), self.vis_object.get_y_encoding(), keywords)
        
        

    #returns parsed JSON for frontend
    def jsonify_vis(self): 
        result = (self.vis_object.get_data()).reset_index().to_json(orient="split")
        parsed = json.loads(result)
        parsed.update({"-type": self.vis_object.type})
        #entferne überflüssige infos 
        parsed.pop("index")
        return json.dumps(parsed, indent=4)
    

    def serialize_object(self):
        return self.vis_object.serialize_object()

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