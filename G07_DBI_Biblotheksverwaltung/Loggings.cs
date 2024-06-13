using System;
using System.IO;

public static class Logger
{
    private static readonly string logFilePath = "app.log";

    public static void Log(string message, string logType = "INFO")
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [{logType}] {message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fehler beim Schreiben in die Logdatei: {ex.Message}");
        }
    }

    public static void LogError(string message)
    {
        Log(message, "ERROR");
    }
}
