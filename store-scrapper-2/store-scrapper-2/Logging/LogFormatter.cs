using System;
using System.Collections.Generic;
using System.Linq;

namespace store_scrapper_2.Logging
{
  public static class LogFormatter
  {
    private static readonly string NoOutput = string.Empty;
    private static readonly string FieldSeparator = ", ";

    public static string Format(IDictionary<string, object> logEntry)
    {
      if (logEntry == null)
      {
        return NoOutput;
      }

      if (logEntry.Keys.Count == 0)
      {
        return NoOutput;
      }

      if (logEntry.Keys.Any(string.IsNullOrEmpty))
      {
        throw new ArgumentException("All keys must be non-Empty");
      }

      return logEntry
        .Select(_ => Format(_.Key, _.Value))
        .Aggregate((i, j) => $"{i}{FieldSeparator}{j}");
    }

    private static string Format(string key, object value)
    {
      var formattedKey = key
        .SanitizeKey()
        .Encapsulate();

      var formattedValue = value
        .FormatValue()
        .Encapsulate();
      return $"{formattedKey}:{formattedValue}";     
    }
  }

  internal static class StringExtentions
  {
    private static readonly string FieldWrapper = "\"";

    internal static string SanitizeKey(this string key)
    {
      var sanitizedChars = key
        .ToCharArray()
        .Where(_ => char.IsLetterOrDigit(_) || char.IsWhiteSpace(_) || _ == '_')
        .ToArray(); 
      return new string(sanitizedChars);
    }
    
    internal static string Encapsulate(this string sender)
    {
      if (sender == string.Empty)
      {
        return $"{FieldWrapper}{sender}{FieldWrapper}"; 
      }
      
      if (!sender.Contains(' '))
      {
        return sender;
      }

      return $"{FieldWrapper}{sender}{FieldWrapper}";
    }

    internal static string FormatValue(this object sender)
    {
      if (sender == null)
      {
        return "null";
      }

      return sender.ToString();
    }
  }
}