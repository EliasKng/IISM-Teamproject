import os
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from QuickParser import getAttributes
from Filter import filter_dataframe
from Visualizations import Visualization



class ScatterPlot(Visualization): 

    def __init__(self, dataframe):
        super().__init__(self, dataframe)
        #Class varibales with example Parameters
        self.x_encoding = {
            "aggregate": null,
            "field": "Sale Price",
            "type": "ordinal"
        }
        self.y_encoding = {
            "aggregate": null,
            "field": "Profit",
            "type": "quantitative"
        }
    
    def get_scatterplot_data(self):