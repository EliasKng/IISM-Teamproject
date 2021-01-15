import Vue from 'vue';
import Dashboard from './Dashboard.vue';
import store from './store';

Vue.config.productionTip = false;

new Vue({
  store,
  render: (h) => h(Dashboard),
}).$mount('#app');
