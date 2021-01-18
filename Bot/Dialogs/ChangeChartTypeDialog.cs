using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;

namespace Microsoft.BotBuilderSamples.Dialogs
{
    //This dialog is used, when a user wants to change a charttype to a new charttype.
    //It is determined, wheather the user made an underspecified task, or if there are even ambiguities
    public class ChangeChartTypeDialog : CancelAndHelpDialog
    {
        private const string DestinationStepMsgText = "Please choose a charttype from the list!";

        private readonly string[] _chartTypeOptions = new string[]
        {
            "barchart", "columnchart", "piechart", "scatterplot",
        };

        public ChangeChartTypeDialog() : base(nameof(ChangeChartTypeDialog))
        {
            HelpMsgText = "In this step type in: \"Change charttype to e.g. \"barchart\" or \"scatterplot\"\"";
            CancelMsgText = "Cancelling the change charttype Dialog";
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                ToCharttypeStepAsync,
                FinalStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> ToCharttypeStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            //Get the given Object from the step context
            var changeChartTypeDetails = (ChangeChartTypeDetails)stepContext.Options;

            if(changeChartTypeDetails.AmbiguousChartTypes?.Length > 1)
            {
                //We have ambiguities (more than one Entity) ==> ask the user with the AmbiguityDialog
                return await stepContext.BeginDialogAsync(nameof(AmbiguityDialog), changeChartTypeDetails.AmbiguousChartTypes, cancellationToken);
            } else if (changeChartTypeDetails.ToChartType == null)
            {
                //We have an underspecified task ==> get the missing information by letting the user choose from a list.
                var options = _chartTypeOptions.ToList();
                var promptOptions = new PromptOptions
                {
                    Prompt = MessageFactory.Text("Please choose an option from the list."),
                    RetryPrompt = MessageFactory.Text("You have to choose an option from the list."),
                    Choices = ChoiceFactory.ToChoices(options),
                };

                return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
            }
            return await stepContext.NextAsync(changeChartTypeDetails.ToChartType, cancellationToken);
        }

        //Confirm Change Charttype
        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            ConsoleWriter.WriteLineInfo("ResultTypeString: " + stepContext.Result.GetType().ToString());
            ConsoleWriter.WriteLineInfo("Result: " + stepContext.Result.ToString());
            var changeChartTypeDetails = (ChangeChartTypeDetails)stepContext.Options;
            //We are comming from a ChoicePromt/Ambiguity Dialog ==> Convert result to FoundCHoice
            if (stepContext.Result.GetType().ToString().Equals("Microsoft.Bot.Builder.Dialogs.Choices.FoundChoice"))
            {
                //Extract the picked result from the step-context
                var pickedChoice = (FoundChoice)stepContext.Result;
                var choiceText = pickedChoice.Value;

                //Set the result to the ChangeCharttypeDetails-Object
                changeChartTypeDetails.ToChartType = choiceText;
                changeChartTypeDetails.AmbiguousChartTypes = new string[] { choiceText };
            }
            //Now the Object is set right and we can print, what we want to change our charttype to
            ConsoleWriter.WriteLineInfo("Change Charttype to: " + changeChartTypeDetails.AmbiguousChartTypes[0]);
            
            //Send Request to API
            await BOT_Api.SendChangeChartTypeAsync(stepContext, changeChartTypeDetails.AmbiguousChartTypes[0]);
            
            return await stepContext.EndDialogAsync(changeChartTypeDetails,cancellationToken);
        }
    }
}