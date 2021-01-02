using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;

namespace Microsoft.BotBuilderSamples
{
    // class saves JSON answer from LUIS
    // class has to exactlz fit JSON feedback, so that RecognizeAsync can parse LUIS Feedback perfectly
    public partial class VisualizationInteraction : IRecognizerConvert
    {
        public string Text;
        public string AlteredText;
        public enum Intent
        {
            ChangeChartType,
            Filter,
            FilterForNumber,
            FilterForWord,
            Nl4dv,
            ChangeVisualizationPart,
            None
        };
        public Dictionary<Intent, IntentScore> Intents;

        public class _Entities
        {
            // Lists: save recognized entity (e.g. barchart)
            public string[][] chartType;
            public string[] filterAttribute;
            public string[][] visualizationPart;


            //Attributes which depend on the dataset
            public string[][] country;
            public string[][] financialSampleColumnName;
            public string[][] segment;
            public string[][] product;

            //Filtering for numbers
            public string[] number;
            public string[][] comparisonOperator;

            // Instance: saves what text user has aqtually put into the query (e.g. bar-chart from Entities._instance.chartType[0].Text)
            public class _Instance
            {
                public InstanceData[] chartType;
                public InstanceData[] filterAttribute;
                public InstanceData[] visualitationPart;
                public InstanceData[] financialSampleColumnName;
                public InstanceData[] country;
                public InstanceData[] segment;
                public InstanceData[] product;
                public InstanceData[] number;
                public InstanceData[] comparisonOperator;
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

        // returns relevant data for filterForNumber-Intent (column, Operator and number)
        public (string[], string, string) FilterForNumberEntities
        {
            get
            {
                //Hget column name
                string[] columnName = Entities?.financialSampleColumnName?[0];
                //get operator
                string comparisonOperator = Entities?.comparisonOperator?[0]?[0];
                //get number
                string filterNumber = Entities.number?[0];


                //Gib beide Werte zurück
                return (columnName, comparisonOperator, filterNumber);
            }
        }

        //returns relevant data for filterForWord/Filter-Intent 
        public (string[], string[], string[], string[], string[]) FilterForWordEntities
        {
            get
            {
                string[] filterAttribute = Entities?.filterAttribute;
                string[] columnName = Entities?.financialSampleColumnName?[0];
                string[] country = Entities?.country?[0];
                string[] segment = Entities?.segment?[0];
                string[] product = Entities?.product?[0];

                return (filterAttribute, columnName, country, segment, product);
            }
        }


        //returns relevant data for ChangeChartType-Intent
        public string[] ToChartTypeEntity
        {
            get
            {
                string[][] toChartValue = Entities?.chartType;
                return toChartValue?[0];
            }
        }

        //returns relevant data for ChangeVisualizationPart-Intent
        public (string, string[]) ChangeVisualizationPartEntities
        {
            get
            {
                //take first entity recognized and return it (visualizationPart)
                string visualizationPart = Entities?.visualizationPart?[0]?[0];
                //take first entity recognized and return it (toValue (what should visualizationPart be changed to))
                string[] toValue = Entities?.financialSampleColumnName?[0];
                ConsoleWriter.WriteLineInfo("Get ChangeVisualizationPartEntities: Change " + visualizationPart + "To " + toValue);

                //Gib beide Werte zurück
                return (visualizationPart, toValue);
            }
        }
    }
}