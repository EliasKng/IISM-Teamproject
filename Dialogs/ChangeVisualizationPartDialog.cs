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
            var changeVisualizationPartDetails = (ChangeVisualizationPartDetails)stepContext.Options;

            return null;
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