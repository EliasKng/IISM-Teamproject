import ast
def nl4dv_output_parser(nl4dv_output):
    output = nl4dv_output
    vega_spec =  ast.literal_eval(str(output["visList"])[1:-1])
    attributes = vega_spec["attributes"]
    vis_type = vega_spec["visType"]
    
    specification = {
        "attributes" : attributes,
        "vis_type" : vis_type
    }

    return specification