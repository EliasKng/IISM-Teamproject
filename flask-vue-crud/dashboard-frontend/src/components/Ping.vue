<template>
 <div class="container">
    <div class="row">
      <div class="col-sm-10">
        <h1>Books</h1>
        <hr><br><br>
        <button type="button" class="btn btn-success btn-sm">Add Book</button>
        <br><br>
        <table class="table table-hover">
          <thead>
            <tr>
              <th scope="col">Title</th>
              <th scope="col">Author</th>
              <th scope="col">Read?</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(book, index) in books" :key="index">
              <td>{{ book.title }}</td>
              <td>{{ book.author }}</td>
              <td>
                <span v-if="book.read">Yes</span>
                <span v-else>No</span>
              </td>
              <td>
                <div class="btn-group" role="group">
                  <button type="button" class="btn btn-warning btn-sm">Update</button>
                  <button type="button" class="btn btn-danger btn-sm">Delete</button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
  <!--
  <div>
    <h1>Column Chart</h1>
    <D3BarChart :config="barconfig" :datum="bardata"></D3BarChart>
    <p>{{ msg }}</p>
  </div>
  -->
</template>

<script>
import axios from 'axios';

export default {
  name: 'Ping',
  data() {
    return {
      msg: '',
    };
  },
  methods: {
    getMessage() {
      const path = 'http://localhost:5000/ping';
      axios.get(path)
        .then((res) => {
          this.msg = res.data;
        })
        .catch((error) => {
          // eslint-disable-next-line
          console.error(error);
        });
    },
  },
  created() {
    this.getMessage();
  },
};

/*
import axios from 'axios';
import { D3BarChart } from 'vue-d3-charts';

export default {
  name: 'Ping',
  components: {
    D3BarChart,
  },
  data() {
    return {
      bartext: '<D3BarChart :config="chart_config" :datum="chart_data"></D3BarChart>',
      titlechart: '<D3BarChart title="Your custom title"></D3BarChart>',
      sourcechart: '<D3BarChart source="Your custom source"></D3BarChart>',
      barconfig: {
        key: 'name',
        values: ['total', 'balance'],
        color: { scheme: ['#41B882', '#222f3e'] },
        transition: { ease: 'easeBounceOut', duration: 1000 },
      },
      bardata: [
        { name: '1992', total: 50000, balance: 748 },
        { name: '1993', total: 555, balance: 5666 },
        { name: '1994', total: 8574, balance: 574 },
        { name: '1995', total: 15805, balance: 5805 },
        { name: '1996', total: 14582, balance: 4582 },
        { name: '1997', total: 26694, balance: 6694 },
        { name: '1998', total: 35205, balance: 5205 },
        { name: '1999', total: 45944, balance: 5944 },
        { name: '2000', total: 78595, balance: 8595 },
      ],
      msg: '',
    };
  },
  methods: {
    getMessage() {
      const path = 'http://localhost:5000/ping';
      axios.get(path)
        .then((res) => {
          this.msg = res.data;
        })
        .catch((error) => {
          // eslint-disable-next-line
          console.error(error);
        });
    },
  },
  created() {
    this.getMessage();
  },
};

*/
</script>
