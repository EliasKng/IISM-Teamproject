<template>
  <div class="my-app">
    <D3BarChart :config="chart_config" :datum="chart_data"></D3BarChart>
  </div>
</template>

<script>
import { D3BarChart } from 'vue-d3-charts';

export default {
  components: {
    D3BarChart,
  },
  data() {
    return {
      chart_data: [
        { vue: 20, d3: 62, category: 'lorem' },
        { vue: 28, d3: 47, category: 'ipsum' },
      ],
      chart_config: {
        key: 'category',
        values: ['vue', 'd3'],
        orientation: 'horizontal',
        color: {
          keys: {
            vue: '#41b882',
            d3: '#b35899',
          },
        },
      },
    };
  },
};
</script>
