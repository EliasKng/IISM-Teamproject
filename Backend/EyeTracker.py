import subprocess
import shlex
import numpy as np
import matplotlib.path as mpltPath
from shapely.geometry import Polygon, MultiPoint, Point
import pandas as pd


#decide whether gaze is within shape
def is_in_polygon(shape, x_coordinate, y_coordinate):
    if shape.intersects(Point(x_coordinate, y_coordinate).buffer(20)): 
        return 1
    else: 
        return 0

#create eyetracker object
class EyeTracker: 
    def __init__(self, list_polygons_raw, list_polygons_name): 
        list_polygons = []
        for i in range(len(list_polygons_raw)):
            list_polygons.append(MultiPoint(list_polygons_raw[i]).convex_hull)
        self.list_polygons = list_polygons
        self.list_polygons_name = list_polygons_name
#start eyetracking
    def execute(self):
        gaze_data_x = []
        gaze_data_y = []
        i=0
        while i<1:
            popen = subprocess.Popen("cpp_stream_data.exe", stdout=subprocess.PIPE)
            lines_iterator = iter(popen.stdout.readline, b"")
            while popen.poll() is None:
                for line in lines_iterator:
                    nline = line.rstrip()
                    nline_decoded = nline.decode('UTF-8')
                    values_from_string = nline_decoded.split()
                    x_coordinate = values_from_string[1]
                    x_coordinate = x_coordinate[:len(x_coordinate)-1]
                    y_coordinate = values_from_string[3]
                    y_coordinate = y_coordinate[:len(y_coordinate)-1]
                    gaze_data_x.append(x_coordinate)
                    gaze_data_y.append(y_coordinate)
            i += 1 
#delete unnecessary informations
        gaze_data_x = [value for value in gaze_data_x if value not in ["interactio", "0", "updat"]]
        gaze_data_y = [value for value in gaze_data_y if value not in ["interactio", "0", "updat"]]
        print(gaze_data_x)
        print(gaze_data_y)
#convert to integer
        gaze_data_x_final = np.array(gaze_data_x,dtype=float).tolist()
        gaze_data_y_final = np.array(gaze_data_y,dtype=float).tolist()
        print(gaze_data_x_final)
        print(gaze_data_y_final)
        df_gaze_data = pd.DataFrame({"x_coordinate" : gaze_data_x_final, "y_coordinate" : gaze_data_y_final})
#calculate the amount of gazes within the shapes 
        results = {}
        for i in range(len(self.list_polygons)):
            new_column_name = self.list_polygons_name[i]
            df_gaze_data[new_column_name] = df_gaze_data.apply(lambda x: is_in_polygon(self.list_polygons[i], x['x_coordinate'],x['y_coordinate']),axis=1)
#calculate the mean per shape and return it
            results.update({new_column_name : (df_gaze_data[new_column_name].mean())*100})
        return results
