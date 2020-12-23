from nl4dv import NL4DV
import os
from jsonPrinter import jsonPrettyPrinter
import os
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from nl4dv_parser import nl4dv_output_parser





def nl4dvQueryAnalyzerFinancialsDataset(query) :
    full_path = os.path.abspath(__file__)
    nl4dv_instance = NL4DV(data_url = os.path.join(".", os.path.dirname(os.path.abspath(__file__)), "examples", "assets", "data", "FinancialSample.csv"))
    dependency_parser_config = {"name": "corenlp", "model": os.path.join(".", os.path.dirname(os.path.abspath(__file__)), "examples","assets","jars","stanford-english-corenlp-2018-10-05-models.jar"),"parser": os.path.join(".", os.path.dirname(os.path.abspath(__file__)), "examples","assets","jars","stanford-parser.jar")}
    nl4dv_instance.set_dependency_parser(config=dependency_parser_config)
    queryInput = query
    return nl4dv_instance.analyze_query(queryInput)



results = nl4dvQueryAnalyzerFinancialsDataset("show me a distribution of sales")
print(nl4dv_output_parser(results))
#results_dic = dict(results)
#print((results_dic["query_raw"]))