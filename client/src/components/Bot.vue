<template>
    <div id = "bot_component">
        <div id="webchat" role="main"></div>
    </div>
</template>

<script>
/* eslint-disable */
export default {
  name: 'botscript',
  mounted() {
      console.log(this.$store);
      const storemounted  = this.$store; 
      let webchatScript = document.createElement('script');
      webchatScript.setAttribute('src', 'https://cdn.botframework.com/botframework-webchat/latest/webchat.js');
      webchatScript.setAttribute('crossOrigin', 'anonymous');
      document.head.appendChild(webchatScript);


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

        window.addEventListener('webchatincomingactivity', function({ data }) {
          console.log(`Received an activity of type "${data.type}":`);
          if (data.hasOwnProperty("value")) {
            console.log("DataValue: " + data["value"]);
            
            var value_obj = JSON.parse(data["value"]);
            var endpoint = value_obj["endpoint"];
            delete value_obj["endpoint"];

            console.log("CALLING FUNCTION");
            console.log(endpoint)
            storemounted.dispatch('changeDataBot', {"endpoint" : endpoint, "data" : value_obj, "specs" : storemounted.state.specs});
          }
        });

        document.querySelector('#webchat > *').focus();
      })().catch(err => console.error(err));
  },
  data() {
    return {
      rawJSON: "",
      data: "",
      endpoint: "",
      value_obj: {},
    };
  },
  
};
</script>

<style>
    #webchat {
       
        height: 900px;
        width: 576px;
      }
</style>
