namespace Microsoft.BotBuilderSamples
{
    //saves all relevant information needed for a metric filter
    public class FilterForNumberDetails
    {
        //Saves all input that are worth considering including ambiguities
        public string[] columnName { get; set; }

        //Saves the operator e.g. >, <, >= or <= 
        public string comparisonOperator { get; set; }

        //Saves the number that comes with the operator
        public string filterNumber { get; set; }
    }
}