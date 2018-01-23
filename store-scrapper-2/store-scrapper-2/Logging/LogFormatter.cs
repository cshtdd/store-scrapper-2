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
        .SanitizeValue()
        .Encapsulate();
      return $"{formattedKey}:{formattedValue}";     
    }
  }

  internal static class StringExtentions
  {
    private static readonly string FieldWrapper = "\"";
    private static readonly char[] WrappeableChars = new[] { '"', ' ', '\''};

    internal static string SanitizeKey(this string key)
    {
      var sanitizedChars = key
        .ToCharArray()
        .Where(_ => char.IsLetterOrDigit(_) || char.IsWhiteSpace(_) || _ == '_')
        .ToArray(); 
      return new string(sanitizedChars);
    }

    internal static string SanitizeValue(this string value)
    {
      return value.Replace("\"", "\\\"");
    }
    
    internal static string Encapsulate(this string sender)
    {
      if (sender == string.Empty || 
          WrappeableChars.Any(sender.Contains))
      {        
        return Wrap(sender);
      }

      return sender;
    }

    private static string Wrap(string str) => $"{FieldWrapper}{str}{FieldWrapper}";

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