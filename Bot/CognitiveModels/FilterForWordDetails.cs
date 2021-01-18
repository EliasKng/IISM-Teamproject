namespace Microsoft.BotBuilderSamples
{
    //all relevant information needed for filtering for words
    public class FilterForWordDetails
    {
        //all entities
        public string[] columnName { get; set; }

        //Machine learned entities
        public string[] filterAttribute { get; set; }

        //Saves the countries entered
        public string[] country { get; set; }

        //saves segments entered
        public string[] segment { get; set; }

        //saves entered products
        public string[] product { get; set; }
        //saves determined colum (which was used)
        public string usedColumn { get; set; }
    }
}