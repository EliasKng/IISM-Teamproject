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

def filter_dataframe(dataframe, filterindex, aggregatevalues, aggregate, filterkeywords): 
    return pd.pivot_table(dataframe.query(" & ".join('{0} {1} {2}'.format(k, cond[0], prepare_string(cond[1])) for k, cond in filterkeywords.items())), index=["Country"], values=["Sales"], aggfunc={"Sales": "sum"})
    #return pd.pivot_table(dataframe.query(" & ".join('{0} {1} {2}'.format(k, cond[0], "\'" + cond[1] + "\'" if dataframe.select_dtypes(include='object').to_dict().get(cond[1]), else: cond[1] ) for k, cond in filterkeywords.items())), index=["Country"], values=["Sales"], aggfunc={"Sales": "sum"})

    #return pd.pivot_table(dataframe, index=[filterindex], values=[aggregatevalues], aggfunc={aggregatevalues: aggregate})

print(prepare_string(x))

#Test 
working_dataframe = pd.read_csv(os.path.join(".", os.path.dirname(os.path.abspath(__file__)), "examples", "assets", "data", "FinancialSample.csv"))
table_new = filter_dataframe(working_dataframe, "Country", "Sales", "sum", { "Product": ["==", "Carretera"], "Sales": [">" , "10000"]})
print(table_new.head())



