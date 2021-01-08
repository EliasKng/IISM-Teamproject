<template>
  <div class="page page--home">
    <section class="chart">
      <h1>Column Chart</h1>
      <D3BarChart :config="barconfig" :datum="bardata"></D3BarChart>
    </section>
  </div>
</template>




<script>
import { D3BarChart } from 'vue-d3-charts';

export default {
  name: 'HomeView',
  components: {
    D3BarChart,
  },
  data(){
    return {
      bartext: '<D3BarChart :config="chart_config" :datum="chart_data"></D3BarChart>',
      titlechart: '<D3BarChart title="Your custom title"></D3BarChart>',
      sourcechart: '<D3BarChart source="Your custom source"></D3BarChart>',
      barconfig: {
        key: 'name',
        values: ['total', 'balance'],
        color: {scheme:['#41B882', '#222f3e']},
        transition: {ease: 'easeBounceOut', duration: 1000}
      },
      bardata: [
        {name: '1992', total: 50000, balance: 748},
        {name: '1993', total: 555, balance: 5666},
        {name: '1994', total: 8574, balance: 574},
        {name: '1995', total: 15805, balance: 5805},
        {name: '1996', total: 14582, balance: 4582},
        {name: '1997', total: 26694, balance: 6694},
        {name: '1998', total: 35205, balance: 5205},
        {name: '1999', total: 45944, balance: 5944},
        {name: '2000', total: 78595, balance: 8595},
      ]
    }
  }
}
</script>
