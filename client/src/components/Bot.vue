<template>
  <div id="bot_component">
    <div id="webchat" role="main"></div>
  </div>
</template>

<script>
/* eslint-disable */
export default {
  name: "botscript",
  mounted() {
    console.log(this.$store);
    const storemounted = this.$store;
    let webchatScript = document.createElement("script");
    webchatScript.setAttribute(
      "src",
      "https://cdn.botframework.com/botframework-webchat/latest/webchat.js"
    );
    webchatScript.setAttribute("crossOrigin", "anonymous");
    document.head.appendChild(webchatScript);
    let jqueryScript = document.createElement("script");
    jqueryScript.setAttribute(
      "src",
      "https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"
    );
    document.head.appendChild(jqueryScript);




    async function createHybridPonyfillFactory() {
      const speechServicesPonyfillFactory = await window.WebChat.createCognitiveServicesSpeechServicesPonyfillFactory({
        credentials: {
          region: 'westus2',
          subscriptionKey: '1bdd6845e36e466cb1478114ea8d641c' //Hier kommt der key eines 'Cognitive Services' von Azure rein. Aktuell ist das einer von mir, den ihr zum testen verwenden könnt.
        }
      });

      return (options) => {
        const speech = speechServicesPonyfillFactory(options);

        return {
          SpeechGrammarList: speech.SpeechGrammarList,
          SpeechRecognition: speech.SpeechRecognition,
          speechSynthesis: null, // speech.speechSynthesis,
          SpeechSynthesisUtterance: null, // speech.SpeechSynthesisUtterance
        };
      }
    };



    (async function () {

      var directLine = window.WebChat.createDirectLine({
        secret: "hklRsvlqmDU.LlAinDiX1BF1692uQtyfVBMkEun7xtEe_smE_UvH6N4", // Hier kommt euer directline secret key rein
        webSocket: false
      });

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
          directLine: directLine,
          styleOptions: {
            botAvatarBackgroundColor: 'rgba(0, 0, 0)',
            hideUploadButton: true,
            bubbleBackground: 'rgba(0, 0, 255, .1)',
            bubbleFromUserBackground: 'rgba(0, 255, 0, .1)',
            sendBoxButtonColor: 'rgba(255,153, 0, 1)',
            hideScrollToEndButton: true,
            sendBoxHeight: 70,
            bubbleMinHeight: 0,
            bubbleMaxWidth: 600,
          },
          webSpeechPonyfillFactory: await createHybridPonyfillFactory(),
          locale: 'en-US',
          store,
          overrideLocalizedStrings: {
            TEXT_INPUT_PLACEHOLDER: 'Hier kann ein personalisierter Text rein'
          }
        },
        document.getElementById('webchat')
      );


      directLine
        .postActivity({
          from: { id: "USER_ID", name: "USER_NAME" },
          name: "requestWelcomeDialog",
          type: "event",
          value: "token"
        })
        .subscribe(
          id => { console.log(`Posted activity, assigned ID ${id}`); },
          error => console.log(`Error posting activity ${error}`)
        );

      window.addEventListener('webchatincomingactivity', function ({ data }) {
        console.log(`Received an activity of type "${data.type}":`);
        if (data.hasOwnProperty("value")) {
          console.log("DataValue: " + data["value"]);

          var value_obj = JSON.parse(data["value"]);
          var endpoint = value_obj["endpoint"];
          delete value_obj["endpoint"];

          console.log("CALLING FUNCTION");
          console.log(endpoint)
          storemounted.dispatch('changeDataBot', { "endpoint": endpoint, "data": JSON.stringify(value_obj), "specs": storemounted.state.specs });
        }
      });

    })().catch(err => console.error(err));











    // (async function () {
    //   const res = await fetch(
    //     "https://webchat-mockbot.azurewebsites.net/directline/token",
    //     { method: "POST" }
    //   );
    //   const { token } = await res.json();

    //   const store = window.WebChat.createStore(
    //     {},
    //     ({ dispatch }) => (next) => (action) => {
    //       if (action.type === "DIRECT_LINE/INCOMING_ACTIVITY") {
    //         const event = new Event("webchatincomingactivity");

    //         event.data = action.payload.activity;
    //         window.dispatchEvent(event);
    //       }

    //       return next(action);
    //     }
    //   );

    //   window.WebChat.renderWebChat(
    //     {
    //       directLine: window.WebChat.createDirectLine({
    //         token: "hklRsvlqmDU.LlAinDiX1BF1692uQtyfVBMkEun7xtEe_smE_UvH6N4",
    //       }),
    //       store,
    //     },
    //     document.getElementById("webchat")
    //   );

    //   window.addEventListener("webchatincomingactivity", function ({ data }) {
    //     console.log(`Received an activity of type "${data.type}":`);
    //     if (data.hasOwnProperty("value")) {
    //       console.log("DataValue: " + data["value"]);

    //       var value_obj = JSON.parse(data["value"]);
    //       var endpoint = value_obj["endpoint"];
    //       delete value_obj["endpoint"];

    //       console.log("CALLING FUNCTION");
    //       console.log(endpoint);
    //       storemounted.dispatch("changeDataBot", {
    //         endpoint: endpoint,
    //         data: JSON.stringify(value_obj),
    //         specs: storemounted.state.specs,
    //       });
    //     }
    //   });

    //   document.querySelector("#webchat > *").focus();
    // })().catch((err) => console.error(err));
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
  height: 800px;
}
</style>