/* eslint-disable */


import axios from 'axios';
import Vue from 'vue';
import Vuex from 'vuex';

Vue.use(Vuex);

export default new Vuex.Store({
  state: {
    specs: {},
    type: "",
    columns: {},
    data: [],
  },
  mutations: {
    changeData(state, rawJSON) {
      var objJSON = JSON.parse(rawJSON);
      state.specs = {};
      state.data = objJSON["data"];
      state.type = objJSON["type"];
      state.columns = objJSON["columns"];
    },
    changeDataBot(state, rawJSON) {
      var values = JSON.parse(rawJSON.values);
      var specs = rawJSON.specs;
      console.log(typeof values);
      state.specs = specs;
      console.log(specs);
      console.log(typeof specs);
      state.data = values["data"];
      console.log(state.data);
      state.type = "";
      state.type = values["type"];
      state.columns = values["columns"];

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
      return {name: dataElem[0], total:  dataElem[1], color:  'red'}});
      return formattedDataPieChart;},
  },

  actions: {
    changeData(context, rawJSON) {
      context.commit('changeData', rawJSON);
    },
    changeDataBot(context, backendSpecs) {
      var value_obj = [backendSpecs["data"], backendSpecs["specs"]];
      var endpoint = 'http://127.0.0.1:5000' + backendSpecs["endpoint"];
      axios
        .post(endpoint, value_obj, {headers: {"Content-Type": "application/json"}})
        .then(response =>{
          console.log((response.data));
          context.commit('changeDataBot', (response.data))
        })
    },
  },
  modules: {
  },
});
