<template>
  <div>
    <h1>Vis Bar Chart</h1>
    <D3BarChart :config="barconfig" :datum="bardata"></D3BarChart>
    <p>{{ xAxis }}</p>
    <p>{{ yAxis }}</p>
    <p>{{ columns_output }}</p>
    <tbody>
        <tr v-for="(x, index) in data_output" :key="index">
            <td>{{ x }}</td>
        </tr>
    </tbody>
  </div>
</template>

<script>
import axios from 'axios';
import { D3BarChart } from 'vue-d3-charts';
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
  },
  data() {
    /* return {
      bartext: '<D3BarChart :config="chart_config" :datum="chart_data"></D3BarChart>',
      titlechart: '<D3BarChart title="Your custom title"></D3BarChart>',
      sourcechart: '<D3BarChart source="Your custom source"></D3BarChart>',
      barconfig: {
        key: 'name',
        values: ['total', 'balance'],
        color: { scheme: ['#41B882', '#222f3e'] },
        transition: { ease: 'easeBounceOut', duration: 1000 },
      },
      bardata: [
        { name: '1992', total: 50000, balance: 748 },
        { name: '1993', total: 555, balance: 5666 },
        { name: '1994', total: 8574, balance: 574 },
        { name: '1995', total: 15805, balance: 5805 },
        { name: '1996', total: 14582, balance: 4582 },
        { name: '1997', total: 26694, balance: 6694 },
        { name: '1998', total: 35205, balance: 5205 },
        { name: '1999', total: 45944, balance: 5944 },
        { name: '2000', total: 78595, balance: 8595 },
      ],
    }; */
    return {
      columns_output: [],
      xAxis: '',
      yAxis: '',
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
};
</script>

<style>
.chart {
  width: 100%;
  height: 100%;
}
</style>
