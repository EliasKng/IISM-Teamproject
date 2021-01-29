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
    public class ChangeAggregateDialog : CancelAndHelpDialog
    {

        public ChangeAggregateDialog() : base(nameof(ChangeAggregateDialog))
        {
            HelpMsgText = "In this step type in e.g.: \"Change aggregate of yaxis to e.g. \"sum\" or \"avg\"\"";
            CancelMsgText = "Cancelling the change aggregate Dialog";
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
            var changeAggregateDetails = (ChangeAggregateDetails)stepContext.Options;
            if (changeAggregateDetails.toAggregate == null)
            {
                //There is information missing to execute the task ==> tell the user how to do it in the right way
                string message = "I could not recognize what aggregate you want to apply. Say something like \"change aggregate of xAxis to sum\"";

                var cancelMessage = MessageFactory.Text(message, CancelMsgText, InputHints.IgnoringInput);
                await stepContext.Context.SendActivityAsync(cancelMessage, cancellationToken);
                return await stepContext.CancelAllDialogsAsync(cancellationToken);
            } else if (changeAggregateDetails.visualizationPart == null)
            {
                //There is information missing to execute the task ==> tell the user how to do it in the right way
                string message = "I could not recognize what axis you want to apply the aggregate " + changeAggregateDetails.toAggregate +" to. Say something like \"change xAxis to sum\"";

                var cancelMessage = MessageFactory.Text(message, CancelMsgText, InputHints.IgnoringInput);
                await stepContext.Context.SendActivityAsync(cancelMessage, cancellationToken);
                return await stepContext.CancelAllDialogsAsync(cancellationToken);
            }

            return await stepContext.NextAsync(changeAggregateDetails, cancellationToken);
        }

        //Confirm task
        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var changeAggregateDetails = (ChangeAggregateDetails)stepContext.Options;

            //Now the Object is set right and we can print, what we want to change our charttype to
            ConsoleWriter.WriteLineInfo("Change aggregate of " + changeAggregateDetails.visualizationPart + " to: " + changeAggregateDetails.toAggregate);

            await BOT_Api.SendChangeAggregate(stepContext, changeAggregateDetails.visualizationPart, changeAggregateDetails.toAggregate);

            return await stepContext.EndDialogAsync(changeAggregateDetails, cancellationToken);
        }
    }
}