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

public class BOT_Api
{
    public static void SendRemoveVisualization()
    {

    }

    public static void SendClearFilter()
    {
        FilterForWordJson json = new FilterForWordJson();
        //Set keywords to none
        HttpPostRequestAsync("http://localhost:5000/keywords/delete/all", json);
    }
    public static async Task SendChangeChartTypeAsync(string toCharttype)
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
        HttpPostRequestAsync("http://localhost:5000/change", json);
    }

    public static void SendChangeVisualizationPart(string visPart, string toColumn)
    {
        ChangeVisPartJson json = new ChangeVisPartJson();
        switch (visPart)
        {
            case "xAxis":
                json.xcolor = toColumn;
                break;
            case "yAxis":
                json.ytetha = toColumn;
                break;
            case "tetha":
                json.ytetha = toColumn;
                break;
            case "color":
                json.xcolor = toColumn;
                break;
            default:
                ConsoleWriter.WriteLineInfo("Error while determining the right vispart for json serialization");
                break;
        }
        HttpPostRequestAsync("http://localhost:5000/change-fields", json);
    }

    public static void SendNL4DV(string query)
    {
        NL4DVJson json = new NL4DVJson();
        json.query = query;
        HttpPostRequestAsync("http://localhost:5000/nl4dv", json);
    }

    public static void SendFilterForNumber(string columnName, string comparisonOperator, string number)
    {
        FilterForNumberJson json = new FilterForNumberJson
        {
            column = columnName,
            comparisonOperator = comparisonOperator,
            number = number
        };
        HttpPostRequestAsync("http://localhost:5000/keywords/add-number", json);
    }

    public static void SendFilterForWord(string p_column, string[] p_values)
    {
        FilterForWordJson json = new FilterForWordJson
        {
            column = p_column,
            values = p_values
        };

        HttpPostRequestAsync("http://localhost:5000/keywords/add-word", json);
    }

    public static HttpClient httpClient = new HttpClient();
    public static async void HttpPostRequestAsync(string url, Json jsonObject)
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
            }

        }
        catch (Exception e)
        {
            ConsoleWriter.WriteLineInfo("Error sending POST request to: " + url);
            ConsoleWriter.WriteLineInfo("Exception: " + e.Message);
        }

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
    [JsonProperty("y-tetha")]
    public string ytetha { get; set; }
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
}