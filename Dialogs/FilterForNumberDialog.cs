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
    public class FilterForNumberDialog : CancelAndHelpDialog
    {

        public FilterForNumberDialog() : base(nameof(FilterForNumberDialog))
        {
            HelpMsgText = "In this step type in e.g.: \"Filter for Sales >= 300\"";
            CancelMsgText = "Cancelling the Filter for Number Dialog";
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

        //Hier findet checkt er, ob Daten fehlen oder wir ambiguitäten vorliegen haben
        private async Task<DialogTurnResult> FirstStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var filterForNumberDetails = (FilterForNumberDetails)stepContext.Options;
            if (filterForNumberDetails.columnName?.Length > 1) //Do we have Ambiguities?
            {
                //We have ambiguities (more than one Entity) ==> ask the user with the AmbiguityDialog
                return await stepContext.BeginDialogAsync(nameof(AmbiguityDialog), filterForNumberDetails.columnName, cancellationToken);
            }
            else if (filterForNumberDetails.columnName == null)//Rowname is missing
            {
                string message = "I could not recognize what Column you want to apply that filter to. Please say something like \"Filter for Sales >= 300\"";

                var cancelMessage = MessageFactory.Text(message, CancelMsgText, InputHints.IgnoringInput);
                await stepContext.Context.SendActivityAsync(cancelMessage, cancellationToken);
                return await stepContext.CancelAllDialogsAsync(cancellationToken);
            }
            else if (filterForNumberDetails.comparisonOperator == null)//Comparison operator is missing
            {
                string message = "I could not recognize what Number Operator (like >, <, >=,...) you want to use in that filter. Please say something like \"Filter for Sales >= 300\"";

                var cancelMessage = MessageFactory.Text(message, CancelMsgText, InputHints.IgnoringInput);
                await stepContext.Context.SendActivityAsync(cancelMessage, cancellationToken);
                return await stepContext.CancelAllDialogsAsync(cancellationToken);
            }
            else if (filterForNumberDetails.filterNumber == null) //The number is missing
            {
                string message = "I could not recognize what Number want to filter for (like 300). Please say something like \"Filter for Sales >= 300\"";

                var cancelMessage = MessageFactory.Text(message, CancelMsgText, InputHints.IgnoringInput);
                await stepContext.Context.SendActivityAsync(cancelMessage, cancellationToken);
                return await stepContext.CancelAllDialogsAsync(cancellationToken);
            }

            //No ambiguities and no missing values
            return await stepContext.NextAsync(filterForNumberDetails.columnName[0], cancellationToken);
        }

        //Summarize results (e.g. by extracting information from the ambiguity dialog)
        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var filterForNumberDetails = (FilterForNumberDetails)stepContext.Options;

            //We are comming from a ChoicePromt ==> Convert result to FoundCHoice (in this case we come from an ambiguity dialog. Otherwise there would have been no ChoicePromt)
            if (stepContext.Result.GetType().ToString().Equals("Microsoft.Bot.Builder.Dialogs.Choices.FoundChoice"))
            {
                //Extract the picked result from the step-context
                var pickedChoice = (FoundChoice)stepContext.Result;
                var choiceText = pickedChoice.Value;

                //Set the result to the ChangeCharttypeDetails-Object
                filterForNumberDetails.columnName = new string[] { choiceText };
            }
            //Now the Object is set right and we can print, what we want to change our charttype to
            ConsoleWriter.WriteLineInfo("Filtering " + filterForNumberDetails.columnName[0] + " " + filterForNumberDetails.comparisonOperator + " " + filterForNumberDetails.filterNumber);
            return await stepContext.EndDialogAsync(filterForNumberDetails, cancellationToken);
        }
    }
}