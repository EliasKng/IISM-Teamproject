import os
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from Visualizations import Visualization
from Filter import prepare_dataframe



class ColumnChart(Visualization): 

    def __init__(self, dataframe):
        super().__init__(dataframe)

        #Class varibales with example Parameters
        self.x_encoding = {
            "aggregate": None,
            "field": "Country",
            "type": "nominal"
        }
        self.y_encoding = {
            "aggregate": "mean",
            "field": "Sales",
            "type": "quantitative"
        }
    
    #def change_axis():
    
    def get_data(self):
        self.dataframe_prepared = prepare_dataframe(self.dataframe,self.x_encoding["field"],self.y_encoding["field"],self.y_encoding["aggregate"])
        return self.dataframe_prepared