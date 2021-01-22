<template>
<!-- eslint-disable -->
    <v-card class="summary">
      <!-- <h1>{{this.$store.state.specs}}</h1> -->
      <v-card-title>Summary</v-card-title>
      <v-card-text>
        <ul id="list">
          <li>Diagrammtyp: {{this.$store.state.type}}</li>
          <hr v-if="Object.keys(this.$store.state.specs.keywords) != 0">
            <ul v-if="Object.keys(this.$store.state.specs.keywords) != 0" id="list">
            <li v-if="Object.keys(this.$store.state.specs.keywords) != 0">
              <h3>Filter</h3>
            </li>
            <li v-for="(item, name) in this.$store.state.specs.keywords" >{{name}} {{item[0]}} {{item[1]}}</li>
          </ul>
          <hr>
          <ul id="list">
            <h3>Y-Achse</h3>
            <li>{{this.$store.state.specs.y_encoding.field}}</li>
            <li v-if="(this.$store.state.specs.y_encoding.aggregate === null)||(this.$store.state.specs.y_encoding.aggregate == 'Null')">Aggregierung: keine</li>
            <li v-if="!(this.$store.state.specs.y_encoding.aggregate === null)||(this.$store.state.specs.y_encoding.aggregate == 'Null')">Aggregierung: {{this.$store.state.specs.y_encoding.aggregate}}</li>
            <li>{{this.$store.state.specs.y_encoding.aggregate}}</li>
          </ul>
          <hr>
          <ul id="list">
            <h3>X-Achse</h3>
            <li>{{this.$store.state.specs.x_encoding.field}}</li>
            <li v-if="(this.$store.state.specs.x_encoding.aggregate === null)||(this.$store.state.specs.x_encoding.aggregate == 'Null')">Aggregierung: keine</li>
            <li v-if="!((this.$store.state.specs.x_encoding.aggregate === null)||(this.$store.state.specs.x_encoding.aggregate == 'Null'))">Aggregierung: {{this.$store.state.specs.x_encoding.aggregate}}</li>
          </ul>
        </ul>
      </v-card-text>
    </v-card>
</template>

<script>
/* eslint-disable */

export default {
  name: 'Summary',
  computed: {  
/*      filterCountry:  function() {return _.pickBy(this.$store.state.specs.keywords, function(a){return a})}
    filterCountry: function() { 
      return this.$store.state.specs.keywords.filter(function(c){
        return c.country})} */
    },
  data() {
    return {

    };
  },
};
</script>

<style>
.summary {
  font-family: Arial, sans-serif;
  background: rgb(255, 255, 255);
  width: 90%;
  height: 30%;
  margin-left: auto; 
  margin-right: auto;
  margin-top: 10px;
  margin-bottom: 10px;
  overflow-x: hidden;
}

#list {
  list-style-type: none;
}
table{
  width: 100%;
  border-collapse: collapse;
}

td, th {
  border: 1px solid #dddddd;
  text-align: left;
  padding: 8px;
}

</style>