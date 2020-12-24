import os
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from Visualizations import Visualization
from Filter import prepare_dataframe




class BarChart(Visualization): 

    def __init__(self, dataframe, x_encoding, y_encoding, keywords=None):
        super().__init__(dataframe, "BarChart", keywords)
        #Class variables 
        self.x_encoding = {
            "aggregate": None,
            "field": x_encoding["field"],
            "type": x_encoding["type"]
        }
        self.y_encoding = {
            "aggregate": y_encoding["aggregate"],
            "field": y_encoding["field"],
            "type": y_encoding["type"]
        }
        
    def get_x_encoding(self): 
        return self.x_encoding

    def get_y_encoding(self): 
        return self.y_encoding

    #set fields for x and or y axis s
    def set_axis(self, xaxis=None, yaxis=None):
        if not xaxis==None:
            self.x_encoding["field"] = xaxis
        if not yaxis==None:    
            self.y_encoding["field"] = yaxis

    def set_aggregate(self, xaggregate=None, yaggregate=None):
        self.x_encoding["aggregate"] = xaggregate
        self.y_encoding["aggregate"] = yaggregate
         
    def normalize_values(self, maxsum):
        if not (self.dataframe_prepared.empty): 
            if self.y_encoding["type"]=="quantitative" : 
                if maxsum == "max":
                    max_value = self.dataframe_prepared[self.y_encoding["field"]].max()
                    min_value = self.dataframe_prepared[self.y_encoding["field"]].min()
                    self.dataframe_prepared[self.y_encoding["field"]] = (self.dataframe_prepared[self.y_encoding["field"]] - min_value) / (max_value - min_value)
                    return self.dataframe_prepared
                elif maxsum == "sum":
                    sum_value = self.dataframe_prepared[self.y_encoding["field"]].sum()
                    min_value = self.dataframe_prepared[self.y_encoding["field"]].min()
                    self.dataframe_prepared[self.y_encoding["field"]] = (self.dataframe_prepared[self.y_encoding["field"]]) / (sum_value)
                    return self.dataframe_prepared

    def get_data(self):
        self.dataframe_prepared = prepare_dataframe(self.dataframe,self.x_encoding["field"],self.y_encoding["field"],self.y_encoding["aggregate"], self.keywords)
        return self.dataframe_prepared