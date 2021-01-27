import ast
import json





def nl4dv_output_parser(nl4dv_output):
    
    output = nl4dv_output
    vega_spec =  output["visList"][0]

    #Read the Attributes out of the vegaspec (is a Dictionary)
    attributes = vega_spec["attributes"]
    #Extraxt the visualization-type
    #vis_type = vega_spec["visType"]
    vis_type = vega_spec["vlSpec"]["mark"]["type"]
    #Extract the Encoding: includes (Barchart) Axis, Aggregate Functions, Field Information (also in "attributes") and variable type (e.g. nominal, quantitative)
    # Or (Pie Chart)  [Color: field, type] and [theta (angle): aggregate, field, type]
    #It depends on the VisType, which information is shown
    axisEncoding = vega_spec["vlSpec"]["encoding"]
    
    #Wrap extracted Information into a new Dictionary
    specification = {
        "attributes" : attributes,
        "vis_type" : vis_type,
        "encoding" : axisEncoding
    }

    #print(specification)

    #Return Dictionary
    return specification