<template>
  <div id="app">
    <Home/>
    <div class="grid__item">
      <h1>Scatterplot</h1>
      <scatter-chart :data="dataChart"/>
    </div>
    <div class="grid__item">
        <h1>Axis</h1>
        <svg width="100%" height="500">
          <g transform="translate(50, 50)">
            <y-axis
              :orient="axisY.orient"
              :scale="axisY.scale"
              :range="axisY.range"
              :domain="axisY.domain"
              :height="axisY.height"
              :width="axisY.width"/>
            />
            <x-axis transform="translate(0, 400)"
              :orient="axisX.orient"
              :scale="axisX.scale"
              :range="axisX.range"
              :domain="axisX.domain"
              :height="axisX.height"
              :width="axisX.width"/>
          </g>
        </svg>
      </div>
      <div class="grid__item">
        <h1>Pie Chart</h1>
        <svg width="100%" height="500">
          <pie-chart :data="dataChart" width="400" height="400" x="270" y="270"/>
        </svg>
      </div>
    <scatterplot/>
    <iframe src='https://webchat.botframework.com/embed/VisBot?s=hklRsvlqmDU.LlAinDiX1BF1692uQtyfVBMkEun7xtEe_smE_UvH6N4'  style='min-width: 400px; width: 100%; min-height: 500px;'></iframe>
  </div>
</template>

<script>
import Home from './components/Home.vue'
import scatterplot from './components/Scatterplot/scatterplot.vue'
import PieChart from './components/PieChart.vue'
import XAxis from './components/XAxis.vue'
import YAxis from './components/YAxis.vue'
import { data } from './store';

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
    scatterplot,
    PieChart,
    XAxis,
    YAxis
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
