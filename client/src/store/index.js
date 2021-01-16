/* eslint-disable */


import Vue from 'vue';
import Vuex from 'vuex';

Vue.use(Vuex);

export default new Vuex.Store({
  state: {
    specs: {},
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
    },
    formattedDataPieChart: (state) => {
      var formattedDataPieChart = state.data.map(dataElem =>{
      return {name: dataElem[0],
            total:  dataElem[1],
            color:  
          }
        });
      return formattedDataPieChart;
    }
  },

  actions: {
    changeData(context, rawJSON) {
      context.commit('changeData', rawJSON);
    },
    changeDataBot(context, Query, Endpoint) {
      var url = 'http://127.0.0.1:5000' + Endpoint;
      var json = JSON.stringify(Query);
      axios(url, {
        method: "post",
        data: json,
        withCredentials: true
      })
      .then(response => {context.commit('changeData', response.data)})
  },
  },
  modules: {
  },
});
