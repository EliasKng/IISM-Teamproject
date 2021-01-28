/* main component */
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
            <!-- Header for the Chart. Toggle for the specific figur names -->
            <div id="chartheader">
              <!-- eslint-disable-next-line -->
              <h1 v-if="this.$store.state.type === 'BarChart'">{{this.$store.state.columns["y-axis"]}} by {{this.$store.state.columns["x-axis"]}}</h1>
              <!-- eslint-disable-next-line -->
              <h1 v-if="this.$store.state.type === 'ColumnChart'">{{this.$store.state.columns["x-axis"]}} by {{this.$store.state.columns["y-axis"]}}</h1>
              <!-- eslint-disable-next-line -->
              <h1 v-if="this.$store.state.type === 'PieChart'">{{this.$store.state.columns["theta"]}} by {{this.$store.state.columns["color"]}}</h1>
              <!-- eslint-disable-next-line -->
              <h1 v-if="this.$store.state.type === 'Scatterplot'">{{this.$store.state.columns["x-axis"]}} by {{this.$store.state.columns["y-axis"]}}</h1>
            </div>
              <div>
                <!-- Implementation of the importet charts -->
                <!-- eslint-disable-next-line -->
                <D3BarChart  :config="barconfig" :datum="formattedData" title="" v-if="this.$store.state.type === 'BarChart'" ></D3BarChart>
                <!-- eslint-disable-next-line -->
                <D3BarChart :config="columnconfig" :datum="formattedData" title="" v-if="this.$store.state.type === 'ColumnChart'" ></D3BarChart>
                <!-- eslint-disable-next-line -->
                <D3PieChart :config="pieconfig" :datum="formattedData" title="" v-if="this.$store.state.type === 'PieChart'"></D3PieChart>
                <!-- eslint-disable-next-line -->
                <scatter-chart :data="formattedData" v-if="this.$store.state.type === 'ScatterPlot'"/>
                </div>
          </div>
          <!-- Implementation of importet Summary -->
          <Summary></Summary>
          </div>
          <!-- Implementation of importet Chatbot -->
        <div class="botarea">
          <bot @callStore="changeDataBot"></bot>
        </div>
    </section>
  </div>
</template>

<script>
/* eslint-disable */
import { D3BarChart, D3PieChart } from 'vue-d3-charts';
import ScatterChart from './components/ScatterChart.vue';
import Bot from './components/Bot';
import Summary from './components/Summary.vue'


export default {
  name: 'app',
  computed: {
    // forces to rerender and calls stores getter function formatedData
    formattedData() {
      this.forceRerender();
      return this.$store.getters.formattedData;
    },
    // forces to rerender and calls stores getter function formattedDataPieChart
    formattedDataPieChart() {
      return this.$store.getters.formattedDataPieChart;
    },
    // configuration for the barchart
    barconfig() { 
      return {
        key: 'name',
        values: ['total'],
        orientation: 'vertical',
        color: { current: '#41B882' },
        axis: {xTitle: "", yTitle:  "", yTicks: 10, yFormat: ".0s", xTicks: 10, xFormat: '.0s' },
        transition: { ease: 'easeBounceOut', duration: 1000 },
      };
    },
    // configuration for the columnchart
    columnconfig() { 
      return {
        key: 'name',
        values: ['total'],
        orientation: 'horizontal',
        color: { current: '#41B882' },
        axis: {xTitle: "", yTitle:  "", yTicks: 10, yFormat: '.0s' },
        transition: { ease: 'easeBounceOut', duration: 1000 },
      };
    },  
  },
  methods: {
    // calls the store action changeData
    changeData: function (rawJSON) {
      this.$store.dispatch('changeData', rawJSON);
    },
    // calls the store action changeDataBot
    changeDataBot: function (data, endpoint) {
      this.$store.dispatch('changeDataBot', {"endpoint" : endpoint, "data" : data, "specs" : this.$store.state.specs});
    },
    // forces to rerender the chartarea via the "key"
    forceRerender() {
       this.componentKey += 1; 
    },
/*     updateSummary() {
    
    this.summarytype.push(this.$store.state.type);
    if(this.$store.state.type == 'PieChart'){
    this.summarydata.push(this.$store.state.columns["theta"] +" by "+ this.$store.state.columns["color"]);
    }
    else{
      this.summarydata.push(this.$store.state.columns["y-axis"] +" by "+ this.$store.state.columns["x-axis"]);
    }    
    }, */
  },
   data() {
    return {
      rawJSON: "",
      componentKey: 0,
      data: "",
      endpoint: "",
      // configuration for the piechart      
      pieconfig: {
        key: 'name',
        value: 'total',
        radius: { inner: 80 },
        color: { scheme: "schemeCategory10", },
        transition: { duration: 200 },
      },
    };
  },
  /* watch: {
   componentKey:  "updateSummary",    
  }, */
  components: {
    D3BarChart,
    D3PieChart,
    ScatterChart,
    Bot,
    Summary
  },
};
</script>

<style>
body {
  margin: 0;
}
#header {
  padding: 20px;
   font-family: Arial, sans-serif;
  text-align: center;
  font-weight: bold;
}

.container {
   font-family: Arial, sans-serif;
  width: 95%;
  height: 900px;
  background: rgb(230, 230, 230);
  margin: auto;
  padding: 10px;
  overflow: hidden;
}

#chartheader {
  padding-top: 20px;
}
.chartarea {
  text-align: center;
  width: 75%;
  height: 800px;
  background:rgb(230, 230, 230);
  float: left;
  overflow: hidden;
}

.chart {
  width: 90%;
  height: 65%;
  margin-left: auto; 
  margin-right: auto;
  background: rgb(255, 255, 255);
  margin-top: 10px;
  margin-bottom: 10px;
}

.botarea {
  margin-left: 25%;
  height: 800px;
  background: rgb(255, 255, 255);
  overflow: hidden;
}

#app {
  height: 900px;  
  overflow: hidden;
}
</style>
