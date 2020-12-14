import os
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt




class Visualization(object): 

    def __init__(self, dataframe, keywords=None):
        self.dataframe = dataframe
        self.dataframe_prepared = dataframe
        self.keywords = keywords
        self.columns = list(dataframe.columns)
        self.column_dtypes = dataframe.dtypes
        #Propertiesstatus

    #new keyword query
    def set_keywords(self, keywords):
        self.keywords = keywords

    #ndd Keyword(s) to existing keyword query
    def add_filter(self, additional_filter):
        self.keywords.update(additional_filter) 
    
    #returns columnnames of unfiltered dataframe 
    def get_columnnames(self):
        return self.columns
    
    #returns datatypes of columns of unfiltered dataframe
    def get_column_datatype(self):
        return self.column_dtypes

