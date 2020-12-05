from nl4dv import NL4DV
import os
from jsonPrinter import jsonPrettyPrinter

# Initialize an instance of NL4DV
# ToDo: verify the path to the source data file. modify accordingly.
nl4dv_instance = NL4DV(data_url = os.path.join(".", "examples", "assets", "data", "FinancialSampleAmbiguityTest.csv"))

# using Stanford Core NLP
# ToDo: verify the paths to the jars. modify accordingly.
dependency_parser_config = {"name": "corenlp", "model": os.path.join(".", "examples","assets","jars","stanford-english-corenlp-2018-10-05-models.jar"),"parser": os.path.join(".", "examples","assets","jars","stanford-parser.jar")}

# Set the Dependency Parser
nl4dv_instance.set_dependency_parser(config=dependency_parser_config)

# Define a query
queryInput = "visualize sales by country in a bar chart"
#queryInput = input("Please type in a query: ")
# Execute the query
output = nl4dv_instance.analyze_query(queryInput)
print(output)
vis = nl4dv_instance.render_vis(queryInput)
'''
# Print the output
jsonFile = jsonPrettyPrinter(output, "output.json")
print("You will find the output at output.json")

import json
import altair as alt
alt.Chart.from_json("""{
    "$schema": "https://vega.github.io/schema/vega-lite/v4.json",
    "data": {"url": "/data/cars.json/"},
    "encoding": {
        "x": {
            "field": "Country",
            "type": "nominal"
        },
        "y": {
            "aggregate": "mean",
            "axis": {
                "format": "s"
            },
            "field": "Sales",
            "type": "quantitative"
        }
    },
    "mark": {
        "tooltip": true,
        "type": "bar"
    },
    "transform": []
}"""
        ).save('chart.html')
'''