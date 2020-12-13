import os
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from Visualizations import Visualization
from BarChart import BarChart
from PieChart import PieChart 
from ColumnChart import ColumnChart
from ScatterPlot import ScatterPlot


def vishandler(current_vis, target_vis): 
    keywords = current_vis.keywords

    #currentvis==BarChart 
    if isinstance(current_vis, BarChart) and target_vis=="ColumnChart":
        return ColumnChart(current_vis.dataframe, current_vis.get_y_encoding(), current_vis.get_x_encoding(), keywords)
    elif isinstance(current_vis, BarChart) and target_vis=="PieChart":
        return PieChart(current_vis.dataframe, current_vis.get_x_encoding(), current_vis.get_y_encoding(), keywords)
    #currentvis==ColumnChart 
    if isinstance(current_vis, ColumnChart) and target_vis=="BarChart":
        return BarChart(current_vis.dataframe, current_vis.get_y_encoding(), current_vis.get_x_encoding(), keywords)
    elif isinstance(current_vis, ColumnChart) and target_vis=="PieChart":
        return  BarChart(current_vis.dataframe, current_vis.get_y_encoding(), current_vis.get_x_encoding(), keywords)
    #currentvis==PieChart
    if isinstance(current_vis, PieChart) and target_vis=="BarChart":
        return BarChart(current_vis.dataframe, current_vis.get_color_encoding(), current_vis.get_theta_encoding(), keywords)
    elif isinstance(current_vis, PieChart) and target_vis=="ColumnChart":
        return BarChart(current_vis.dataframe, current_vis.get_theta_encoding(), current_vis.get_color_encoding(), keywords)

