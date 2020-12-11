import os
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt



def filter_dataframe(dataframe, filterindex, aggregatevalues, aggregate, filterkeywords): 
    
    if  filterkeywords: 
     ''' for k, v in filterkeywords.items():
            if v[1] is dataframe.select_dtypes(include='object').to_dict():
                v[1] = "\'" + v[1] + "\'" '''
    return pd.pivot_table(dataframe.query(" & ".join('{0} {1} {2}'.format(k, cond[0], cond[1]) for k, cond in filterkeywords.items())), index=["Country"], values=["Sales"], aggfunc={"Sales": "sum"})

    return pd.pivot_table(dataframe, index=[filterindex], values=[aggregatevalues], aggfunc={aggregatevalues: aggregate})


working_dataframe = pd.read_csv(os.path.join(".", os.path.dirname(os.path.abspath(__file__)), "examples", "assets", "data", "FinancialSample.csv"))
table_new = filter_dataframe(working_dataframe, "Country", "Sales", "mean", { "Product": ["==", "\'Carretera\'"], "Sales" : [">", "10000"], "Country": ["==", "\'Canada\'"]})


print(table_new.head())





