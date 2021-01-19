using Microsoft.BotBuilderSamples;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Web;  // also add a reference to System.web.dll for HttpUtility class to be found
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;

public class BOT_Api
{
    public async static void SendRemoveVisualization()
    {

    }

    public async static Task SendClearFilter(WaterfallStepContext stepContext)
    {
        FilterForWordJson json = new FilterForWordJson();
        //Set keywords to none
        string messageForUser = "Clearing all Filters";
        //This would send the JSON to the Backend
        //await HttpPostRequestAsync(stepContext, "http://localhost:5000/keywords/delete/all", json, messageFoUser);
        //This sends the json to the frontend
        await SendActivityAsync(stepContext, "/keywords/delete/all",json,messageForUser);
    }
    public async static Task SendChangeChartTypeAsync(WaterfallStepContext stepContext, string toCharttype)
    {
        switch (toCharttype)
        {
            case "barchart":
                toCharttype = "BarChart";
                break;
            case "scatterplot":
                toCharttype = "ScatterPlot";
                break;
            case "columnchart":
                toCharttype = "ColumnChart";
                break;
            case "piechart":
                toCharttype = "PieChart";
                break;
        }
        ChangeCharttypeJson json = new ChangeCharttypeJson
        {
            target_vis = toCharttype
        };
        string messageForUser = "changing charttype to " + toCharttype;
        //await HttpPostRequestAsync(stepContext, "http://localhost:5000/change", json, messageForUser);
        //This sends the json to the frontend
        await SendActivityAsync(stepContext, "/change", json, messageForUser);
    }

    public async static Task SendChangeVisualizationPart(WaterfallStepContext stepContext, string visPart, string toColumn)
    {
        ChangeVisPartJson json = new ChangeVisPartJson();
        switch (visPart)
        {
            case "xAxis":
                json.xcolor = toColumn;
                break;
            case "yAxis":
                json.ytheta = toColumn;
                break;
            case "theta":
                json.ytheta = toColumn;
                break;
            case "color":
                json.xcolor = toColumn;
                break;
            default:
                ConsoleWriter.WriteLineInfo("Error while determining the right vispart for json serialization");
                break;
        }
        string messageForUser = "changing " + visPart + " to " + toColumn;
        //await HttpPostRequestAsync(stepContext, "http://localhost:5000/change-fields", json, messageForUser);
        //This sends the json to the frontend
        await SendActivityAsync(stepContext, "/change-fields", json, messageForUser);
    }

    public async static Task SendChangeAggregate(WaterfallStepContext stepContext, string visPart, string toAggregate)
    {
        ChangeVisPartJson json = new ChangeVisPartJson();
        switch (visPart)
        {
            case "xAxis":
                json.xcolor = toAggregate;
                break;
            case "yAxis":
                json.ytheta = toAggregate;
                break;
            case "theta":
                json.ytheta = toAggregate;
                break;
            case "color":
                json.xcolor = toAggregate;
                break;
            default:
                ConsoleWriter.WriteLineInfo("Error while determining the right vispart for json serialization");
                break;
        }
        string messageForUser = "changing aggregate of " + visPart + " to " + toAggregate;
        //await HttpPostRequestAsync(stepContext, "http://localhost:5000/change-fields", json, messageForUser);
        //This sends the json to the frontend
        await SendActivityAsync(stepContext, "/change-aggregate", json, messageForUser);
    }

    public async static Task SendNL4DV(WaterfallStepContext stepContext, string query)
    {
        NL4DVJson json = new NL4DVJson();
        json.query = query;
        string messageForUser = "Executing your query";
        //await HttpPostRequestAsync(stepContext, "http://localhost:5000/nl4dv", json, messageForUser);
        //This sends the json to the frontend
        await SendActivityAsync(stepContext, "/nl4dv", json, messageForUser);
    }

    public async static Task SendFilterForNumber(WaterfallStepContext stepContext, string columnName, string comparisonOperator, string number)
    {
        FilterForNumberJson json = new FilterForNumberJson
        {
            column = columnName,
            comparisonOperator = comparisonOperator,
            number = number
        };
        string messageForUser = "Filter for " + columnName + " " + comparisonOperator + " " + number;
        //await HttpPostRequestAsync(stepContext, "http://localhost:5000/keywords/add-number", json, messageForUser);
        //This sends the json to the frontend
        await SendActivityAsync(stepContext, "/keywords/add-number", json, messageForUser);
    }

    public async static Task SendFilterForWord(WaterfallStepContext stepContext, string p_column, string[] p_values)
    {
        FilterForWordJson json = new FilterForWordJson
        {
            column = p_column,
            values = p_values
        };
        string messageForUser = "Filter for " + String.Join(", ",p_values);
        //await HttpPostRequestAsync(stepContext, "http://localhost:5000/keywords/add-word", json, messageForUser);
        //This sends the json to the frontend
        await SendActivityAsync(stepContext, "/keywords/add-word", json, messageForUser);
    }

    public static HttpClient httpClient = new HttpClient();
    public static async Task HttpPostRequestAsync(WaterfallStepContext stepContext, string url, Json jsonObject, string messageForUser)
    {
        try
        {
            var jsonString = await Task.Run(() => JsonConvert.SerializeObject(jsonObject));

            ConsoleWriter.WriteLineInfo("Sending JSON: " + jsonString);

            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            // Do the actual request and await the response
            var httpResponse = await httpClient.PostAsync(url, httpContent);

            // If the response contains content we want to read it!
            if (httpResponse.Content != null)
            {
                var responseContent = await httpResponse.Content.ReadAsStringAsync();
                ConsoleWriter.WriteLineInfo("Output: " + responseContent);
                // From here on you could deserialize the ResponseContent back again to a concrete C# type using Json.Net
                
                
                //Sending activity to the frontend
                Activity message = MessageFactory.Text(messageForUser);
                message.Value = responseContent;
                await stepContext.Context.SendActivityAsync(message);
            }

        }
        catch (Exception e)
        {
            ConsoleWriter.WriteLineInfo("Error sending POST request to: " + url);
            ConsoleWriter.WriteLineInfo("Exception: " + e.Message);
        }

    }

    public static async Task SendActivityAsync(WaterfallStepContext stepContext, string endpoint, Json jsonObject, string messageForUser)
    {
        jsonObject.endpoint = endpoint;

        var jsonString = await Task.Run(() => JsonConvert.SerializeObject(jsonObject));

        ConsoleWriter.WriteLineInfo("Sending JSON: " + jsonString);

        //Sending activity to the frontend
        Activity message = MessageFactory.Text(messageForUser);
        message.Value = jsonString;
        await stepContext.Context.SendActivityAsync(message);
    }
}

public class ChangeCharttypeJson : Json
{
    [JsonProperty("target_vis")]
    public string target_vis { get; set; }
}

public class ChangeVisPartJson : Json
{
    [JsonProperty("x-color")]
    public string xcolor { get; set; }
    [JsonProperty("y-theta")]
    public string ytheta { get; set; }
}
public class NL4DVJson : Json
{
    [JsonProperty("query")]
    public string query { get; set; }
}

public class FilterForWordJson : Json
{
    [JsonProperty("column")]
    public string column { get; set; }
    [JsonProperty("values")]
    public string[] values { get; set; }

}
public class FilterForNumberJson : Json
{
    [JsonProperty("column")]
    public string column { get; set; }
    [JsonProperty("comparisonOperator")]
    public string comparisonOperator { get; set; }
    [JsonProperty("number")]
    public string number { get; set; }

}

public class Json
{
    [JsonProperty("endpoint")]
    public string endpoint { get; set; }
}