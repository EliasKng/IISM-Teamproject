import os
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from nl4dv_parser import nl4dv_output_parser
from nl4dv_test import nl4dvQueryAnalyzerFinancialsDataset
from filter_functions import filterDataframe, mask
from Visualizations import Visualization, Barchart, ScatterPlot, PieChart, ColumnChart#


x = "Show me sales by country in a pie chart"

specification = nl4dv_output_parser(nl4dvQueryAnalyzerFinancialsDataset(x))

working_dataframe = pd.read_csv(os.path.join(".", os.path.dirname(os.path.abspath(__file__)), "examples", "assets", "data", "FinancialSample.csv"))

print(specification)

