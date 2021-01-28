import Vue from 'vue';
import Vuetify from 'vuetify';
import Dashboard from './Dashboard.vue';
import store from './store';
import 'vuetify/dist/vuetify.min.css';

Vue.config.productionTip = false;
Vue.use(Vuetify);

new Vue({
  store,
  render: (h) => h(Dashboard),
}).$mount('#app');
