<template>
  <div>
    <h1>Vis Bar Chart</h1>
    <D3BarChart :config="barconfig" :datum="bardata"></D3BarChart>
    <d-3-barchart-horizontal/>
    <p>{{ xAxis }}</p>
    <p>{{ yAxis }}</p>
    <p>{{ columns_output }}</p>
    <p>{{ type_output }}</p>
    <tbody>
        <tr v-for="(x, index) in data_output" :key="index">
            <td>{{ x }}</td>
        </tr>
    </tbody>
    <div class="grid__item">
    <h1>Scatter Chart</h1>
    <scatter-chart :data="dataChart" x="270" y="270"/>
    </div>
  </div>
</template>

<script>
import axios from 'axios';
import { D3BarChart } from 'vue-d3-charts';
import ScatterChart from './ScatterChart.vue';
import D3BarchartHorizontal from './Columnchart/d3-barchart-horizontal.vue';
/*
function setBarChartConfig(xAxis, yAxis) {
  return {
    key: xAxis,
    values: [yAxis],
    color: { scheme: ['#41B882', '#222f3e'] },
    transition: { ease: 'easeBounceOut', duration: 1000 },
  };
} */

// [["Canada", 2894762.65],["France",6539875.15]]
/* function setBarChartData() {
  const z = [];
  for (let i = 0; i < this.data_output.length; i += 1) {
    const x = { Country: 'Canada', Sales: 12425 };
    z.push(x);
  }
  return z;
} */

export default {
  name: 'Visualization',
  components: {
    D3BarChart,
    ScatterChart,
    D3BarchartHorizontal,
  },
  methods: {
    getData() {
      const path = 'http://localhost:5000/data';
      axios.get(path)
        .then((res) => {
        // get return from app.py
          this.columns_output = res.data.columns_output;
          const [x, y] = res.data.columns_output;
          this.xAxis = x;
          this.yAxis = y;
          this.data_output = res.data.data_output;
          this.type_output = res.data.type_output;
        })
        .catch((error) => {
          // eslint-disable-next-line
        console.error(error);
        });
    },
  },
  created() {
    this.getData();
  },
  data() {
    let result;
    if (this.type_output === 'BarChart') {
      result = {
        columns_output: this.columns_output,
        xAxis: this.xAxis,
        yAxis: this.yAxis,
        type_output: this.type_output,
        bartext: '<D3BarChart :config="chart_config" :datum="chart_data"></D3BarChart>',
        titlechart: '<D3BarChart title="Your custom title"></D3BarChart>',
        sourcechart: '<D3BarChart source="Your custom source"></D3BarChart>',
        /*       barconfig: setBarChartConfig('Country', 'Sales'),
        bardata: setBarChartData(), */
        // hardcoding works
        barconfig: {
          key: 'Country',
          values: ['Sales'],
          color: { scheme: ['#41B882', '#222f3e'] },
          transition: { ease: 'easeBounceOut', duration: 1000 },
        },
        // [["Canada", 2894762.65],["France",6539875.15]]
        bardata: [
          { Country: 'Canada', Sales: 2894762.65 },
          { Country: 'France', Sales: 6539875.15 },
        ],
      };
    } else if (this.type_output === 'ColumnChart') {
      result = {
        columns_output: this.columns_output,
        xAxis: this.xAxis,
        yAxis: this.yAxis,
        type_output: this.type_output,
        chart_data: [
          { vue: 20, d3: 62, category: 'lorem' },
          { vue: 28, d3: 47, category: 'ipsum' },
          { vue: 35, d3: 36, category: 'dolor' },
          { vue: 60, d3: 24, category: 'sit' },
          { vue: 65, d3: 18, category: 'amet' },
        ],
        chart_config: {
          key: 'category',
          values: ['vue', 'd3'],
          orientation: 'horizontal',
          color: {
            keys: {
              vue: '#41b882',
              d3: '#b35899',
            },
          },
        },
      };
    } else {
      result = {
        columns_output: this.columns_output,
        xAxis: this.xAxis,
        yAxis: this.yAxis,
        type_output: this.type_output,
        bartext: '<D3BarChart :config="chart_config" :datum="chart_data"></D3BarChart>',
        titlechart: '<D3BarChart title="Your custom title"></D3BarChart>',
        sourcechart: '<D3BarChart source="Your custom source"></D3BarChart>',
        /*       barconfig: setBarChartConfig('Country', 'Sales'),
        bardata: setBarChartData(), */
        // hardcoding works
        barconfig: {
          key: 'Country',
          values: ['Sales'],
          color: { scheme: ['#41B882', '#222f3e'] },
          transition: { ease: 'easeBounceOut', duration: 1000 },
        },
        // [["Canada", 2894762.65],["France",6539875.15]]
        bardata: [
          { Country: 'test', Sales: 0 },
          { Country: 'nichts', Sales: 0 },
        ],
      };
    }
    return result;
  },
};
</script>

<style>
.chart {
  width: 100%;
  height: 100%;
}
</style>
