using System.Linq;
using log4net;

namespace store_scrapper_2.Logging
{
  public static class LoggerExtensions
  {
    public static void Info(this ILog sender, string message, params object[] logParams) =>
      sender.Info(new object[] { "message", message }.Concat(logParams));
  }
}