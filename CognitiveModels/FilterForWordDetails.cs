namespace Microsoft.BotBuilderSamples
{
    //Hier werden alle Relevanten daten gespeichert, die gebraucht werden, wenn ein Metrischer Filter erstellt werden soll
    public class FilterForWordDetails
    {
        //Speichert Spalten, die genannt werden
        public string[] columnName { get; set; }

        //speicher Machine learned
        public string[] filterAttribute { get; set; }

        //Speichert das Land (z.B. Canada, Germany)
        public string[] country { get; set; }

        //Speichert segments, die genannt werden (bsp.: Small business, enterprise)
        public string[] segment { get; set; }

        //Speichert Produkte, die genannt werden
        public string[] product { get; set; }
    }
}