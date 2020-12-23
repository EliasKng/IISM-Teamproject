import os
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from nl4dv_parser import nl4dv_output_parser






'''#loading csv
working_dataframe = pd.read_csv(os.path.join(".", os.path.dirname(os.path.abspath(__file__)), "examples", "assets", "data", "FinancialSample.csv"))
print(working_dataframe.head(20))

#pivot table for visualization
#table = pd.pivot_table(working_dataframe, index=["Country"], values=["Sales"], aggfunc={"Sales": np.sum})
#Query with filter. Shows Sales per Country for Product Carretera

Aggregation = "sum"
Query = "Carretera"


table = pd.pivot_table(working_dataframe.query('Product=="Carretera"', inplace = True), index=["Country"], values=["Sales"], aggfunc={"Sales": Aggregation})
'''

#Integrating Output Nl4DV
attributes = getAttributes()


'''
#table = pd.pivot_table(working_dataframe, index=[attributes[1]], values=[attributes[0]], aggfunc={attributes[0]: np.sum})


#Output
print(table.head())
table.plot(kind='bar')
plt.show()




#Option to select values on x axis
table_new = table.reset_index()
print(table_new.head())
table_new.plot(kind='bar')
plt.show()
table_filtered = filterDataframe(table_new,"Country")
print(table_filtered)



'''
