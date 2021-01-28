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
    // sets the current state and assings the properties of the backend rawJSON to the state properties
    changeData(state, rawJSON) {
      var objJSON = JSON.parse(rawJSON);
      state.specs = {};
      state.data = objJSON["data"];
      state.type = objJSON["type"];
      state.columns = objJSON["columns"];
    },
    // sets the current state and assings the properties of the backend rawJSON to the state properties
    changeDataBot(state, rawJSON) {
      var values = JSON.parse(rawJSON.values);
      var specs = rawJSON.specs;
      console.log(typeof values);
      state.specs = specs;
      state.data = values["data"];
      console.log(state.data);
      state.type = "";
      state.type = values["type"];
      state.columns = values["columns"];
    },
  },
  getters: {
    // assigns the data array to the state and returns it to the Dashboard.vue when called
    allData: (state) => state.data,
    //
    formattedData: (state) => {
      var formattedData = state.data.map(dataElem =>{
      return {name: dataElem[0],
            total:  dataElem[1]
          }
        });
      return formattedData;
    },


  actions: {
    // calls the changeData mutation 
    changeData(context, rawJSON) {
      context.commit('changeData', rawJSON);
    },
    // calls the changeDataBot mutation and acceses backenddata via axios
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
