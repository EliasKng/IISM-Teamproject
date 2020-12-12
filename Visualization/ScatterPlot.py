import os
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from Visualizations import Visualization
from Filter import prepare_dataframe, calculate_theta



class ScatterPlot(Visualization): 

    def __init__(self, dataframe):
        super().__init__(dataframe)
        #Class varibales with example Parameters
        self.x_encoding = {
            "aggregate": None,
            "field": "Sale Price",
            "type": "ordinal"
        }
        self.y_encoding = {
            "aggregate": None,
            "field": "Profit",
            "type": "quantitative"
        }
    
    def set_axis(self, xaxis=None, yaxis=None):
        self.x_encoding["field"] = xaxis
        self.y_encoding["field"] = yaxis

    #Returns the prepared Data for this visualization
    def get_data(self):
        self.dataframe_prepared = prepare_dataframe(self.dataframe,self.x_encoding["field"],self.y_encoding["field"],self.y_encoding["aggregate"])
        return self.dataframe_prepared