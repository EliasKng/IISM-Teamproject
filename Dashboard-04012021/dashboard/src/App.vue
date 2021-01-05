<template>
  <div id="app">
    <router-view/>
  </div>
</template>

<script>
import * as d3 from "d3";

import {
  Data,
  Chart,
} from '../src/';

export default {
  name: 'app',
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
    BarChart,
    Data
  },
}
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}

#nav {
  padding: 30px;
}

#nav a {
  font-weight: bold;
  color: #2c3e50;
}

#nav a.router-link-exact-active {
  color: #42b983;
}
</style>
