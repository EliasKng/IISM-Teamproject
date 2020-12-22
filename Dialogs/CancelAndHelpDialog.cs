//Nur hier vorhanden ==> Abändern?s
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

namespace Microsoft.BotBuilderSamples.Dialogs
{
    public class CancelAndHelpDialog : ComponentDialog
    {
        protected string HelpMsgText = "What you could type in:\n\"Filter for e.g. country\" \n\"Change charttype to e.g. barchart\"";
        protected string CancelMsgText = "Cancelling...";
        protected string FilterMsgText = "Please type in: Filter for e.g. Canada.";

        public CancelAndHelpDialog(string id)
            : base(id)
        {
        }

        protected override async Task<DialogTurnResult> OnContinueDialogAsync(DialogContext innerDc, CancellationToken cancellationToken = default)
        {
            var result = await InterruptAsync(innerDc, cancellationToken);
            if (result != null)
            {
                return result;
            }

            return await base.OnContinueDialogAsync(innerDc, cancellationToken);
        }

        private async Task<DialogTurnResult> InterruptAsync(DialogContext innerDc, CancellationToken cancellationToken)
        {
            if (innerDc.Context.Activity.Type == ActivityTypes.Message)
            {
                var text = innerDc.Context.Activity.Text.ToLowerInvariant();

                switch (text)
                {
                    case "help":
                    case "?":
                        var helpMessage = MessageFactory.Text(HelpMsgText, HelpMsgText, InputHints.ExpectingInput);
                        await innerDc.Context.SendActivityAsync(helpMessage, cancellationToken);
                        //return new DialogTurnResult(DialogTurnStatus.Waiting);
                        return await innerDc.CancelAllDialogsAsync(cancellationToken);

                    case "cancel":
                    case "quit":
                        var cancelMessage = MessageFactory.Text(CancelMsgText, CancelMsgText, InputHints.IgnoringInput);
                        await innerDc.Context.SendActivityAsync(cancelMessage, cancellationToken);
                        return await innerDc.CancelAllDialogsAsync(cancellationToken);
                    case "filter":
                    case "fitexit":
                    case "filter for":
                        var filterMessage = MessageFactory.Text(FilterMsgText, FilterMsgText, InputHints.IgnoringInput);
                        await innerDc.Context.SendActivityAsync(filterMessage, cancellationToken);
                        return await innerDc.CancelAllDialogsAsync(cancellationToken);
                }
            }

            return null;
        }
    }
}
