using System;
using System.Linq;
using log4net;

namespace store_scrapper_2.Logging
{
  public static class LoggerExtensions
  {
    public static void LogDebug(this ILog sender, string message, params object[] logParams) => 
      sender.Debug(CreateFormattedMessage(message, logParams));
    public static void LogDebug(this ILog sender, string message, Exception error, params object[] logParams) =>
      sender.Debug(CreateFormattedMessage(message, logParams), error);
    
    public static void LogInfo(this ILog sender, string message, params object[] logParams) => 
      sender.Info(CreateFormattedMessage(message, logParams));
    public static void LogInfo(this ILog sender, string message, Exception error, params object[] logParams) =>
      sender.Info(CreateFormattedMessage(message, logParams), error);
    
    public static void LogWarn(this ILog sender, string message, params object[] logParams) => 
      sender.Warn(CreateFormattedMessage(message, logParams));
    public static void LogWarn(this ILog sender, string message, Exception error, params object[] logParams) =>
      sender.Warn(CreateFormattedMessage(message, logParams), error);

    public static void LogError(this ILog sender, string message, params object[] logParams) => 
      sender.Error(CreateFormattedMessage(message, logParams));
    public static void LogError(this ILog sender, string message, Exception error, params object[] logParams) =>
      sender.Error(CreateFormattedMessage(message, logParams), error);

    public static void LogFatal(this ILog sender, string message, params object[] logParams) => 
      sender.Fatal(CreateFormattedMessage(message, logParams));
    public static void LogFatal(this ILog sender, string message, Exception error, params object[] logParams) =>
      sender.Fatal(CreateFormattedMessage(message, logParams), error);
    
    private static string CreateFormattedMessage(string message, object[] logParams)
    {
      var logEntry = new object[] {"message", message}.Concat(logParams).ToArray();
      var formattedMessage = LogFormatter.Format(logEntry);
      return formattedMessage;
    }
  }
}