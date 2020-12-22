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
        public MainDialog(LuisRecognizer luisRecognizer,FilterDialog filterDialog, ChangeChartTypeDialog chartTypeDialog, AmbiguityDialog ambiguityDialog, ILogger<MainDialog> logger)
            : base(nameof(MainDialog))
        {
            _luisRecognizer = luisRecognizer;
            Logger = logger;

            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(filterDialog);
            AddDialog(chartTypeDialog);
            AddDialog(ambiguityDialog);
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                IntroStepAsync,
                ActStepAsync,
                FinalStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }


        //
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

        private async Task<DialogTurnResult> ActStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            //Brauchen wir nicht, weil wir davon ausgehen, dass LUIS konfiguriert ist.
            //if (!_luisRecognizer.IsConfigured)
            //{
            //    // LUIS is not configured, we just run the BookingDialog path with an empty BookingDetailsInstance.
            //    return await stepContext.BeginDialogAsync(nameof(BookingDialog), new BookingDetails(), cancellationToken);
            //}

            //Visualization
            //HIER WIRD LOUIS AUFGERUFEN
            // Call LUIS and gather any potential booking details. (Note the TurnContext has the response to the prompt.)
            var luisResult = await _luisRecognizer.RecognizeAsync<VisualizationInteraction>(stepContext.Context, cancellationToken);


            switch (luisResult.TopIntent().intent)
            {
                
                //VISUALIZATION ********************
                case VisualizationInteraction.Intent.ChangeChartType:

                    string[] chartTypeResults = luisResult.ToChartTypeEntity;

                    var changeChartTypeDetails = new ChangeChartTypeDetails()
                    {
                        AmbiguousChartTypes = chartTypeResults,
                        ToChartType = chartTypeResults?[0],
                    };                       

                    //Check if changechartTypeDetails is null (in the Dialog) and ask for information if it is null
                    return await stepContext.BeginDialogAsync(nameof(ChangeChartTypeDialog), changeChartTypeDetails, cancellationToken);

                case VisualizationInteraction.Intent.Filter:

                    string[] filterResults = luisResult.FilterTypeEntity;
                    var filterDetails = new FilterDetails()
                    {
                        multipleFilters = filterResults,
                    };
                    return await stepContext.BeginDialogAsync(nameof(FilterDialog), filterDetails, cancellationToken);

                case VisualizationInteraction.Intent.Nl4dv:

                    //Gets the whole message from the User to the bot out of the luis result
                    string nl4dvQuery = luisResult.Text;

                    ConsoleWriter.WriteLineInfo("nl4dvQuery: " + nl4dvQuery);

                    //Here we would have to call the NL4DV function in the event handler (in the Python project)
                    break;

                case VisualizationInteraction.Intent.ChangeVisualizationPart:

                    (string visualizationPartLuis, string[] toValueLuis) = luisResult.ChangeVisualizationPartEntities;
                    ConsoleWriter.WriteLineInfo("Change " + visualizationPartLuis + " to (first Value): " + toValueLuis[0]);

                    var changeVisualizationPartDetails = new ChangeVisualizationPartDetails
                    {
                        visualizationPart = visualizationPartLuis,
                        toValue = toValueLuis
                    };
                    break;


                default:
                    // Catch all for unhandled intents
                    var didntUnderstandMessageText = $"Sorry, I didn't get that. Please try asking in a different way (intent was {luisResult.TopIntent().intent})";
                    var didntUnderstandMessage = MessageFactory.Text(didntUnderstandMessageText, didntUnderstandMessageText, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(didntUnderstandMessage, cancellationToken);
                    break;
            }

            return await stepContext.NextAsync(null, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var promptMessage = "What else can I do for you?";
            return await stepContext.ReplaceDialogAsync(InitialDialogId, promptMessage, cancellationToken);
        }
    }
}
