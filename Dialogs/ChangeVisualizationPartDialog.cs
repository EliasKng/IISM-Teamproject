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
    public class ChangeVisualizationPartDialog : CancelAndHelpDialog
    {

        public ChangeVisualizationPartDialog() : base(nameof(ChangeVisualizationPartDialog))
        {
            HelpMsgText = "In this step type in e.g.: \"Change x-Axis to e.g. \"sales\" or \"profit\"\"";
            CancelMsgText = "Cancelling the change visualization part Dialog";
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                FirstStepAsync,
                FinalStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> FirstStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var changeVisualizationPartDetails = (ChangeVisualizationPartDetails)stepContext.Options;
            if (changeVisualizationPartDetails.toValue?.Length > 1)
            {
                //We have ambiguities (more than one Entity) ==> ask the user with the AmbiguityDialog
                return await stepContext.BeginDialogAsync(nameof(AmbiguityDialog), changeVisualizationPartDetails.toValue, cancellationToken);
            }
            else if (changeVisualizationPartDetails.toValue == null)
            {
                //There is information missing to execute the task ==> tell the user how to do it in the right way
                string message = "I could not regonize what Column you want to change that part to. Please say something like \"change xAxis to Sales\"";

                var cancelMessage = MessageFactory.Text(message, CancelMsgText, InputHints.IgnoringInput);
                await stepContext.Context.SendActivityAsync(cancelMessage, cancellationToken);
                return await stepContext.CancelAllDialogsAsync(cancellationToken);
            }

            //It is not null and length is not larger than one ==> take the first one
            return await stepContext.NextAsync(changeVisualizationPartDetails.toValue[0], cancellationToken);
        }

        //Confirm task
        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var changeVisualizationPartDetails = (ChangeVisualizationPartDetails)stepContext.Options;
            //We are comming from a ChoicePromt ==> Convert result to FoundCHoice
            if (stepContext.Result.GetType().ToString().Equals("Microsoft.Bot.Builder.Dialogs.Choices.FoundChoice"))
            {
                //Extract the picked result from the step-context
                var pickedChoice = (FoundChoice)stepContext.Result;
                var choiceText = pickedChoice.Value;

                //Set the result to the ChangeCharttypeDetails-Object
                changeVisualizationPartDetails.toValue = new string[] { choiceText };
            }
            //Now the Object is set right and we can print, what we want to change our charttype to
            ConsoleWriter.WriteLineInfo("Change " + changeVisualizationPartDetails.visualizationPart + " to (first Value): " + changeVisualizationPartDetails.toValue[0]);
            return await stepContext.EndDialogAsync(changeVisualizationPartDetails, cancellationToken);
        }
    }
}