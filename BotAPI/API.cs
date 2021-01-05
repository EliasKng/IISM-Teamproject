using Microsoft.BotBuilderSamples;
using System;

public class BOT_Api
{
    public static void WriteLineInfo(string value)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write("info");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(": ");
        Console.WriteLine(value);
    }

    public static void SendRequest(VisualizationInteraction.Intent intent, string[] parameters)
    {
        string param = string.Join(", ", parameters);

        ConsoleWriter.WriteLineInfo("Intent: " + intent + " Parameters: " + param);
    }

    public static void SendChangeChartType(string toCharttype)
    {
        SendRequest(VisualizationInteraction.Intent.ChangeChartType, new string[] { toCharttype });
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
}