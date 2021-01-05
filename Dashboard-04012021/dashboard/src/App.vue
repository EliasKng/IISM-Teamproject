<template>
  <div id="app">
    <router-view/>
    <div class="grid">
      <div class="grid__item">
        <h1>Bar Chart</h1>
        <bar-chart :data="dataChart" width="500" height="500"/>
      </div>
      </div>
  </div>
</template>

<script>
import BarChart from './components/BarChart.vue';
import { obj } from './output/output';

export default {
  name: 'app',
  data() {
    return {
      dataChart: obj,
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
    BarChart,
  },
};
</script>;

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
