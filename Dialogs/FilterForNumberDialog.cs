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
            var filterForNumberDetails = (FilterForNumberDetails)stepContext.Options;
            if (filterForNumberDetails.columnName?.Length > 1)
            {
                //We have ambiguities (more than one Entity) ==> ask the user with the AmbiguityDialog
                return await stepContext.BeginDialogAsync(nameof(AmbiguityDialog), filterForNumberDetails.columnName, cancellationToken);
            }
            else if (filterForNumberDetails.columnName == null)
            {
                string message = "I could not regonize what Column you want to apply that filter to. Please say something like \"Filter for Sales >= 300\"";

                var cancelMessage = MessageFactory.Text(message, CancelMsgText, InputHints.IgnoringInput);
                await stepContext.Context.SendActivityAsync(cancelMessage, cancellationToken);
                return await stepContext.CancelAllDialogsAsync(cancellationToken);
            }
            else if (filterForNumberDetails.comparisonOperator == null)
            {
                string message = "I could not regonize what Number Operator (like >, <, >=,...) you want to use in that filter. Please say something like \"Filter for Sales >= 300\"";

                var cancelMessage = MessageFactory.Text(message, CancelMsgText, InputHints.IgnoringInput);
                await stepContext.Context.SendActivityAsync(cancelMessage, cancellationToken);
                return await stepContext.CancelAllDialogsAsync(cancellationToken);
            }
            else if (filterForNumberDetails.filterNumber == null)
            {
                string message = "I could not regonize what Number want to filter for (like 300). Please say something like \"Filter for Sales >= 300\"";

                var cancelMessage = MessageFactory.Text(message, CancelMsgText, InputHints.IgnoringInput);
                await stepContext.Context.SendActivityAsync(cancelMessage, cancellationToken);
                return await stepContext.CancelAllDialogsAsync(cancellationToken);
            }

            //It is not null and length is not larger than one ==> take the first one
            return await stepContext.NextAsync(filterForNumberDetails.columnName[0], cancellationToken);
        }

        //Hier wird bestätigt, dass wohin gewechselt wurde
        //WaterfallStepContext wird von vorherigem Step übernommen
        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var filterForNumberDetails = (FilterForNumberDetails)stepContext.Options;
            //We are comming from a ChoicePromt ==> Convert result to FoundCHoice
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