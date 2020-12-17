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
            if (changeChartTypeDetails.ToChartType == null)
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

            return await stepContext.EndDialogAsync(changeChartTypeDetails,cancellationToken);
        }
    }
}