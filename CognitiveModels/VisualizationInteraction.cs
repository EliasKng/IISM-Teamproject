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
            Filter,
            None
        };
        public Dictionary<Intent, IntentScore> Intents;

        public class _Entities
        {
            // Lists: Hier drin wird das erkannte entity gespeichert (z.B. barchart)
            public string[][] chartType;
            public string[][] filterType;

            // Instance: Hier wird gespeichert, welchen Text der Benutzer in der Query tatsächlich eingegeben hat (z.B. bar-chart aus Entities._instance.chartType[0].Text)
            public class _Instance
            {
                public InstanceData[] chartType;
                public InstanceData[] filterType;
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

        public string[] ToChartTypeEntity
        {
            get
            {
                //Nimm das erste erkannte Entity heraus und gebe es zurück
                string[][] toChartValue = Entities?.chartType;
                ConsoleWriter.WriteLineInfo("FirstChartType From Luis Result: " + toChartValue?[0]?[0]);
                return toChartValue?[0];

                //ConsoleWriter.WriteLineInfo(toChartValue?[0]?[1]);

                //***********Hier müsste dann ein Ambiguity-Dialog erstellt werden, falls das feldtoChartValue[0] länger als 1 ist, wo dann abgefragt wird, welchen entity man tatsächlich wollte
                //***********Hier müsste dann ein Ambiguity-Dialog erstellt werden, falls das feldtoChartValue[] länger als 1 ist, wo dann abgefragt wird, welchen entity man tatsächlich wollte

                //Das hier wäre das zweite erkannte entity (falls es existiert (z.B: change charttype to barchart and scatterplot) dann wäre scatterplot das zweite
                //string toChartValue = Entities.chartType[1][0];

                ////ruft alle gefundenen entities auf
                //for (int i = 0; i < Entities.chartType.Length; i++)
                //{
                //    Console.WriteLine(Entities.chartType[i][0]);
                //}

                //Hier drin steht das erste erkannte entity, aber in Textform, so wie der Nutzer es tatsächelich eingegeben hat (z.B. bar-chart)
                //string toChartValue = Entities._instance.chartType[0].Text;


            }
        }

        public string[] filterTypeEntity
        {
            get
            {
                string[][] filterValue = Entities?.filterType;

                //Innere for schleife würde Ambiguität überprüfen
                //for (int i = 0; i < Entities.country.Length; i++)
                //{
                //    for (int j = 0; j < Entities.country[i].Length; j++)
                //    {
                //        ConsoleWriter.WriteLineInfo(Entities.country[i][j]);
                //    }
                //}

                for (int i = 0; i < Entities?.filterType?.Length; i++)
                {
                    ConsoleWriter.WriteLineInfo(Entities.filterType[i][0]);
                    filterValue[0][0] = String.Join(",", Entities.filterType[i][0]);
                }
                ConsoleWriter.WriteLineInfo(filterValue?[0]?[0]);

                return filterValue?[0];
            }
        }

        //// This value will be a TIMEX. And we are only interested in a Date so grab the first result and drop the Time part.
        //// TIMEX is a format that represents DateTime expressions that include some ambiguity. e.g. missing a Year.
        //public string TravelDate
        //    => Entities.datetime?.FirstOrDefault()?.Expressions.FirstOrDefault()?.Split('T')[0];
    }
}