import json

#prettyprint Dict Filetype to JSON File
def jsonPrettyPrinter(output, filename) :
    #Change Datatype from Dict to JSON
    output = json.dumps(output, sort_keys=True, indent=4)
    #Open JSON-File
    jsonFile = open(filename, 'w')
    #Write Output To File
    jsonFile.write(json.dumps(json.loads(output), indent=4, sort_keys=True))
    return json.dumps(json.loads(output), indent=4, sort_keys=True)