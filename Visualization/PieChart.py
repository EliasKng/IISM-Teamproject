import os
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from Visualizations import Visualization
from Filter import prepare_dataframe, calculate_theta



class PieChart(Visualization): 

    def __init__(self, dataframe):
        super().__init__(dataframe)
        #Class varibales with example Parameters
        self.color_encoding = {
            "aggregate": None,
            "field": "Country",
            "type": "nominal"
        }
        #specifies the angle
        self.theta_encoding = {
            "aggregate": "mean",
            "field": "Sales",
            "type": "quantitative"
        }
    
    def get_data(self):
        self.dataframe_prepared = prepare_dataframe(self.dataframe,self.color_encoding["field"],self.theta_encoding["field"],self.theta_encoding["aggregate"])
        angleRowName = self.dataframe_prepared.columns.values[0]
        angle_series = calculate_theta(self.dataframe_prepared[angleRowName])
        
        self.dataframe_prepared["Sales"] = angle_series.values
        return self.dataframe_prepared