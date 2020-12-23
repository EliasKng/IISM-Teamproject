using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;

namespace Microsoft.BotBuilderSamples
{
    //Diese Klasse speichert die JSON antwort von Luis
    //Die klasse muss genau zu den JSON Feedback passen, damit der RecognizeAsync aus der Flightbookingrecognizer.cs das LUIS Feedback richtig parsen kann.
    public partial class VisualizationInteraction : IRecognizerConvert
    {
        public string Text;
        public string AlteredText;
        public enum Intent
        {
            ChangeChartType,
            Filter,
            FilterForNumber,
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

            //Filtering for numbers
            public string[] number;
            public string[][] comparisonOperator;

            // Instance: Hier wird gespeichert, welchen Text der Benutzer in der Query tatsächlich eingegeben hat (z.B. bar-chart aus Entities._instance.chartType[0].Text)
            public class _Instance
            {
                public InstanceData[] chartType;
                public InstanceData[] filterAttribute;
                public InstanceData[] visualitationPart;
                public InstanceData[] financialSampleColumnName;
                public InstanceData[] country;
                public InstanceData[] segment;
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

        //Gibt die relevanten Daten für den FilterForNumber-Intent zurück (Spalte, Operator und Zahl)
        public (string[], string, string) FilterForNumberEntities
        {
            get
            {
                //Hole Den Spaltenname
                string[] columnName = Entities?.financialSampleColumnName?[0];
                //Hole den Operator
                string comparisonOperator = Entities?.comparisonOperator?[0]?[0];
                //Hole die Zahl
                string filterNumber = Entities.number?[0];


                //Gib beide Werte zurück
                return (columnName, comparisonOperator, filterNumber);
            }
        }

        //Gibt die relevanten Daten für den Change Charttype-Intent zurück 
        public string[] ToChartTypeEntity
        {
            get
            {
                string[][] toChartValue = Entities?.chartType;
                return toChartValue?[0];
            }
        }

        //Gibt die relevanten Daten für den Filter-Intent zurück 
        public string[] FilterTypeEntity
        {
            get
            {
             
                //Filter for country
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

        //Gibt die relevanten Daten für den ChangeVisualizationPart-Intent zurück 
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
    }
}