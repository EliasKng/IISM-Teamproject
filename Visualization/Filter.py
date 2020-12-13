import os
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt


#Fügt ein \' for und nach den string, falls dieser keine Zahl ist
def prepare_string(xstr):
    xstr = xstr.replace(',','.')
    try:
        int(xstr)
        return xstr
    except:
        try:
            float(xstr)
            return xstr
        except:
            return "\'"+xstr+"\'"

#Takes a dataframe and filters for filterkeywords & aggregates by aggretae parameter (e.g. SUM)
def prepare_dataframe(dataframe, filter_index_row, values_row, aggregate=None, filterkeywords=None): 
    if aggregate:
        if filterkeywords: 
            return pd.pivot_table(dataframe.query(" & ".join('{0} {1} {2}'.format(k, cond[0], prepare_string(cond[1])) for k, cond in filterkeywords.items())), index=[filter_index_row], values=[values_row], aggfunc={values_row: aggregate})
        else: 
            return pd.pivot_table(dataframe, index=[filter_index_row], values=[values_row], aggfunc={values_row: aggregate})
    else:
        if filterkeywords: 
            return dataframe.query(" & ".join('{0} {1} {2}'.format(k, cond[0], prepare_string(cond[1])) for k, cond in filterkeywords.items())).filter([filter_index_row, values_row])
        else: 
            return dataframe.filter([filter_index_row, values_row])



'''
def prepare_dataframe(dataframe, filter_index_row, values_row, aggregate=None, filterkeywords=None): 
    if filterkeywords:
        return pd.pivot_table(dataframe.query(" & ".join('{0} {1} {2}'.format(k, cond[0], prepare_string(cond[1])) for k, cond in filterkeywords.items())), index=[filter_index_row], values=[values_row], aggfunc={values_row: aggregate})
    elif aggregate:
        return pd.pivot_table(dataframe, index=[filter_index_row], values=[values_row], aggfunc={values_row: aggregate})
    return dataframe.filter([filter_index_row, values_row])
'''



def calculate_theta(value_series):
    angle_list = []
    summe = value_series.sum()
    for x in value_series:
        angle = x/summe*360
        angle_list.append(angle)
    data = np.array(angle_list)
    return pd.Series(data)