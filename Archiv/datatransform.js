/* eslint-disable */
let s = '';
var ChartData = '[';
const Origindata = {
  "data": [
      [
          "Canada",
          66.8999622162
      ],
      [
          "France",
          151.1410272044
      ],
      [
          "Germany",
          40.2477755585
      ],
      [
          "Mexico",
          25.8672198564
      ],
      [
          "United States of America",
          75.8440151646
      ]
  ],
  "type": "PieChart",
  "columns": {
      "color": "Country",
      "theta": "Sales"
  }
}

for (let i = 0; i < Origindata.data.length; i += 1) {
  if(i != Origindata.data.length - 1){
    s = ('{ \"name\": \"' + Origindata.data[i][0] + '\", \"total\": ' + Origindata.data[i][1] + ' }, ');
  }
  else{
  s = ('{ \"name\": \"' + Origindata.data[i][0] + '\", \"total\": ' + Origindata.data[i][1] + ' }]');
  }
  // eslint-disable-next-line
  ChartData += s;
  console.log(s)
}

/* eslint-disable import/prefer-default-export */
export const ex = [ 
  ChartData
];

/* eslint-disable import/prefer-default-export */
export const ex2 = [
  'Barcharsdfafsdfsdt',
];

