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
    
    def get_piechart_data(self):