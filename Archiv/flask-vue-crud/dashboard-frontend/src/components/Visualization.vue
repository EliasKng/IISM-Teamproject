<template>
  <div>





    <!--
    <h1>Vis Bar Chart</h1>
    <p>{{ charttype }}</p>
    <D3BarChart :config="barconfig" :datum="bardata"></D3BarChart>
    <d-3-barchart-horizontal/>
    <p>{{ charttype }}</p>
    <p>{{ xAxis }}</p>
    <p>{{ yAxis }}</p>
    <p>{{ columns_output }}</p>
    <p>{{ type_output }}</p>
    <tbody>
        <tr v-for="(x, index) in data_output" :key="index">
            <td>{{ x }}</td>
        </tr>
    </tbody>
    -->
  </div>
</template>

<script>
import axios from 'axios';
import { D3BarChart } from 'vue-d3-charts';
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
  data() {
    return {
      charttype: '',
    };
  },
  components: {
    D3BarChart,
    D3BarchartHorizontal,
  },
  methods: {
    getData() {
      const path = 'http://localhost:5000/data';
      axios.get(path)
        .then((res) => {

          
        // get return from app.py
          /* this.columns_output = res.data.columns_output;
          const [x, y] = res.data.columns_output;
          this.xAxis = x;
          this.yAxis = y;
          this.data_output = res.data.data_output;
          this.type_output = res.data.type_output; */
          this.charttype = res.data.type;
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
