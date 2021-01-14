<template>
    <div id="app">
        <div class="grid__item">
          <h1>Scatter Chart</h1>
          <scatter-chart :data="dataChart" v-if="showScatterChart"/>
        </div>
        <div class="grid__item">
          <h1>Bar Chart</h1>
          <bar-chart :data="dataChart" width="500" height="500" v-if="showBarchart"/>
        </div>
        <div id="bot">
            <iframe src='https://webchat.botframework.com/embed/VisBot?s=hklRsvlqmDU.LlAinDiX1BF1692uQtyfVBMkEun7xtEe_smE_UvH6N4'  style='width: 100%; height: 100%;'></iframe>
        </div>
    </div>
</template>

<script>
import ScatterChart from './components/ScatterChart.vue';
import BarChart from './components/BarChart.vue';
import { data } from './components/data';

export default {
  name: 'app',
  data() {
    return {
      showBarchart: false,
      showScatterchart: true,
      dataChart: data,
      axisX: {
        scale: 'scaleLinear',
        range: [0, 400],
        domain: [0, 10],
        orient: 'Bottom',
        width: 400,
        height: 400,
      },
      axisY: {
        scale: 'scaleLinear',
        range: [0, 400],
        domain: [500, 0],
        orient: 'Left',
        width: 400,
        height: 400,
      },
    };
  },
  components: {
    ScatterChart,
    BarChart,
  },
};
</script>

<style>
body {
  margin: 0;
}

.app {
  margin-top: 20px;
  width: 1920px;
  height: 900px;
  margin: 0 auto;
}

.grid {
  display: grid;
  grid-template-columns: repeat(2, 500px);
  grid-gap: 20px;
}

#content {
  background-color: aqua;
  float: left;
  width: 98%;
  height: 100%;
  }

#diagramm {
  background-color: rgb(255, 145, 0);
  width: 1400px;
  float: left;
}

#bot {
  background-color: rgb(200, 255, 0);
  width: 450px;
  float: right;
  height: 900px;
}
</style>
