using System;
using System.Linq;
using log4net;

namespace store_scrapper_2.Logging
{
  public static class LoggerExtensions
  {
    public static void LogInfo(this ILog sender, string message, params object[] logParams)
    {
      var logEntry = new object[] {"message", message}.Concat(logParams).ToArray();
      var formattedMessage = LogFormatter.Format(logEntry);
      sender.Info(formattedMessage);
    }

    public static void LogInfo(this ILog sender, string message, Exception error, params object[] logParams)
    {
      var logEntry = new object[] {"message", message}.Concat(logParams).ToArray();
      var formattedMessage = LogFormatter.Format(logEntry);
      sender.Info(formattedMessage, error);
    }
  }
}