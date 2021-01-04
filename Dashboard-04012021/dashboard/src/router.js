import Vue from 'vue';
import Router from 'vue-router';
import Data from './components/Data.vue';

Vue.use(Router);

export default new Router({
  mode: 'history',
  base: process.env.BASE_URL,
  routes: [
    {
      path: '/data',
      name: 'Data',
      component: Data,
    }
  ],
});