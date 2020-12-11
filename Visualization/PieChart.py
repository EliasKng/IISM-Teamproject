import os
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from QuickParser import getAttributes
from Filter import filter_dataframe
from Visualizations import Visualization



class PieChart(Visualization): 
    #override
    def filter(self, xaxis, yaxis, aggregate, filterkeywords): 
        #SetProperties....
        #self.xaxis = xaxis 
        #und so weiter... evt. könnte man noch eine set Properties methode hinzufügen, die hier nur gecalled wird. 
        return self.dataframe-filtered = filterDataframe(self.dataframe, xaxis, yaxis, aggregate, filterkeywords)

    def __init__(self, dataframe):
        super().__init__(self, dataframe)
        #Class varibales with example Parameters
        self.color_encoding = {
            "aggregate": null,
            "field": "Country",
            "type": "nominal"
        }
        #specifies the angle
        self.theta_encoding = {
            "aggregate": "mean",
            "field": "Sales",
            "type": "quantitative"
        }
    
    def get_piechart_data(self):