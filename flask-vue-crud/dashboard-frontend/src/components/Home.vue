<template>
  <div id="app">
    <div id="content">
      <div id= "diagramm">
        <Visualization/>
      </div>
      <div id="bot">
        <iframe src='https://webchat.botframework.com/embed/VisBot?s=hklRsvlqmDU.LlAinDiX1BF1692uQtyfVBMkEun7xtEe_smE_UvH6N4'  style='width: 100%; height: 100%;'></iframe>
      </div>
    </div>
    </div>
</template>

<script>
import axios from 'axios';
import Visualization from './Visualization.vue';

export default {
  data() {
    return {
      books: [],
    };
  },
  components: {
    Visualization,
  },
  methods: {
    getData() {
      const path = 'http://localhost:5000/data';
      axios.get(path)
        .then((res) => {
        // get return from app.py
          this.books = res.data.books;
        })
        .catch((error) => {
          // eslint-disable-next-line
          console.error(error);
        });
    },
  },
  created() {
    this.getBooks();
  },
};
</script>

<style>
body {
  margin: 0;
}

.app {
  margin-top: 20px;
  width: 1920px;
  height: 900px;
  margin: 0 auto;
}

.grid {
  display: grid;
  grid-template-columns: repeat(2, 500px);
  grid-gap: 20px;
}

#content {
  background-color: aqua;
  float: left;
  width: 98%;
  height: 100%;
  }

#diagramm {
  background-color: rgb(255, 145, 0);
  width: 1400px;
  float: left;
}

#bot {
  background-color: rgb(200, 255, 0);
  width: 450px;
  float: right;
  height: 900px;
}
</style>
