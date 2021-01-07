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

public class BOT_Api
{
    public static void SendRequest(VisualizationInteraction.Intent intent, string[] parameters)
    {
        string param = string.Join(", ", parameters);

        ConsoleWriter.WriteLineInfo("Intent: " + intent + " Parameters: " + param);
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
        ChangeCharttypeJson testChangeCharttypeJson = new ChangeCharttypeJson
        {
            target_vis = toCharttype
        };
        HttpPostRequestAsync("http://localhost:5000/change", testChangeCharttypeJson);
    }

    public static void SendChangeVisualizationPart(string visPart, string toColumn)
    {
        SendRequest(VisualizationInteraction.Intent.ChangeVisualizationPart, new string[] { visPart, toColumn });
    }

    public static void SendFilterForNumber(string columnName, string comparisonOperator, string number)
    {
        SendRequest(VisualizationInteraction.Intent.ChangeVisualizationPart, new string[] { columnName, comparisonOperator, number});
    }

    public static void SendFilterForWord(string[] keywords)
    {
        SendRequest(VisualizationInteraction.Intent.ChangeVisualizationPart, keywords); 
    }


    public static async void HttpPostRequestAsync(string url, Json jsonObject)
    {
        var jsonString = await Task.Run(() => JsonConvert.SerializeObject(jsonObject));


        var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");


        using (var httpClient = new HttpClient())
        {

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
    }
}

public class ChangeCharttypeJson : Json
{
    [JsonProperty("target_vis")]
    public string target_vis { get; set; }
}

public abstract class Json
{
}