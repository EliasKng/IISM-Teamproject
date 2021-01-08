<template>
  <div id="app">
    <Home/>
    <h1>Horizontal bar chart</h1>
    <d-3-barchart-horizontal/>
    <div class="grid__item">
      <h1>Scatter Chart</h1>
      <scatter-chart :data="dataChart" x="270" y="270"/>
    </div>
    <div class="grid__item">
      <h1>Pie Chart</h1>
      <svg width="100%" height="500">
        <pie-chart :data="dataChart" width="400" height="400" x="270" y="270"/>
      </svg>
    </div>
    <iframe src='https://webchat.botframework.com/embed/VisBot?s=hklRsvlqmDU.LlAinDiX1BF1692uQtyfVBMkEun7xtEe_smE_UvH6N4'  style='min-width: 400px; width: 100%; min-height: 500px;'></iframe>
  </div>
</template>

<script>
import Home from './components/Home.vue'
import ScatterChart from './components/ScatterChart.vue'
import PieChart from './components/PieChart.vue'
import { data } from './store';
import D3BarchartHorizontal from './components/Columnchart/d3-barchart-horizontal.vue';

export default {
  name: 'App',
  data() {
    return {
      dataChart: data,
      axisX: {
        scale: 'scaleLinear',
        range: [0, 400],
        domain: [0, 10],
        orient: 'Bottom',
        width: 400,
        height: 400
      },
      axisY: {
        scale: 'scaleLinear',
        range: [0, 400],
        domain: [500, 0],
        orient: 'Left',
        width: 400,
        height: 400
      }
    };
  },
  components: {
    Home,
    ScatterChart,
    PieChart,
    D3BarchartHorizontal
  }
}
</script>

<style>
body {
  margin: 0;
}

.app {
  margin-top: 20px;
  max-width: 1020px;
  margin: 0 auto;
}

.grid {
  display: grid;
  grid-template-columns: repeat(2, 500px);
  grid-gap: 20px;
}
</style>
