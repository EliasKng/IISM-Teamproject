/* eslint-disable */


import Vue from 'vue';
import Vuex from 'vuex';

Vue.use(Vuex);

export default new Vuex.Store({
  state: {
    type: "",
    columns: {},
    data: 
    [],
  },
  mutations: {
    changeData(state, rawJSON) {
      var objJSON = JSON.parse(rawJSON)
      state.data = objJSON["data"];
      state.type = objJSON["type"];
      state.columns = objJSON["columns"];
    },
  },
  getters: {
    allData: (state) => state.data,
    formattedData: (state) => {
      var formattedData = state.data.map(dataElem =>{
      return {name: dataElem[0],
            total:  dataElem[1]
          }
        });
      return formattedData;
    }
  },

  actions: {
    changeData(context, rawJSON) {
      context.commit('changeData', rawJSON);
    },
  },
  modules: {
  },
});
