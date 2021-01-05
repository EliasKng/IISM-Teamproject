const fs = require('fs');

const rawdata = fs.readFileSync('output.json');

const obj = JSON.parse(rawdata);

alert(obj.columns);
alert(obj.data);
