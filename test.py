import subprocess
import shlex
import numpy as np
import matplotlib.path as mpltPath
from shapely.geometry import Polygon, MultiPoint, Point
import pandas as pd

tuple1 = [[0,0], [1280,0], [0, 720], [1280, 720]]
      
#test shapes 
poly1 = MultiPoint(tuple1).convex_hull
poly2 = MultiPoint(((0,1440), (0,720), (1280, 720), (1280, 1440))).convex_hull
poly3 = MultiPoint(((1280,0), (1280,720), (2560, 720), (2560, 0))).convex_hull
poly4 = MultiPoint(((1280,720), (1280,1440), (2560, 1440), (2560, 720))).convex_hull
shapes = [poly1, poly2, poly3, poly4]

print(shapes)

