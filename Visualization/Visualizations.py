import os
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from QuickParser import getAttributes
from Filter import filter_dataframe

    def calculate_percentage(val, total):
        #Calculates the percentage of a value over a total
        percent = np.divide(val, total)

        return percent


class Visualization(object): 

    def __init__(self, dataframe ):
        self.dataframe = dataframe
        self.dataframe-filtered = []
        #Properties
        self.xaxis= ""
        self.yaxis= ""
        self.aggregate= ""
        self.columnnames= []

    def filter(self, xaxis, yaxis, aggregate, filterkeywords): 
        #SetProperties....
        #self.xaxis = xaxis 
        #und so weiter... evt. könnte man noch eine set Properties methode hinzufügen, die hier nur gecalled wird. 
        return self.dataframe-filtered = filter_dataframe(self.dataframe, xaxis, yaxis, aggregate, filterkeywords)

    def get_column_datatype(self):

    def get_properties(self): 

    def change_aggregation(self):
    
    def select_values(self):
    
        '''def filterDataframe(df, column): 
    filter_list = []
    filtered_table = []
    for index, row in df.iterrows():
        val = input("Would you like to filter for " + row[column] + "? Answer with y or n. Multiple filter in" + column + " are possible.")
        if (val == "y"):
            filter_list.append(row[column])
    for element in filter_list:
        temp_dataframe = mask(df, column, element)
        filtered_table.append(temp_dataframe)
    return filtered_table

def mask(df, key, value):
    return df[df[key] == value]'''

    def normalize_Values(self):
