import ast
import json

def nl4dv_output_parser(nl4dv_output):
    output = nl4dv_output

    #Remove the "[" & "]" Brackets around the Dictionary-String from the Vega-Lite Spec
    #Convert the String to a dictionary
    #vega_spec =  json.loads(str(output["visList"])[1:-1])
    
    #Approach doesnt work
    #vega_spec =  ast.literal_eval(str(output["visList"])[1:-1])
    vega_spec =  dict(output)
    print(vega_spec)
    #Read the Attributes out of the vegaspec (is a Dictionary)
    #attributes = vega_spec["attributes"]

    #Extraxt the visualization-type
    vis_type = vega_spec["visType"]

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

    print(specification)

    #Return Dictionary
    return specification