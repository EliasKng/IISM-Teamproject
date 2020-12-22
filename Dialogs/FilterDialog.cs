using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Builder.Dialogs.Choices;

namespace Microsoft.BotBuilderSamples.Dialogs
{
    public class FilterDialog : CancelAndHelpDialog
    {
        //Zu dem CHarttyp wollen wir wechseln
        private const string DestinationStepMsgText = "Please choose an attribute from the list!";

        // Define a "done" response for the company selection prompt.
        private const string DoneOption = "done";

        // Define value names for values tracked inside the dialogs.
        private const string CountriesSelected = "value-countriesSelected";


        private readonly string[] _countryOptions = new string[]
        {
            "Canada", "France", "Germany", "Mexico", "United States of America"
        };

        public FilterDialog()
            : base(nameof(FilterDialog))
        {
            HelpMsgText = "In this step type in: \"Filter for e.g. Germany and France\"";
            CancelMsgText = "Cancelling the Filter-Dialog";
            FilterMsgText = "Please type in: Filter for e.g. Canada.";
            //AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                SelectionStepAsync,
                FinalStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            //s = s.ToLower(); //does'nt work for United States of America
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        //Hier findet er raus, nach welchen Attributen gefiltert werden soll
        private async Task<DialogTurnResult> SelectionStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            ConsoleWriter.WriteLineInfo("Type: " + stepContext.Options.GetType().ToString());



            // Continue using the same selection list, if any, from the previous iteration of this dialog.
            var list = stepContext.Options as List<string> ?? new List<string>();

            //bool country = false;
            bool filterForCountry = false;

            //We add the recognized Entities from Louis to the Selection list
            if (stepContext.Options.GetType().ToString().Equals("Microsoft.BotBuilderSamples.FilterDetails"))
            {
                FilterDetails filterDetails = (FilterDetails)stepContext.Options;
                
                if (filterDetails.multipleFilters == null)
                {
                    //Other dialog that reacts to "Filter!" ??
                    return await stepContext.EndDialogAsync(list, cancellationToken);
                }

                for (int i = 0; i < filterDetails.multipleFilters.Length; i++)
                {
                    string s = UppercaseFirst(filterDetails.multipleFilters[i]);
                    list.Add(s);

                    //becomes true when you enter "filter for country"
                    if (string.Equals(s, "Country") || string.Equals(s, "Countries"))
                    {
                        filterForCountry = true;
                    }
                }

                //Breaks if we did not recognize any countries
                if (!filterForCountry)
                {
                    ConsoleWriter.WriteLineInfo("Filter for: " + string.Join(", ", list));
                    return await stepContext.EndDialogAsync(filterDetails.multipleFilters);
                }
            }

            //if user entered "FILTER FOR COUNTRY" OR "COUNTRIES":

            stepContext.Values[CountriesSelected] = list;
            //Create a prompt message
            string message;
            if (list[0] == "Country")
            {
                message = $"Please choose a country to filter to finish.";
                list.Remove("Country");
            }
            else
            {
                message = $"You have selected **{String.Join(", ", list)}**. You can filter for an additional country, " +
                    $"or choose `{DoneOption}` to finish.";
            }

            // Create the list of options to choose from.
            var options = _countryOptions.ToList();
            
            if (list.Count > 0)
            {
                options.Add(DoneOption);
                options = options.Except(list).ToList();
            }

            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text(message),
                RetryPrompt = MessageFactory.Text("Please choose an option from the list."),
                Choices = ChoiceFactory.ToChoices(options),
            };

            // Prompt the user for a choice.
            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(
            WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            // Retrieve their selection list, the choice they made, and whether they chose to finish.
            var list = stepContext.Values[CountriesSelected] as List<string>;
            var choice = (FoundChoice)stepContext.Result;
            var done = choice.Value == DoneOption;

            if (!done)
            {
                // If they chose a company, add it to the list.
                list.Add(choice.Value);
            }

            if (done)
            {
                // If they're done, exit and return their list.
                ConsoleWriter.WriteLineInfo( "Filter for: " + string.Join(", ", list));

                return await stepContext.EndDialogAsync(list, cancellationToken);
            }
            else
            {
                // Otherwise, repeat this dialog, passing in the list from this iteration.
                return await stepContext.ReplaceDialogAsync(nameof(FilterDialog), list, cancellationToken);
            }
        }
    }
}

