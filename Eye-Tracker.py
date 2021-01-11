import subprocess
import shlex
import numpy as np
import matplotlib.path as mpltPath
from shapely.geometry import Polygon, MultiPoint, Point
import pandas as pd



'''test shapes 
    poly1 = MultiPoint(((0,0), (1280,0), (0, 720), (1280, 720))).convex_hull
    poly2 = MultiPoint(((0,1440), (0,720), (1280, 720), (1280, 1440))).convex_hull
    poly3 = MultiPoint(((1280,0), (1280,720), (2560, 720), (2560, 0))).convex_hull
    poly4 = MultiPoint(((1280,720), (1280,1440), (2560, 1440), (2560, 720))).convex_hull
    shapes = [poly1, poly2, poly3, poly4]'''


def is_in_polygon(shape, x_coordinate, y_coordinate):
    if shape.intersects(Point(x_coordinate, y_coordinate).buffer(50)): 
        return 1
    else: 
        return 0


class Eye_Tracker: 
    def __init__(self, list_polygons_raw): 
        for i in range(len(list_polygons_raw)):
            list_polygons[i] = MultiPoint(list_polygons_raw[i]).convex_hull
        self.list_polygons = list_polygons


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
            gaze_data_x.remove('interactio')
            gaze_data_x.remove('0')
            gaze_data_y.remove('0')
            gaze_data_y.remove('updat')
            i += 1 

        gaze_data_x_final = np.array(gaze_data_x,dtype=float).tolist()
        gaze_data_y_final = np.array(gaze_data_y,dtype=float).tolist()
        df_gaze_data = pd.DataFrame({"x_coordinate" : gaze_data_x_final, "y_coordinate" : gaze_data_y_final})


        results = {}
        for i in range(len(shapes)):
            new_column_name = "shape_" + str(i)
            df_gaze_data[new_column_name] = df_gaze_data.apply(lambda x: is_in_polygon(shapes[i], x['x_coordinate'],x['y_coordinate']),axis=1)
            results.update({new_column_name : df_gaze_data[new_column_name].mean()})
        print(df_gaze_data.head(200))
        print(results)
        return results
