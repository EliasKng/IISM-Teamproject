<template>
  <div id="app">
    <header id="header"><h1>IISM Teamprojekt Dashboard</h1></header>
    <h3>this is the data : {{this.$store.state.type}}  {{formattedData}}</h3>
    <textarea v-model="rawJSON" placeholder="add Data here"></textarea>
    <br>
    <textarea v-model="data" placeholder="add Query/params/commands here"></textarea>
    <br>
    <textarea v-model="endpoint" placeholder="add endpoint here"></textarea>

    <button v-on:click="changeDataBot(data, endpoint)">Submit</button>
    <!-- eslint-disable-next-line -->
    <D3BarChart :config="barconfig" :datum="formattedData" title="Lm" source="Dt" v-if="this.$store.state.type === 'BarChart'"></D3BarChart>
    <!-- eslint-disable-next-line -->
    <D3BarChart :config="columnconfig" :datum="formattedData" title="Lm" source="Dt" v-if="this.$store.state.type === 'ColumnChart'"></D3BarChart>
    <!-- eslint-disable-next-line -->
    <D3PieChart :config="pieconfig" :datum="formattedData" title="Lo" source="Dl" v-if="this.$store.state.type === 'PieChart'"></D3PieChart>
    <scatter-chart :data="formattedData" v-if="this.$store.state.type === 'ScatterPlot'"/>
    <bot @callStore="changeDataBot"></bot>
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
  },
  data() {
    return {
      rawJSON: "",
      data: "",
      endpoint: "",    
      piedata: [
        { name: 'Lorem', total: 30, color: '#425265' },
        { name: 'Ipsum', total: 21, color: '#3e9a70' },
        { name: 'Dolor', total: 20, color: '#41ab7b' },
        { name: 'Sit', total: 112, color: '#222f3e' },
      ],
      
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
.header {
/*  float:center;
  font-weight: bold; */
}
.app {
/*  margin-top: 20px;
  width: 1920px;
  height: 900px;
  margin: 0 auto; */
}

.grid {
/*  display: grid;
  grid-template-columns: repeat(2, 500px);
  grid-gap: 20px; */
}

#content {
/*
  background-color: aqua;
  float: left;
  width: 98%;
  height: 100%; */
  }

#bot {
/*  background-color: rgb(200, 255, 0);
  width: 450px;
  float: right;
  height: 900px; */
}
</style>
