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
    public class Nl4dvDialog : CancelAndHelpDialog
    {

        public Nl4dvDialog() : base(nameof(Nl4dvDialog))
        {
            HelpMsgText = "In this step type in e.g.: Show me a distribution of sales";
            CancelMsgText = "Cancelling the NL4DV Query Dialog";
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
            var nl4dvQueryDetails = (Nl4dvQueryDetails)stepContext.Options;
            if (nl4dvQueryDetails.chartType?.Length > 1) //Do we have Ambiguities?
            {
                //We have ambiguities (more than one Entity) ==> ask the user with the AmbiguityDialog
                ConsoleWriter.WriteLineInfo("Checking for Chart Ambiguities");
                return await stepContext.BeginDialogAsync(nameof(AmbiguityDialog), nl4dvQueryDetails.chartType, cancellationToken);
                ConsoleWriter.WriteLineInfo("Checked for Chart Ambiguities");
            }
            if (nl4dvQueryDetails.chartType == null) //Do we have Ambiguities?
            {
                return await stepContext.BeginDialogAsync(nameof(AmbiguityDialog),new string[]{ "barchart","columnchart","scatterplot","piechart", "I am not sure"}, cancellationToken);
            }
            return await stepContext.NextAsync(nl4dvQueryDetails, cancellationToken);
        }

        //Summarize results (e.g. by extracting information from the ambiguity dialog)
        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var nl4dvQueryDetails = (Nl4dvQueryDetails)stepContext.Options;

            //We are comming from a ChoicePromt ==> Convert result to FoundCHoice (in this case we come from an ambiguity dialog. Otherwise there would have been no ChoicePromt)
            if (stepContext.Result.GetType().ToString().Equals("Microsoft.Bot.Builder.Dialogs.Choices.FoundChoice"))
            {
                //Extract the picked result from the step-context
                var pickedChoice = (FoundChoice)stepContext.Result;
                var choiceText = pickedChoice.Value;
                ConsoleWriter.WriteLineInfo("Replacing Chart by " + choiceText);
                if(choiceText.Equals("I am not sure"))
                {
                    nl4dvQueryDetails.queryText = nl4dvQueryDetails.queryText.Replace("chart", "");
                } else
                {
                    nl4dvQueryDetails.queryText = nl4dvQueryDetails.queryText.Replace("chart", choiceText);
                    if(!nl4dvQueryDetails.queryText.Contains(choiceText))
                    {
                        nl4dvQueryDetails.queryText = nl4dvQueryDetails.queryText + " in a " + choiceText;
                    }
                }
                
                
            }
            //Now the Object is set right and we can print, what we want to change our charttype to

            BOT_Api.SendNL4DV(nl4dvQueryDetails.queryText);

            return await stepContext.EndDialogAsync(nl4dvQueryDetails, cancellationToken);
        }
    }
}