import Vue from 'vue';
import Vuex from 'vuex';

Vue.use(Vuex);

export default new Vuex.Store({
  state: {
    data: {}
  },
  mutations: {
    change_data(state, data){
        state.data = data
    }
  },
  getters: {
    data: state => state.data
  },
  actions: {
  },
  modules: {
  },
});
