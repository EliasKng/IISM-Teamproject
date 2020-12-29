//Neu schreiben / abändern
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;

namespace Microsoft.BotBuilderSamples.Dialogs
{ 
    public class MainDialog : ComponentDialog
    {
        private readonly LuisRecognizer _luisRecognizer;
        protected readonly ILogger Logger;

        // Dependency injection uses this constructor to instantiate MainDialog
        public MainDialog(LuisRecognizer luisRecognizer, FilterForNumberDialog filterForNumberDialog, ChangeVisualizationPartDialog changeVisualizationPartDialog, FilterForWordDialog filterForWordDialog, ChangeChartTypeDialog chartTypeDialog, AmbiguityDialog ambiguityDialog, ILogger<MainDialog> logger)
            : base(nameof(MainDialog))
        {
            _luisRecognizer = luisRecognizer;
            Logger = logger;

            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(filterForWordDialog);
            AddDialog(chartTypeDialog);
            AddDialog(changeVisualizationPartDialog);
            AddDialog(ambiguityDialog);
            AddDialog(filterForNumberDialog);
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                IntroStepAsync,
                ActStepAsync,
                FinalStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> IntroStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (!_luisRecognizer.IsConfigured)
            {
                await stepContext.Context.SendActivityAsync(
                    MessageFactory.Text("NOTE: LUIS is not configured. To enable all capabilities, add 'LuisAppId', 'LuisAPIKey' and 'LuisAPIHostName' to the appsettings.json file.", inputHint: InputHints.IgnoringInput), cancellationToken);

                return await stepContext.NextAsync(null, cancellationToken);
            }

            // Use the text provided in FinalStepAsync or the default if it is the first time.
            var messageText = stepContext.Options?.ToString() ?? "What can I help you with today?";
            var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        //Hiere wird das Luis Result ausgewertet. Daraus wird der jeweilige Intend abgeleitet (switch case) und dann je nach case anders vorgegangen
        private async Task<DialogTurnResult> ActStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var luisResult = await _luisRecognizer.RecognizeAsync<VisualizationInteraction>(stepContext.Context, cancellationToken);


            switch (luisResult.TopIntent().intent)
            {
                //Der Nutzer will den Visualisierungstypen ändern
                case VisualizationInteraction.Intent.ChangeChartType:

                    string[] chartTypeResults = luisResult.ToChartTypeEntity;

                    var changeChartTypeDetails = new ChangeChartTypeDetails()
                    {
                        AmbiguousChartTypes = chartTypeResults,
                        ToChartType = chartTypeResults?[0],
                    };                       

                    //Check if changechartTypeDetails is null (in the Dialog) and ask for information if it is null
                    return await stepContext.BeginDialogAsync(nameof(ChangeChartTypeDialog), changeChartTypeDetails, cancellationToken);

                //Der Nuter will Filtern (nominal/ordinal)
                //case VisualizationInteraction.Intent.Filter:

                //    string[] filterResults = luisResult.FilterTypeEntity;
                //    if (filterResults is null)
                //    {
                //        var falseFilterMessageText = $"Sorry, I didn't get that. Please enter a valid {luisResult.TopIntent().intent}-request.";
                //        var falseFilterMessage = MessageFactory.Text(falseFilterMessageText, falseFilterMessageText, InputHints.IgnoringInput);
                //        await stepContext.Context.SendActivityAsync(falseFilterMessage, cancellationToken);
                //        break;
                //    }

                //    var filterDetails = new FilterDetails()
                //    {
                //        multipleFilters = filterResults,
                //    };
                //    return await stepContext.BeginDialogAsync(nameof(FilterDialog), filterDetails, cancellationToken);

                case VisualizationInteraction.Intent.Filter:

                    (string[] columnnameLuis, string[] filterAttributeLuis, string[] countryLuis, string[] segmentLuis, string[] productLuis) = luisResult.FilterForWordEntities;
                    var filterForWordDetails = new FilterForWordDetails
                    {
                        columnName = columnnameLuis,
                        filterAttribute = filterAttributeLuis,
                        country = countryLuis,
                        segment = segmentLuis,
                        product = productLuis
                    };
                    return await stepContext.BeginDialogAsync(nameof(FilterForWordDialog), filterForWordDetails, cancellationToken);


                //Der Nutzer will Filtern (Metrisch (kardinal))
                case VisualizationInteraction.Intent.FilterForNumber:

                    (string[] columnNameLuis, string comparisonOperatorLuis, string filterNumberLuis) = luisResult.FilterForNumberEntities;

                    var filterForNumberDetails = new FilterForNumberDetails
                    {
                        columnName = columnNameLuis,
                        comparisonOperator = comparisonOperatorLuis,
                        filterNumber = filterNumberLuis
                    };
                    
                    return await stepContext.BeginDialogAsync(nameof(FilterForNumberDialog), filterForNumberDetails, cancellationToken);

                //Der Nutzer hat eine Komplette Query zur erstellung einer Visualisierung eingegeben ==> sende diese an Nl4DV
                case VisualizationInteraction.Intent.Nl4dv:

                    //Gets the whole message from the User to the bot out of the luis result
                    string nl4dvQuery = luisResult.Text;

                    ConsoleWriter.WriteLineInfo("nl4dvQuery: " + nl4dvQuery);

                    //Here we would have to call the NL4DV function in the event handler (in the Python project)
                    break;

                //Der nutzer will z.B. die Legende oder y-Achse ändern (eine andere Spalte darauf setzen)
                case VisualizationInteraction.Intent.ChangeVisualizationPart:

                    (string visualizationPartLuis, string[] toValueLuis) = luisResult.ChangeVisualizationPartEntities;
                    

                    var changeVisualizationPartDetails = new ChangeVisualizationPartDetails
                    {
                        visualizationPart = visualizationPartLuis,
                        toValue = toValueLuis
                    };

                    return await stepContext.BeginDialogAsync(nameof(ChangeVisualizationPartDialog), changeVisualizationPartDetails, cancellationToken);

                //Der intent wurde nicht erkannt
                default:
                    // Catch all for unhandled intents
                    var didntUnderstandMessageText = $"Sorry, I didn't get that. Please try asking in a different way (intent was {luisResult.TopIntent().intent})";
                    var didntUnderstandMessage = MessageFactory.Text(didntUnderstandMessageText, didntUnderstandMessageText, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(didntUnderstandMessage, cancellationToken);
                    break;
            }

            return await stepContext.NextAsync(null, cancellationToken);
        }

        //Wir haveb den Dialog durchlaufen ==> von vorne starten
        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var promptMessage = "What else can I do for you?";
            return await stepContext.ReplaceDialogAsync(InitialDialogId, promptMessage, cancellationToken);
        }
    }
}
