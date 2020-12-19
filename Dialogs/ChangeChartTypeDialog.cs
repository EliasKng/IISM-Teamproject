using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;

namespace Microsoft.BotBuilderSamples.Dialogs
{
    public class ChangeChartTypeDialog : CancelAndHelpDialog
    {
        //Zu dem CHarttyp wollen wir wechseln
        private const string DestinationStepMsgText = "Please choose a charttype from the list!";

        private readonly string[] _chartTypeOptions = new string[]
        {
            "barchart", "columnchart", "piechart", "scatterplot",
        };

        public ChangeChartTypeDialog()
            : base(nameof(ChangeChartTypeDialog))
        {
            HelpMsgText = "In this step type in: \"Change charttype to e.g. \"barchart\" or \"scatterplot\"\"";
            CancelMsgText = "Cancelling the change charttype Dialog";
            //AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));
            AddDialog(new DateResolverDialog());
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                DestinationStepAsync,
                FinalStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        //Hier findet er raus, zu welchem Charttyp wir wechseln wollen
        private async Task<DialogTurnResult> DestinationStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var changeChartTypeDetails = (ChangeChartTypeDetails)stepContext.Options;

            

            if(changeChartTypeDetails.AmbiguousChartTypes?.Length > 1)
            {
                //We have ambiguities (more than one Entity) ==> ask the user with the AmbiguityDialog
                return await stepContext.BeginDialogAsync(nameof(AmbiguityDialog), changeChartTypeDetails, cancellationToken);
            } else if (changeChartTypeDetails.ToChartType == null)
            {
                var options = _chartTypeOptions.ToList();
                var promptOptions = new PromptOptions
                {
                    Prompt = MessageFactory.Text("Please choose an option from the list."),
                    RetryPrompt = MessageFactory.Text("You have to choose an option from the list."),
                    Choices = ChoiceFactory.ToChoices(options),
                };

                return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);

                //var promptMessage = MessageFactory.Text(DestinationStepMsgText, DestinationStepMsgText, InputHints.ExpectingInput);
                // return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }
            return await stepContext.NextAsync(changeChartTypeDetails.ToChartType, cancellationToken);
        }

        //Hier wird bestätigt, dass wohin gewechselt wurde
        //WaterfallStepContext wird von vorherigem Step übernommen
        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var changeChartTypeDetails = (ChangeChartTypeDetails)stepContext.Options;
            //We are comming from a ChoicePromt ==> Convert result to FoundCHoice
            if (stepContext.Result.GetType().ToString().Equals("Microsoft.Bot.Builder.Dialogs.Choices.FoundChoice"))
            {
                //Extract the picked result from the step-context
                var pickedChoice = (FoundChoice)stepContext.Result;
                var choiceText = pickedChoice.Value;

                //Set the result to the ChangeCharttypeDetails-Object
                changeChartTypeDetails.ToChartType = choiceText;
                changeChartTypeDetails.AmbiguousChartTypes = new string[] { choiceText };
            }
            //We probably come from the AMbiguity Dialog which returns a ChangeChartTypeDetails Object ==> get the Object if yes
            else if (stepContext.Result.GetType().ToString().Equals("Microsoft.BotBuilderSamples.ChangeChartTypeDetails")) 
            {
                changeChartTypeDetails = (ChangeChartTypeDetails) stepContext.Result;
            }
            //Now the Object is set right and we can print, what we want to change our charttype to
            ConsoleWriter.WriteLineInfo("Change Charttype to: " + changeChartTypeDetails.AmbiguousChartTypes[0]);
            return await stepContext.EndDialogAsync(changeChartTypeDetails,cancellationToken);
        }
    }
}