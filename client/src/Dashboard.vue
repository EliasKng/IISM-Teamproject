<template>
  <div id="app">
    <header id="header"><h1>IISM Teamprojekt Dashboard</h1></header>
    <p> {{ showchart  }} </p>
    <!--v-if="charttype === 'Barchart'"-->
    <!-- eslint-disable-next-line -->
    <D3BarChart :config="barconfig" :datum="barcolumndata" title="Lm" source="Dt" v-if="showchart === 'Barchart'"></D3BarChart>
    <!-- eslint-disable-next-line -->
    <D3BarChart :config="columnconfig" :datum="barcolumndata" title="Lm" source="Dt" v-if="showchart === 'Columnchart'"></D3BarChart>
    <!-- eslint-disable-next-line -->
    <D3PieChart :config="pieconfig" :datum="piedata" title="Lo" source="Dl" v-if="showchart === 'Piechart'"></D3PieChart>
    <scatter-chart :data="Scatterchartdata" xAxisLabel="xAxis" yAxisLabel="yAxis"
     v-if="showchart === 'Scatterplot'"/>
    <div id="bot">
        <iframe src='https://webchat.botframework.com/embed/VisBot?s=hklRsvlqmDU.LlAinDiX1BF1692uQtyfVBMkEun7xtEe_smE_UvH6N4'  style='width: 100%; height: 100%;'></iframe>
    </div>
  </div>
</template>

<script>
import { D3BarChart, D3PieChart } from 'vue-d3-charts';
import ScatterChart from './components/ScatterChart.vue';
import { data } from './components/data';
import { charttype } from './components/charttype';

export default {
  name: 'app',
  data() {
    return {
      // 0 - barchart, 1 - column, 2 - pie, 3 - scatter
      showchart: charttype[3],
      Scatterchartdata: data,
      barcolumndata: [
        { name: '1992', total: 4748 },
        { name: '1993', total: 5526 },
        { name: '1994', total: 8574 },
        { name: '1995', total: 15805 },
        { name: '1996', total: 14582 },
        { name: '1997', total: 26694 },
        { name: '1998', total: 35205 },
        { name: '1999', total: 45944 },
        { name: '2000', total: 78595 },
        { name: '2001', total: 78530 },
        { name: '2002', total: 45407 },
        { name: '2003', total: 54044 },
        { name: '2004', total: 69165 },
        { name: '2005', total: 61798 },
        { name: '2006', total: 63686 },
      ],
      piedata: [
        { name: 'Lorem', total: 30, color: '#425265' },
        { name: 'Ipsum', total: 21, color: '#3e9a70' },
        { name: 'Dolor', total: 20, color: '#41ab7b' },
        { name: 'Sit', total: 112, color: '#222f3e' },
      ],
      barconfig: {
        key: 'name',
        values: ['total'],
        orientation: 'vertical',
        color: { current: '#41B882' },
        axis: { yTitle: 'Lorem ipsum dolor sit amet', yTicks: 10, yFormat: '.0s' },
        transition: { ease: 'easeBounceOut', duration: 1000 },
      },
      columnconfig: {
        key: 'name',
        values: ['total'],
        orientation: 'horizontal',
        color: { current: '#41B882' },
        axis: { yTitle: 'Lorem ipsum dolor sit amet', yTicks: 10, yFormat: '.0s' },
        transition: { ease: 'easeBounceOut', duration: 1000 },
      },
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
