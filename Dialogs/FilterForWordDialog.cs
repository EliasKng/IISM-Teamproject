using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Builder.Dialogs.Choices;

namespace Microsoft.BotBuilderSamples.Dialogs
{
    public class FilterForWordDialog : CancelAndHelpDialog
    {
        private const string DestinationStepMsgText = "Please choose an attribute from the list!";

        // Define a "done" response for the country selection prompt.
        private const string DoneOption = "done";

        // Define value names for values tracked inside the dialogs.
        private const string CountriesSelected = "value-countriesSelected";


        // options of countries to choose from
        private readonly string[] _countryOptions = new string[]
        {
            "Canada", "France", "Germany", "Mexico", "United States of America"
        };

        public FilterForWordDialog() : base(nameof(FilterForWordDialog))
        {
            HelpMsgText = "In this step type in e.g.: \"Filter for enterprise.\"";
            CancelMsgText = "Cancelling the Filter for Word Dialog";
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
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        //Check if input is empty or additionally it is asked to filter for countries, then add country option list
        private async Task<DialogTurnResult> SelectionStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // Continue using the same selection list, if any, from the previous iteration of this dialog.
            var list = stepContext.Options as List<string> ?? new List<string>();

            bool filterForCountry = false;

            if (stepContext.Options.GetType().ToString().Equals("Microsoft.BotBuilderSamples.FilterForWordDetails"))
            {
                FilterForWordDetails filterForWordDetails = (FilterForWordDetails)stepContext.Options;
                
                // Check if filter for Word Dialog input is empty
                if (filterForWordDetails.columnName == null)
                {   
                    string messageNull = "I could not recognize what column you want to apply that filter to. Please say something like \"Filter for Germany and Canada\"";
                    var cancelMessage = MessageFactory.Text(messageNull, CancelMsgText, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(cancelMessage, cancellationToken);
                    return await stepContext.CancelAllDialogsAsync(cancellationToken);
                }

                //Filter for country
                for (int i = 0; i < filterForWordDetails.columnName.Length; i++)
                {
                    string s = UppercaseFirst(filterForWordDetails.columnName[i]);
                    list.Add(s);

                    //becomes true when you enter "filter for country"
                    if (string.Equals(s, "Country") || string.Equals(s, "Countries"))
                    {
                        filterForCountry = true;
                    }
                }

                if ((filterForWordDetails.country == null) && (filterForWordDetails.segment== null) && (filterForWordDetails.product == null) && !filterForCountry)
                {
                    string messageNull = "I could not recognize what column you want to apply that filter to. Please say something like \"Filter for Germany and Canada\"";

                    var cancelMessage = MessageFactory.Text(messageNull, CancelMsgText, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(cancelMessage, cancellationToken);
                    return await stepContext.CancelAllDialogsAsync(cancellationToken);
                }
                

                //Breaks if we did not recognize any countries
                if (!filterForCountry)
                {
                    filterForWordDetails.columnName = list.ToArray();
                    ConsoleWriter.WriteLineInfo("Filter for: " + string.Join(", ", list));
                    BOT_Api.SendFilterForWord(list.ToArray());
                    return await stepContext.EndDialogAsync(filterForWordDetails.columnName);
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
            else if (list[0] == "Countries")
            {
                message = $"Please choose a country to filter to finish.";
                list.Remove("Countries");
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
                ConsoleWriter.WriteLineInfo("Filter for: " + string.Join(", ", list));
                FilterForWordDetails filterForWordDetails = new FilterForWordDetails();
                filterForWordDetails.columnName = list.ToArray();

                BOT_Api.SendFilterForWord(list.ToArray());

                return await stepContext.EndDialogAsync(list, cancellationToken);
            }
            else
            {
                // Otherwise, repeat this dialog, passing in the list from this iteration.
                return await stepContext.ReplaceDialogAsync(nameof(FilterForWordDialog), list, cancellationToken);
            }
        }


    }
}