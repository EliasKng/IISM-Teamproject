<template>
    <div id = "bot_component">
        <div>Test1234************************************</div>
        <div id="webchat" role="main"></div>
    </div>
</template>

<script>
/* eslint-disable */
export default {
  name: 'botscript',
  methods: {
    changeDataBot: function (data, endpoint) {
      this.$store.dispatch('changeDataBot', {"endpoint" : endpoint, "data" : data, "specs" : this.$store.state.specs});
    },
  },
  data() {
    return {
      rawJSON: "",
      data: "",
      endpoint: "",
    };
  },
  mounted() {
      let webchatScript = document.createElement('script');
      webchatScript.setAttribute('src', 'https://cdn.botframework.com/botframework-webchat/latest/webchat.js');
      webchatScript.setAttribute('crossOrigin', 'anonymous');
      document.head.appendChild(webchatScript);

      console.log("awduihawiduhaiwudh");

      (async function() {
        const res = await fetch('https://webchat-mockbot.azurewebsites.net/directline/token', { method: 'POST' });
        const { token } = await res.json();

        const store = window.WebChat.createStore({}, ({ dispatch }) => next => action => {
          if (action.type === 'DIRECT_LINE/INCOMING_ACTIVITY') {
            const event = new Event('webchatincomingactivity');

            event.data = action.payload.activity;
            window.dispatchEvent(event);
          }

          return next(action);
        });

        window.WebChat.renderWebChat(
          {
            directLine: window.WebChat.createDirectLine({
            token: 'hklRsvlqmDU.LlAinDiX1BF1692uQtyfVBMkEun7xtEe_smE_UvH6N4'
          }),
            store
          },
          document.getElementById('webchat')
        );

        window.addEventListener('webchatincomingactivity', ({ data }) => {
          console.log(`Received an activity of type "${data.type}":`);
          if (data.hasOwnProperty("value")) {
            console.log(data["value"]);
            var value_obj = JSON.parse(data["value"]);
            var endpoint = 'http://127.0.0.1:5000' + value_obj["endpoint"];
            delete value_obj["endpoint"];
            console.log(value_obj);
          }
        });

        document.querySelector('#webchat > *').focus();
      })().catch(err => console.error(err));
  }
};
</script>

<style>
    #bot_component {
        background-color: red;
    }
    #webchat {
        height: 100%;
        width: 100%;
      }
</style>
