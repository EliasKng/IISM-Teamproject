import os
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from Filter import prepare_dataframe, calculate_theta
from BarChart import BarChart
from ColumnChart import ColumnChart
from PieChart import PieChart
from ScatterPlot import ScatterPlot
#from VisHandler import change_vistype

#Import CSV Data
#working_dataframe = pd.read_csv(os.path.join(".", os.path.dirname(os.path.abspath(__file__)), "examples", "assets", "data", "FinancialSample.csv"))
working_dataframe = pd.read_excel(os.path.join(".", os.path.dirname(os.path.abspath(__file__)), "examples", "assets", "data", "FinancialSample.xlsx"), engine='openpyxl')

#*****TESTING prepare_dataframe-Method and calculate_theta-Method
#Test 

# #FilterFor Country & Sales & Product = Carretera & Sales > 10.000 & Aggregate Sales by "sum"
# table_new = prepare_dataframe(working_dataframe, "Country", "Sales", "sum", { "Product": ["==", "Carretera"], "Sales": [">" , "10000"]})
# print(table_new.head())

# #For scatterplots: Reduce Dataframe to two Rows
# table_scatterplot_new = prepare_dataframe(working_dataframe, "Manufacturing Price", "Profit")
# print(table_scatterplot_new)

# #For Piecharts: recalculate the Sales slot to fitting angles (sum of anlges = 360)
# table_piechart_new = prepare_dataframe(working_dataframe, "Country", "Sales", "sum", { "Product": ["==", "Carretera"], "Sales": [">" , "10000"]})
# angle_series = calculate_theta(table_piechart_new["Sales"])
# table_piechart_new["Sales"] = angle_series.values
# print(table_piechart_new)





#********Testing Visualization Obejcts (bacrchat...)

keywords = {"Product" : ["==", "Carretera"]}
x_encoding = {"field": "Country", "type": "nominal"}
y_encoding = {"aggregate": "mean", "field": "Sales", "type": "quantitative"}


#pd.set_option("display.max_rows", None, "display.max_columns", None)
#print(working_dataframe["Sales"].to_string())




print(pd.pivot_table(working_dataframe, index='Country', aggfunc='sum'))



'''
# BarChart
b1 = BarChart(working_dataframe, x_encoding, y_encoding, keywords)
b1.set_fields()
print(type(b1))
print(b1.get_data())
#print(b1.normalize_values("sum"))
#b1 = change_vistype(b1, "PieChart")
#print(b1.get_data())
#print(type(b1))



# ColumnChart
c1 = ColumnChart(working_dataframe, y_encoding, x_encoding)
print(c1.get_data())
print(c1.normalize_values("sum"))


# #PieChart      
color_encoding = {"aggregate": None, "field": "Country", "type": "nominal"}
theta_encoding = {"aggregate": "mean", "field": "Sales", "type": "quantitative"}
p1 = PieChart(working_dataframe, color_encoding, theta_encoding, keywords)
print(p1.get_data())
converted_p1 = change_vistype(p1,"BarChart")
#print(converted_p1.get_data())
#print(type(converted_p1))

# #Scatterplot
scatterplot_x_encoding = {"aggregate": None,"field": "Sale Price","type": "ordinal"}
scatterplot_y_encoding = {"aggregate": None, "field": "Profit", "type": "quantitative"}  
s1 = ScatterPlot(working_dataframe, scatterplot_x_encoding, scatterplot_y_encoding, keywords)
print(s1.get_data().head(50))
'''