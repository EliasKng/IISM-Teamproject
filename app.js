const DUMMY_DATEN = [
    {id: 'd1', value: 10, region : 'USA'}
    {id: 'd2', value: 12, region : 'GER'}
    {id: 'd3', value: 13, region : 'FRA'}
    {id: 'd4', value: 15, region : 'ITA'}
 ]

 const container = d3.select('div')
    .classed('container', true)
    .style('1px solid red');

const bars = container
 .selectAll('.bar')
 .data(DUMMY_DATEN)
 .enter()
 .append('div')
 .classed('bar', true) 