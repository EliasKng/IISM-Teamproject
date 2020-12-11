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
    if filterkeywords :
        return pd.pivot_table(dataframe.query(" & ".join('{0} {1} {2}'.format(k, cond[0], prepare_string(cond[1])) for k, cond in filterkeywords.items())), index=[filter_index_row], values=[values_row], aggfunc={values_row: aggregate})
    elif aggregate :
        return pd.pivot_table(dataframe, index=[filter_index_row], values=[values_row], aggfunc={values_row: aggregate})
    return dataframe.filter([filter_index_row, values_row])

def calculate_theta(value_series):
    llist = []
    summe = value_series.sum()
    for x in value_series:
        angle = x/summe*360
        llist.append(angle)
    data = np.array(llist)
    return pd.Series(data)




#Test 
#Import CSV Data
working_dataframe = pd.read_csv(os.path.join(".", os.path.dirname(os.path.abspath(__file__)), "examples", "assets", "data", "FinancialSample.csv"))

#FilterFor Country & Sales & Product = Carretera & Sales > 10.000 & Aggregate Sales by "sum"
table_new = prepare_dataframe(working_dataframe, "Country", "Sales", "sum", { "Product": ["==", "Carretera"], "Sales": [">" , "10000"]})
print(table_new.head())

#For scatterplots: Reduce Dataframe to two Rows
table_scatterplot_new = prepare_dataframe(working_dataframe, "Manufacturing Price", "Profit")
print(table_scatterplot_new)

print(type(table_scatterplot_new["Profit"]))

print(calculate_theta(table_new["Sales"]))


