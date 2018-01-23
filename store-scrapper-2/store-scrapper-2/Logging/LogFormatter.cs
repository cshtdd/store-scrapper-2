using System;
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

      if (logEntry.Keys.Any(string.IsNullOrEmpty))
      {
        throw new ArgumentException("All keys must be non-Empty");
      }

      return logEntry
        .Select(_ => Format(_.Key, _.Value))
        .Aggregate((i, j) => $"{i}{FIELD_SEPARATOR}{j}");
    }

    private static string Format(string key, object value)
    {
      var formattedKey = key.AddQuotesToKey();
      return $"{formattedKey}:null";     
    }
  }

  internal static class StringExtentions
  {
    internal static string AddQuotesToKey(this string key)
    {
      if (!key.Contains(' '))
      {
        return key;
      }

      return $"\"{key}\"";
    }
  }
}