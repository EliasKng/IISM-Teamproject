import os
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from Visualizations import Visualization
from Filter import prepare_dataframe, calculate_theta



class PieChart(Visualization): 

    def __init__(self, dataframe, color_encoding,theta_encoding, keywords=None):
        super().__init__(dataframe, "PieChart", keywords)
        #Class varibales 
        self.color_encoding = {
            "aggregate": None,
            "field": color_encoding["field"],
            "type": color_encoding["type"]
        }
        #specifies the angle
        self.theta_encoding = {
            "aggregate": theta_encoding["aggregate"],
            "field": theta_encoding["field"],
            "type": theta_encoding["type"]
        }
    #Equivalent to set_fields/set_axis function
    def set_fields(self, color_field=None, theta_field=None):
        if not color_field==None:
            self.color_encoding["field"] = color_field
        if not theta_field==None:    
            self.theta_encoding["field"] = theta_field
    #set aggregate of values 
    def set_aggregate(self, theta_aggregate):
        self.theta_encoding["aggregate"] = theta_aggregate
    #return  color-encoding
    def get_color_encoding(self): 
        return self.color_encoding
    #return  theta-encoding
    def get_theta_encoding(self): 
        return self.theta_encoding
    
    #applies all properties/keywords/encodings and returns the data
    #Returns the prepared Data for this visualization. Instead of total Values, it returns angles
    def get_data(self):
        self.dataframe_prepared = prepare_dataframe(self.dataframe,self.color_encoding["field"],self.theta_encoding["field"],self.theta_encoding["aggregate"], self.keywords)
        angleRowName = self.dataframe_prepared.columns.values[0]
        angle_series = calculate_theta(self.dataframe_prepared[angleRowName])
        
        self.dataframe_prepared[self.theta_encoding["field"]] = angle_series.values
        return self.dataframe_prepared
    #returns dictionnary with all information of the object 
    def serialize_object(self): 
        return {"type" : self.type, "keywords" : self.keywords, "color_encoding" : self.color_encoding, "theta_encoding" : self.theta_encoding}