import os
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from Filter import prepare_dataframe, calculate_theta
from BarChart import Barchart
from ColumnChart import ColumnChart
from PieChart import PieChart
from ScatterPlot import ScatterPlot

#Import CSV Data
working_dataframe = pd.read_csv(os.path.join(".", os.path.dirname(os.path.abspath(__file__)), "examples", "assets", "data", "FinancialSample.csv"))

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

# BarChart
b1 = Barchart(working_dataframe)
print(b1.get_data())
print(b1.normalize_values("sum"))

# ColumnChart
c1 = ColumnChart(working_dataframe)
print(c1.get_data())
print(c1.normalize_values("sum"))

# #PieChart
# p1 = PieChart(working_dataframe)
# print(p1.get_data())

# #Scatterplot
# s1 = ScatterPlot(working_dataframe)
# print(s1.get_data())