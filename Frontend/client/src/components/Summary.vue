<template>
<!-- eslint-disable -->
<!-- Compoment for the Summary, in which the aplied filters, aggregate and axis labeling are shown -->
<!-- eslint-disable -->
    <v-card class="summary">
      <!-- <h1>{{this.$store.state.specs}}</h1> -->
      <v-card-title>Summary</v-card-title>
      <v-card-text>
        <ul id="list">
          <!-- section for charttype -->
          <li>Diagram Type: {{this.$store.state.type}}</li>
          <hr v-if="Object.keys(this.$store.state.specs.keywords) != 0">
            <ul v-if="Object.keys(this.$store.state.specs.keywords) != 0" id="list">
            <li v-if="Object.keys(this.$store.state.specs.keywords) != 0">
              <!-- section for the Filter -->
              <h3>Filter</h3>
            </li>
            <li v-for="(item, name) in this.$store.state.specs.keywords" >{{name}} {{item[0]}} {{item[1]}}</li>
          </ul>
          <hr>
          <!-- section for y-Axis labeling and aggregate  -->
          <ul id="list">
            <h3>Y-Axis</h3>
            <li v-if="(this.$store.state.specs.type === 'PieChart')">{{this.$store.state.specs.theta_encoding.field}}</li>
            <li v-else>{{this.$store.state.specs.y_encoding.field}}</li>
           <li>Aggregation: {{yaggregation}}</li>

          </ul>
          <hr>
          <ul id="list">
            <h3>X-Axis</h3>
            <li v-if="(this.$store.state.specs.type === 'PieChart')">{{this.$store.state.specs.color_encoding.field}}</li>
            <li v-else>{{this.$store.state.specs.x_encoding.field}}</li>
            <li>Aggregation: {{xaggregation}}</li>
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
    yaggregation(){
    if(this.$store.state.specs.type === 'PieChart'){
      if(!(this.$store.state.specs.theta_encoding.aggregate === ''||this.$store.state.specs.theta_encoding.aggregate === null)||(this.$store.state.specs.theta_encoding.aggregate == 'Null')){
        return this.$store.state.specs.theta_encoding.aggregate
      }else{
        return 'none'
      }
    };
    if (!(this.$store.state.specs.y_encoding.aggregate === ''||this.$store.state.specs.y_encoding.aggregate === null)||(this.$store.state.specs.y_encoding.aggregate == 'Null')){
      return this.$store.state.specs.y_encoding.aggregate
    } else{
       return 'none'
    }

    },
    xaggregation(){
    if(this.$store.state.specs.type === 'PieChart'){
      if(!(this.$store.state.specs.color_encoding.aggregate === ''||this.$store.state.specs.color_encoding.aggregate === null)||(this.$store.state.specs.color_encoding.aggregate == 'Null')){
        return this.$store.state.specs.color_encoding.aggregate
      }else{
        return 'none'
      }
    };
    if (!(this.$store.state.specs.x_encoding.aggregate === ''||this.$store.state.specs.x_encoding.aggregate === null)||(this.$store.state.specs.x_encoding.aggregate == 'Null')){
      return this.$store.state.specs.x_encoding.aggregate
    } else{
       return 'none'
    }

    }
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