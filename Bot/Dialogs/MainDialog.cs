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
        public MainDialog(LuisRecognizer luisRecognizer,Nl4dvDialog nl4DvDialog, FilterForNumberDialog filterForNumberDialog, ChangeAggregateDialog changeAggregateDialog, ChangeVisualizationPartDialog changeVisualizationPartDialog, FilterForWordDialog filterForWordDialog, ChangeChartTypeDialog chartTypeDialog, AmbiguityDialog ambiguityDialog, ILogger<MainDialog> logger)
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
            AddDialog(nl4DvDialog);
            AddDialog(changeAggregateDialog);
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                IntroStepAsync,
                ActStepAsync,
                FinalStepAsync
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        //Say hello to the user and ask what can I help you with today
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

        //This function is called every time the user enters an input. The intent is determined in the switch case block. From there we call the needed Dialogs
        private async Task<DialogTurnResult> ActStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var luisResult = await _luisRecognizer.RecognizeAsync<VisualizationInteraction>(stepContext.Context, cancellationToken);


            switch (luisResult.TopIntent().intent)
            {
                //The user wants to change the charttype
                case VisualizationInteraction.Intent.ChangeChartType:

                    string[] chartTypeResults = luisResult.ToChartTypeEntity;

                    var changeChartTypeDetails = new ChangeChartTypeDetails()
                    {
                        AmbiguousChartTypes = chartTypeResults,
                        ToChartType = chartTypeResults?[0],
                    };                       

                    //Check if changechartTypeDetails is null (in the Dialog) and ask for information if it is null
                    return await stepContext.BeginDialogAsync(nameof(ChangeChartTypeDialog), changeChartTypeDetails, cancellationToken);

                // user wants to filter for country, product or segment (nominal Rows)
                case VisualizationInteraction.Intent.Filter:

                    (string[] columnnameLuis, string[] filterAttributeLuis, string[] countryLuis, string[] segmentLuis, string[] productLuis) = luisResult.FilterForWordEntities;

                    string usedColumn;

                    if(countryLuis!=null)
                    {
                        usedColumn = "Country";
                    } else if (segmentLuis!= null)
                    {
                        usedColumn = "Segment";
                    } else if (productLuis != null)
                    {
                        usedColumn = "Product";
                    } else
                    {
                        usedColumn = "Country";
                    }

                    ConsoleWriter.WriteLineInfo("Column: " + usedColumn);

                    var filterForWordDetails = new FilterForWordDetails
                    {
                        columnName = columnnameLuis,
                        filterAttribute = filterAttributeLuis,
                        country = countryLuis,
                        segment = segmentLuis,
                        product = productLuis,
                        usedColumn = usedColumn
                    };
                    return await stepContext.BeginDialogAsync(nameof(FilterForWordDialog), filterForWordDetails, cancellationToken);


                //user wants to filter for number (cardinal rows)
                case VisualizationInteraction.Intent.FilterForNumber:

                    (string[] columnNameLuis, string comparisonOperatorLuis, string filterNumberLuis) = luisResult.FilterForNumberEntities;

                    var filterForNumberDetails = new FilterForNumberDetails
                    {
                        columnName = columnNameLuis,
                        comparisonOperator = comparisonOperatorLuis,
                        filterNumber = filterNumberLuis
                    };
                    
                    return await stepContext.BeginDialogAsync(nameof(FilterForNumberDialog), filterForNumberDetails, cancellationToken);

                //user input is a complete query to be visualized -> send query to nl4dv
                case VisualizationInteraction.Intent.Nl4dv:
                    //Gets the whole message from the User to the bot out of the luis result
                    string nl4dvQuery = luisResult.Text;

                    (string queryText, string[] chartType, string[] axis1, string[] axis2) = luisResult.Nl4dvEntities;

                    var nl4dvQueryDetails = new Nl4dvQueryDetails
                    {
                        queryText = queryText,
                        chartType = chartType,
                        axis1 = axis1,
                        axis2 = axis2
                    };

                    ConsoleWriter.WriteLineInfo("nl4dvQuery: " + nl4dvQuery);

                    return await stepContext.BeginDialogAsync(nameof(Nl4dvDialog), nl4dvQueryDetails, cancellationToken);


                    //Here we would have to call the NL4DV function in the event handler (in the Python project)
                    //BOT_Api.SendNL4DV(nl4dvQuery);
                    break;

                // user wants to change e.g. legend or y-axis
                case VisualizationInteraction.Intent.ChangeVisualizationPart:

                    (string visualizationPartLuis, string[] toValueLuis) = luisResult.ChangeVisualizationPartEntities;
                    

                    var changeVisualizationPartDetails = new ChangeVisualizationPartDetails
                    {
                        visualizationPart = visualizationPartLuis,
                        toValue = toValueLuis
                    };

                    return await stepContext.BeginDialogAsync(nameof(ChangeVisualizationPartDialog), changeVisualizationPartDetails, cancellationToken);

                // user wants to change the aggregate of an axis
                case VisualizationInteraction.Intent.ChangeAggregate:

                    (string toVisPartLuis, string toAggregateLuis) = luisResult.ChangeAggregateEntities;


                    var changeAggregateDetails = new ChangeAggregateDetails
                    {
                        visualizationPart = toVisPartLuis,
                        toAggregate = toAggregateLuis
                    };

                    return await stepContext.BeginDialogAsync(nameof(ChangeAggregateDialog), changeAggregateDetails, cancellationToken);

                case VisualizationInteraction.Intent.ClearFilter:
                    await BOT_Api.SendClearFilter(stepContext);
                    break;

                case VisualizationInteraction.Intent.Help:
                    var helpMessageText = $"By starting your query with \"Show me\" you can enter a natural language query, e.g. \"Show me a distribution of sales\". After that, you can tell me to change the charttype, an attribute or even filter for attributes. Clear those filters by telling me e.g. \"clear filter\". You can also reset your visualization or simply type in another \"Show me\" query.";
                    var helpMessage = MessageFactory.Text(helpMessageText, helpMessageText, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(helpMessage, cancellationToken);
                    break;


                //intent not recognized
                default:
                    // Catch all for unhandled intents
                    var didntUnderstandMessageText = $"Sorry, I didn't get that. Please try asking in a different way (intent was {luisResult.TopIntent().intent})";
                    var didntUnderstandMessage = MessageFactory.Text(didntUnderstandMessageText, didntUnderstandMessageText, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(didntUnderstandMessage, cancellationToken);
                    break;
            }

            return await stepContext.NextAsync(null, cancellationToken);
        }

        //Dialog done -> start new from beginning
        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var promptMessage = "What else can I do for you?";
            return await stepContext.ReplaceDialogAsync(InitialDialogId, promptMessage, cancellationToken);
        }
    }
}
