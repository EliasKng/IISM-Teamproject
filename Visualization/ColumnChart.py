import os
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from Visualizations import Visualization
from Filter import prepare_dataframe



class ColumnChart(Visualization): 

    def __init__(self, dataframe, x_encoding, y_encoding, keywords=None):
        super().__init__(dataframe, "ColumnChart", keywords)

        #Class varibales 
        self.x_encoding = {
            "aggregate": x_encoding["aggregate"],
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
            if self.x_encoding["type"]=="quantitative" : 
                if maxsum == "max":
                    max_value = self.dataframe_prepared[self.x_encoding["field"]].max()
                    min_value = self.dataframe_prepared[self.x_encoding["field"]].min()
                    self.dataframe_prepared[self.x_encoding["field"]] = (self.dataframe_prepared[self.x_encoding["field"]] - min_value) / (max_value - min_value)
                    return self.dataframe_prepared
                elif maxsum == "sum":
                    sum_value = self.dataframe_prepared[self.x_encoding["field"]].sum()
                    min_value = self.dataframe_prepared[self.x_encoding["field"]].min()
                    self.dataframe_prepared[self.x_encoding["field"]] = (self.dataframe_prepared[self.x_encoding["field"]]) / (sum_value)
                    return self.dataframe_prepared
    
    def get_data(self):
        self.dataframe_prepared = prepare_dataframe(self.dataframe,self.y_encoding["field"],self.x_encoding["field"],self.x_encoding["aggregate"], self.keywords)
        return self.dataframe_prepared
    
    def serialize_object(self): 
        return {"type" : self.type, "keywords" : self.keywords, "x_encoding" : self.x_encoding, "y_encoding" : self.y_encoding}