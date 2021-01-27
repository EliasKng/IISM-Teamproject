import os
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt


#überprüft, ob es sich um ein String oder int/float handelt. Gibt True bei String zurück. 
def prepare_string(xstr):
    #xstr = xstr.replace(',','.')
    try:
        int(xstr)
        return False
    except:
        try:
            float(xstr)
            return False
        except:
            return True


#Entfernt Klammern bei Integer Keywords 
def final_prep(input_list):
    if not prepare_string(input_list[0]):
        return int(input_list[0])
    else: 
        return str(input_list)

#Takes a dataframe and filters for filterkeywords & aggregates by aggretae parameter (e.g. SUM)
def prepare_dataframe(dataframe, filter_index_row, values_row, aggregate=None, filterkeywords=None): 
    if aggregate!=("Null" or "null"):
        if filterkeywords:
            if filter_index_row == values_row:
                df = dataframe.query(" & ".join('{0} {1} {2}'.format("`" + k + "`", cond[0], (final_prep(cond[1]))) for k, cond in filterkeywords.items()))
                return df[filter_index_row].value_counts().rename_axis(filter_index_row).reset_index(name='Counts')
            else:     
                return pd.pivot_table(dataframe.query(" & ".join('{0} {1} {2}'.format("`" + k + "`", cond[0], (final_prep(cond[1]))) for k, cond in filterkeywords.items())), index=[filter_index_row], values=[values_row], aggfunc={values_row: aggregate})

        else: 
            if filter_index_row == values_row:
                return dataframe[filter_index_row].value_counts().rename_axis(filter_index_row).reset_index(name='Counts')
            else:     
                return pd.pivot_table(dataframe, index=[filter_index_row], values=[values_row], aggfunc={values_row: aggregate})
    else:
        if filterkeywords: 
            return dataframe.query(" & ".join('{0} {1} {2}'.format("`" + k + "`", cond[0], (final_prep(cond[1]))) for k, cond in filterkeywords.items())).filter([filter_index_row, values_row])
        else: 
            return dataframe.filter([filter_index_row, values_row])


#needed for piechart
def calculate_theta(value_series):
    angle_list = []
    summe = value_series.sum()
    for x in value_series:
        angle = x/summe*360
        angle_list.append(angle)
    data = np.array(angle_list)
    return pd.Series(data)


