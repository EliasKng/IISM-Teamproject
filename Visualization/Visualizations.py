import os
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt


def calculate_percentage(val, total):
    #Calculates the percentage of a value over a total
    percent = np.divide(val, total)

    return percent


class Visualization(object): 

    def __init__(self, dataframe):
        self.dataframe = dataframe
        self.dataframe_prepared = dataframe
        self.columns = list(dataframe.columns)
        self.column_dtypes = dataframe.dtypes
        #Propertiesstatus
        

    
    # def get_column_datatype(self):

    # def get_properties(self): 

    # def change_aggregation(self):

    # def normalize_Values(self):