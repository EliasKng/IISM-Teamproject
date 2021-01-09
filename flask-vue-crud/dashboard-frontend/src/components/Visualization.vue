<template>
  <div>
    <h1>Vis Bar Chart</h1>
    <p>{{ xAxis }}</p>
    <p>{{ yAxis }}</p>
    <p>{{ columns_output }}</p>
    <D3BarChart :config="barconfig" :datum="bardata"></D3BarChart>
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

export default {
  name: 'Visualization',
  components: {
    D3BarChart,
  },
  data() {
    return {
      columns_output: [],
      xAxis: '',
      yAxis: '',
      bartext: '<D3BarChart :config="chart_config" :datum="chart_data"></D3BarChart>',
      titlechart: '<D3BarChart title="Your custom title"></D3BarChart>',
      sourcechart: '<D3BarChart source="Your custom source"></D3BarChart>',
      barconfig: {
        key: this.xAxis,
        values: [this.yAxis],
        color: { scheme: ['#41B882', '#222f3e'] },
        transition: { ease: 'easeBounceOut', duration: 1000 },
      },
      bardata: [
        { Country: 'Canada', Sales: 50000 },
        { Country: 'France', Sales: 555 },
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
