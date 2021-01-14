<template>
  <svg :width="width" :height="height" ref="svg">
    <g ref="chart"></g>
    <g ref="circle"></g>
    <g ref="axis"></g>
  </svg>
</template>

<script>
import * as d3 from 'd3';
import { xSelector, ySelector } from '../utils';

export default {
  name: 'ScatterChart',
  props: ['data', 'xAxisLabel', 'yAxisLabel'],
  data() {
    return {
      width: 500,
      height: 500,
      path: '',
    };
  },
  mounted() {
    const xScale = d3
      .scaleLinear()
      .range([0, 400])
      .domain([0, 10]);
    const yScale = d3
      .scaleLinear()
      .range([420, 0])
      .domain([0, 420]);
    const path = d3
      .line()
      .x((d) => xScale(xSelector(d)))
      .y((d) => yScale(ySelector(d)));
    this.path = path(this.data);

    const margin = {
      top: 40, left: 40, bottom: 40, right: 0,
    };
    const yAxis = d3.axisLeft(yScale).tickSizeInner(-420);
    const xAxis = d3.axisBottom(xScale);

    const chartWidth = this.width - (margin.left + margin.right);
    const chartHeight = this.height - (margin.top + margin.bottom);

    d3
      .select(this.$refs.chart)
      .attr('width', chartWidth)
      .attr('height', chartHeight)
      .attr('transform', `translate(${margin.left}, ${margin.top})`);

    // set xAxis
    d3
      .select(this.$refs.axis)
      .append('g')
      .attr(
        'transform',
        `translate(${margin.left}, ${chartHeight + margin.top})`,
      )
      .attr('class', 'axis x')
      .text('xAxis')
      .call(xAxis);

    // set xAxis Label
    d3
      .select(this.$refs.axis)
      .append('text')
      .attr('transform', `translate(${(chartWidth / 2)}, ${chartHeight + 35 + margin.bottom})`)
      .style('text-anchor', 'middle')
      .text(this.xAxisLabel);
    // set yAxis
    d3
      .select(this.$refs.axis)
      .append('g')
      .attr('transform', `translate(${margin.left}, ${margin.top})`)
      .attr('class', 'axis y')
      .call(yAxis);

    // set yAxis Label
    d3
      .select(this.$refs.axis)
      .append('text')
      .attr('transform', `translate(${margin.left}, ${0 + margin.bottom - 35})`)
      .attr('dy', '1em')
      .style('text-anchor', 'middle')
      .text(this.yAxisLabel);

    this.data.forEach((d) => {
      d3
        .select(this.$refs.circle)
        .attr('transform', `translate(130, ${margin.top})`)
        .append('circle')
        .attr('cx', this.xPoint(d))
        .attr('cy', this.yPoint(d))
        .attr('r', '5')
        .attr('stroke', '#fff')
        .attr('strokeWidth', 3)
        .attr('fill', '#3CB371');
    });
  },
  methods: {
    xPoint(d) {
      const xScale = d3
        .scaleLinear()
        .range([0, 400])
        .domain([0, 10]);
      return xScale(xSelector(d));
    },
    yPoint(d) {
      const yScale = d3
        .scaleLinear()
        .range([420, 0])
        .domain([0, 420]);
      return yScale(ySelector(d));
    },
  },
};
</script>

<style>
</style>
