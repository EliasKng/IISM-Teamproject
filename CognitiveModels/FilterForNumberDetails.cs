namespace Microsoft.BotBuilderSamples
{
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