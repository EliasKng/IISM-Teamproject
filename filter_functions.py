




def filterDataframe(df, column): 
    filter_list = []
    filtered_table = []
    for index, row in df.iterrows():
        val = input("Would you like to filter for " + row[column] + "? Answer with y or n. Multiple filter in" + column + " are possible.")
        if (val == "y"):
            filter_list.append(row[column])
    for element in filter_list:
        temp_dataframe = mask(df, column, element)
        filtered_table.append(temp_dataframe)
    return filtered_table

def mask(df, key, value):
    return df[df[key] == value]