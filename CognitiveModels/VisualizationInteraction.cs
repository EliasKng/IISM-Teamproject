using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;

namespace Microsoft.BotBuilderSamples
{
    //Die klasse muss genau zu den JSON Feedback passen, damit der RecognizeAsync aus der Flightbookingrecognizer.cs das LUIS Feedback richtig parsen kann.
    public partial class VisualizationInteraction : IRecognizerConvert
    {
        public string Text;
        public string AlteredText;
        public enum Intent
        {
            ChangeChartType,
            None
        };
        public Dictionary<Intent, IntentScore> Intents;

        public class _Entities
        {
            // Lists
            public string[][] chartType;


            ////Instances
            //public class _InstanceChartType
            //{
            //    public InstanceData[] chartType;
            //}
            //public class chartTypeClass
            //{
            //    public string[][] chartType;
            //    [JsonProperty("$instance")]
            //    public _InstanceChartType _instance;
            //}
            //public chartTypeClass[] chartType2222;


            // Instance
            public class _Instance
            {
                public InstanceData[] chartType;
            }
            [JsonProperty("$instance")]
            public _Instance _instance;
        }
        public _Entities Entities;

        [JsonExtensionData(ReadData = true, WriteData = true)]
        public IDictionary<string, object> Properties { get; set; }

        public void Convert(dynamic result)
        {
            var app = JsonConvert.DeserializeObject<VisualizationInteraction>(JsonConvert.SerializeObject(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            Text = app.Text;
            AlteredText = app.AlteredText;
            Intents = app.Intents;
            Entities = app.Entities;
            Properties = app.Properties;
        }

        public (Intent intent, double score) TopIntent()
        {
            Intent maxIntent = Intent.None;
            var max = 0.0;
            foreach (var entry in Intents)
            {
                if (entry.Value.Score > max)
                {
                    maxIntent = entry.Key;
                    max = entry.Value.Score.Value;
                }
            }
            return (maxIntent, max);
        }
    }
}