import Vue from 'vue';
import Router from 'vue-router';
import Data from './components/Data.vue';
import BarChart from '@/components/BarChart.vue'

Vue.use(Router);

export {
  BarChart,
};

export default new Router({
  mode: 'history',
  base: process.env.BASE_URL,
  routes: [
    {
      path: '/data',
      name: 'Data',
      component: Data,
    },
    {
      path: '/barchart',
      name: 'BarChart',
      component: BarChart,
    },
  ],
});