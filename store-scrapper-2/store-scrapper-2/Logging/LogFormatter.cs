using System.Collections.Generic;
using System.Linq;

namespace store_scrapper_2.Logging
{
  public static class LogFormatter
  {
    private static readonly string NO_OUTPUT = string.Empty;
    private static readonly string FIELD_SEPARATOR = ", ";

    public static string Format(IDictionary<string, object> logEntry)
    {
      if (logEntry == null)
      {
        return NO_OUTPUT;
      }

      if (logEntry.Keys.Count == 0)
      {
        return NO_OUTPUT;
      }

      return logEntry
        .Select(_ => Format(_.Key, _.Value))
        .Aggregate((i, j) => i + FIELD_SEPARATOR + j);
    }

    private static string Format(string key, object value)
    {
      return $"{key}:null";     
    }
  }
}