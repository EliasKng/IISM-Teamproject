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
def prepare_dataframe(dataframe, filterindex, aggregatevalues, aggregate, filterkeywords): 
    if filterkeywords :
        return pd.pivot_table(dataframe.query(" & ".join('{0} {1} {2}'.format(k, cond[0], prepare_string(cond[1])) for k, cond in filterkeywords.items())), index=[filterindex], values=[aggregatevalues], aggfunc={aggregatevalues: aggregate})
    return pd.pivot_table(dataframe, index=[filterindex], values=[aggregatevalues], aggfunc={aggregatevalues: aggregate})

#Test 
working_dataframe = pd.read_csv(os.path.join(".", os.path.dirname(os.path.abspath(__file__)), "examples", "assets", "data", "FinancialSample.csv"))
table_new = prepare_dataframe(working_dataframe, "Country", "Sales", "sum", { "Product": ["==", "Carretera"], "Sales": [">" , "10000"]})
print(table_new.head())



