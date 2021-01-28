using System;
using System.IO;
using System.Text;

public class ConsoleWriter
{
    public static void WriteLineInfo(string value)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write("info");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(": ");
        Console.WriteLine(value);
    }
}