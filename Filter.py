import os
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from QuickParser import getAttributes
from filter_functions import filterDataframe, mask



#table = pd.pivot_table(working_dataframe.query('Product=="Carretera"', inplace = True), index=["Country"], values=["Sales"], aggfunc={"Sales": Aggregation})
#table = pd.pivot_table(working_dataframe[working_dataframe.Product=="Carretera"], index=["Country"], values=["Sales"], aggfunc={"Sales": np.sum})


def filter_dataframe(dataframe, xaxis, yaxis, aggregate, filterkeywords): 
filterquery = ""
    for key, value in filterkeywords: 

    table = pd.pivot_table(dataframe, index=[xaxis], values=[yaxis], aggfunc={yaxis: aggregate})
