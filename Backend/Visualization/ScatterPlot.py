import os
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from Visualizations import Visualization
from Filter import prepare_dataframe, calculate_theta



class ScatterPlot(Visualization): 

    def __init__(self, dataframe, x_encoding, y_encoding, keywords=None):
        super().__init__(dataframe, "ScatterPlot", keywords)
        #Class varibales
        self.x_encoding = {
            "aggregate": "Null",
            "field": x_encoding["field"],
            "type": x_encoding["type"]
        }
        self.y_encoding = {
            "aggregate": "Null",
            "field": y_encoding["field"],
            "type": y_encoding["type"]
        }
    #return  x-encoding
    def get_x_encoding(self): 
        return self.x_encoding
    #return  y-encoding
    def get_y_encoding(self): 
        return self.y_encoding
    
    #Equivalent to set_fields/set_axis function
    def set_fields(self, xaxis=None, yaxis=None):
        if not xaxis==None:
            self.x_encoding["field"] = xaxis
        if not yaxis==None:    
            self.y_encoding["field"] = yaxis

    #Returns the prepared Data for this visualization
    #applies all properties/keywords/encodings and returns the data
    def get_data(self):
        self.dataframe_prepared = prepare_dataframe(self.dataframe,self.x_encoding["field"],self.y_encoding["field"],self.y_encoding["aggregate"], self.keywords)
        return self.dataframe_prepared
    #returns dictionnary with all information of the object 
    def serialize_object(self): 
        return {"type" : self.type, "keywords" : self.keywords, "x_encoding" : self.x_encoding, "y_encoding" : self.y_encoding}