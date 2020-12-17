using Newtonsoft.Json;
using System;
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
            // Lists: Hier drin wird das erkannte entity gespeichert (z.B. barchart)
            public string[][] chartType;

            // Instance: Hier wird gespeichert, welchen Text der Benutzer in der Query tatsächlich eingegeben hat (z.B. bar-chart aus Entities._instance.chartType[0].Text)
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

        //public (string To, string Airport) ToEntities
        //{
        //    get
        //    {
        //        var toValue = Entities?._instance?.To?.FirstOrDefault()?.Text;
        //        var toAirportValue = Entities?.To?.FirstOrDefault()?.Airport?.FirstOrDefault()?.FirstOrDefault();
        //        return (toValue, toAirportValue);
        //    }
        //}

        public string ToChartTypeEntity
        {
            get
            {
                //Nimm das erste erkannte Entity heraus und gebe es zurück
                string toChartValue = Entities?.chartType?[0][0];

                //Das hier wäre das zweite erkannte entity (falls es existiert (z.B: change charttype to barchart and scatterplot) dann wäre scatterplot das zweite
                //string toChartValue = Entities.chartType[1][0];

                //Hier drin steht das erste erkannte entity, aber in Textform, so wie der Nutzer es tatsächelich eingegeben hat (z.B. bar-chart)
                //string toChartValue = Entities._instance.chartType[0].Text;

                return toChartValue;
            }
        }

        //// This value will be a TIMEX. And we are only interested in a Date so grab the first result and drop the Time part.
        //// TIMEX is a format that represents DateTime expressions that include some ambiguity. e.g. missing a Year.
        //public string TravelDate
        //    => Entities.datetime?.FirstOrDefault()?.Expressions.FirstOrDefault()?.Split('T')[0];
    }
}