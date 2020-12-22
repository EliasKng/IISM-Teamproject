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
            Nl4dv,
            ChangeVisualizationPart,
            None
        };
        public Dictionary<Intent, IntentScore> Intents;

        public class _Entities
        {
            // Lists: Hier drin wird das erkannte entity gespeichert (z.B. barchart)
            public string[][] chartType;
            public string[] filterAttribute;
            public string[][] visualizationPart;


            //Attributes which depend on the dataset
            public string[][] country;
            public string[][] financialSampleColumnName;
            public string[][] segment;

            // Instance: Hier wird gespeichert, welchen Text der Benutzer in der Query tatsächlich eingegeben hat (z.B. bar-chart aus Entities._instance.chartType[0].Text)
            public class _Instance
            {
                public InstanceData[] chartType;
                public InstanceData[] filterAttribute;
                public InstanceData[] visualitationPart;
                public InstanceData[] financialSampleColumnName;
                public InstanceData[] country;
                public InstanceData[] segment;
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
                //ConsoleWriter.WriteLineInfo("FirstChartType From Luis Result: " + toChartValue?[0]?[0]);
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

        public string[] FilterTypeEntity
        {
            get
            {
             
                if ((string.Equals(Entities?.filterAttribute?[0].ToLower(), "country")) || (string.Equals(Entities?.filterAttribute?[0].ToLower(), "countries"))) {
                    return Entities?.filterAttribute;
                }
                
                //ADD ALL ENTITIES THAT CAN BE FILTERED FOR, ONE OF THOSE ENTITIES HAS TO BE != NULL 
                if ((Entities?.country?[0] == null) && (Entities?.segment?[0] == null))
                {
                    return null; 
                }
                
                for(int i = 0; i < Entities?.filterAttribute?.Length; i++)
                {
                    ConsoleWriter.WriteLineInfo("Attribute " + i + ": " + Entities?.filterAttribute[i]);
                }
                return Entities?.filterAttribute;
            }
        }

        public (string, string[]) ChangeVisualizationPartEntities
        {
            get
            {
                //Nimm das erste erkannte Entity heraus und gebe es zurück (visualizationPart)
                string visualizationPart = Entities?.visualizationPart?[0]?[0];
                //Nimm das erste erkannte Entity heraus und gebe es zurück (toValue (also zu was es geändert werden soll))
                string[] toValue = Entities?.financialSampleColumnName?[0];
                ConsoleWriter.WriteLineInfo("Get ChangeVisualizationPartEntities: Change " + visualizationPart + "To " + toValue);

                //Gib beide Werte zurück
                return (visualizationPart, toValue);
            }
        }

        //// This value will be a TIMEX. And we are only interested in a Date so grab the first result and drop the Time part.
        //// TIMEX is a format that represents DateTime expressions that include some ambiguity. e.g. missing a Year.
        //public string TravelDate
        //    => Entities.datetime?.FirstOrDefault()?.Expressions.FirstOrDefault()?.Split('T')[0];
    }
}