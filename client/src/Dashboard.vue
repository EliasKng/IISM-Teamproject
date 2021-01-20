<template>
  <div id="app">
    <header id="header"><h1>IISM Teamprojekt Dashboard</h1></header>
    <!--
    <h3>this is the data : {{this.$store.state.type}}  {{formattedData}}</h3>
    <textarea v-model="rawJSON" placeholder="add Data here"></textarea>
    <br>
    <textarea v-model="data" placeholder="add Query/params/commands here"></textarea>
    <br>
    <textarea v-model="endpoint" placeholder="add endpoint here"></textarea>

    <button v-on:click="changeDataBot(data, endpoint)">Submit</button>
    -->
    <section class="container">
        <div class="chartarea">
          <div class="chart" :key="componentKey">
            <!-- eslint-disable-next-line -->
            <D3BarChart :config="barconfig" :datum="formattedData" title="Lm" source="Dt" v-if="this.$store.state.type === 'BarChart'"></D3BarChart>
            <!-- eslint-disable-next-line -->
            <D3BarChart :config="columnconfig" :datum="formattedData" title="Lm" source="Dt" v-else-if="this.$store.state.type === 'ColumnChart'" ></D3BarChart>
            <!-- eslint-disable-next-line -->
            <D3PieChart :config="pieconfig" :datum="formattedData" title="Lo" source="Dl" v-else-if="this.$store.state.type === 'PieChart'"></D3PieChart>
            <!-- eslint-disable-next-line -->
            <scatter-chart :data="formattedData" v-else-if="this.$store.state.type === 'ScatterPlot'"/></div>
          </div>
        <div class="botarea">
          <bot @callStore="changeDataBot"></bot>
        </div>
      </section>
    <button v-on:click="forceRerender()">refresh</button>
    <h1>{{ buttonvalue }}</h1>
  </div>
</template>

<script>
/* eslint-disable */
import { D3BarChart, D3PieChart } from 'vue-d3-charts';
import ScatterChart from './components/ScatterChart.vue';
import Bot from './components/Bot';

export default {
  name: 'app',
  computed: {
    formattedData() {
      return this.$store.getters.formattedData;
    },
    formattedDataPieChart() {
      return this.$store.getters.formattedDataPieChart;
    },
    barconfig() { 
      return {
        key: 'name',
        values: ['total'],
        orientation: 'vertical',
        color: { current: '#41B882' },
        axis: {xTitle: this.$store.state.columns["x-axis"], yTitle: this.$store.state.columns["y-axis"], yTicks: 10, yFormat: '.0s' },
        transition: { ease: 'easeBounceOut', duration: 1000 },
      };
    },
    columnconfig() { 
      return {
        key: 'name',
        values: ['total'],
        orientation: 'horizontal',
        color: { current: '#41B882' },
        axis: { yTitle: this.$store.state.columns["x-axis"], yTicks: 10, yFormat: '.0s' },
        transition: { ease: 'easeBounceOut', duration: 1000 },
      };
    },  
  },
  methods: {
    changeData: function (rawJSON) {
      this.$store.dispatch('changeData', rawJSON);
    },
    changeDataBot: function (data, endpoint) {
      this.$store.dispatch('changeDataBot', {"endpoint" : endpoint, "data" : data, "specs" : this.$store.state.specs});
    },
    forceRerender() {
       this.componentKey += 1; 
       console.log("component key", this.componentKey)
    },
    changeButtonvalue() {
      this.buttonvalue += 1;
     },
  },
  data() {
    return {
      rawJSON: "",
      buttonvalue: 1,
      componentKey: 0,
      data: "",
      endpoint: "",      
      pieconfig: {
        key: 'name',
        value: 'total',
        radius: { inner: 80 },
        color: { key: 'color' },
        transition: { duration: 200 },
      },
    };
  },
  components: {
    D3BarChart,
    D3PieChart,
    ScatterChart,
    Bot,
  },
};
</script>

<style>
body {
  margin: 0;
}
#header {
  text-align: center;
  font-weight: bold;
}

.container {
  width: 95%;
  height: 900px;
  background: rgb(230, 230, 230);
  margin: auto;
  padding: 10px;
  overflow: hidden;
}

.chartarea {
  width: 75%;
  height: 800px;
  background: rgb(225, 252, 248);
  float: left;
  overflow: hidden;
}

.chart {
  width: 90%;
  height: 100%;  
  margin-top:auto;;
  margin-left: auto; 
  margin-right: auto;
}

.botarea {
  margin-left: 25%;
  height: 800px;
  background: rgb(255, 255, 255);
  overflow: hidden;
}
#app {
  height: 900px;
}
</style>
