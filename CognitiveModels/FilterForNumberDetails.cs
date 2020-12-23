namespace Microsoft.BotBuilderSamples
{
    //Hier werden alle Relevanten daten gespeichert, die gebraucht werden, wenn ein Metrischer Filter erstellt werden soll
    public class FilterForNumberDetails
    {
        //Speichert alle spalten die infrage kommen (ambiguity)
        public string[] columnName { get; set; }

        //Speichert den Operator (z.B. > oder >= oder = oder <)
        public string comparisonOperator { get; set; }
        //Speichert die Zahl, die mit dem Operator verbunden ist.
        public string filterNumber { get; set; }
    }
}