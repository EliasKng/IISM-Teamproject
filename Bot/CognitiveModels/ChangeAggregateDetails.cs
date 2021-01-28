//Abgeändert/neu schreiben
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.BotBuilderSamples
{
    public class ChangeAggregateDetails
    {
        //Name of the part to change (e.g. xAxis)
        public string visualizationPart { get; set; }
        //What should this part be changed to (e.g. sales). Can contain multiple entities ==> Ambiguity. At the end of the AmbiguityDialog the right one will be on the toValue[0]
        public string toAggregate { get; set; }
    }
}
