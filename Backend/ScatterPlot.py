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

    def get_scatterplot_data(self):
