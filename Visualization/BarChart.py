import os
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from QuickParser import getAttributes
from Filter import filter_dataframe
from Visualizations import Visualization




class Barchart(Visualization): 

    def __init__(self, dataframe):
        super().__init__(self, dataframe)
        #Class varibales with example Parameters
        self.x_encoding = {
            "aggregate": null,
            "field": "Country",
            "type": "nominal"
        }
        self.y_encoding = {
            "aggregate": "mean",
            "field": "Sales",
            "type": "quantitative"
        }
        
    def change_axis(self):

    def get_barchart_data(self):
        return JSONdata