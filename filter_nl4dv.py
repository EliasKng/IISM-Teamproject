import os
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from QuickParser import getAttributes
from filter_functions import filterDataframe, mask


attributes = getAttributes()

working_dataframe = pd.read_csv(os.path.join(".", os.path.dirname(os.path.abspath(__file__)), "examples", "assets", "data", "FinancialSample.csv"))
print(working_dataframe.head(20))

#table = pd.pivot_table(working_dataframe, index=["Country"], values=["Sale Price"], aggfunc={"Sale Price": np.sum})
table = pd.pivot_table(working_dataframe, index=[attributes[1]], values=[attributes[0]], aggfunc={attributes[0]: np.sum})

table_new = table.reset_index()


print(table_new.head())
table_new.plot(kind='bar')
plt.show()

table_filtered = filterDataframe(table_new,"Country")
print(table_filtered)





