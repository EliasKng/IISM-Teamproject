from nl4dv_test import nl4dvQueryAnalyzerFinancialsDataset
import ast


def getAttributes():
    #tkinter._test()
    #nl4dvOutput = nl4dvQueryAnalyzerFinancialsDataset("test")
    nl4dvOutput = nl4dvQueryAnalyzerFinancialsDataset("Show me sales by country in a bar chart")


    #Get x and y-Axis out of the NL4DV-Output-Attribute-Map (the attribues-Variable (see below) contains the same, but extracted out of the VisList)
    xAxis = list(nl4dvOutput["attributeMap"].keys())[0]
    yAxis = list(nl4dvOutput["attributeMap"].keys())[1]


    #The AttributeMap or Taskmap does not contain the VIsualization Type. So we have to convert the Vislist into a dict and then extract the needed Information

    #Convert visList to a dict
    #Removing the first and last char of the string and then convert it do a dictionary
    visString = str(nl4dvOutput["visList"])
    visListDict = ast.literal_eval(visString[1:-1]) 

    print(visListDict)
    #Extracting the attributes (y Axis and x-Axis out of the VisListDictionary)
    attributes = visListDict["attributes"]


    #Extract Visualization-Type out of NL4DV Output
    visType = visListDict["visType"]
    return attributes