using System;
using System.Collections.Generic;
using System.Linq;

namespace store_scrapper_2.Logging
{
  public static class LogFormatter
  {
    private static readonly string NoOutput = string.Empty;
    private static readonly string FieldSeparator = ", ";

    public static string Format(object[] kvPairs)
    {
      ValidateInput(kvPairs);

      if (kvPairs.Length == 0)
      {
        return NoOutput;
      }

      return kvPairs
        .Keys()
        .Select((k, i) => Format(k, kvPairs[i*2 + 1]))
        .Aggregate((i, j) => $"{i}{FieldSeparator}{j}");
    }

    private static void ValidateInput(object[] kvPairs)
    {
      if (kvPairs == null)
      {
        throw new ArgumentException($"{nameof(kvPairs)} cannot be null");
      }

      if (kvPairs.Length % 2 != 0)
      {
        throw new ArgumentException($"{nameof(kvPairs)}.Length must be even");
      }

      var keys = kvPairs.Keys().ToArray();
      
      if (keys.Any(_ => _ == null))
      {
        throw new ArgumentException("All Keys must be non-null");
      }
      
      if (keys.Any(string.Empty.Equals))
      {
        throw new ArgumentException("All Keys must be non-empty");
      }
      
      if (keys.Any(_ => !(_ is string)))
      {
        throw new ArgumentException("Invalid Non-String key");
      }

      if (keys.Length != keys.Distinct().Count())
      {
        throw new ArgumentException("Found Duplicated key");
      }
    }

    private static string Format(object stringKey, object value) => Format((string) stringKey, value);
    
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

    internal static string SanitizeKey(this string key)
    {
      var sanitizedChars = key
        .ToCharArray()
        .Where(_ => IsAlwaysAllowed(_) || char.IsWhiteSpace(_))
        .ToArray(); 
      return new string(sanitizedChars);
    }

    internal static string SanitizeValue(this string value) => value.Replace("\"", "\\\"");
    
    internal static string Encapsulate(this string sender)
    {
      if (sender == string.Empty || 
          sender.Any(_ => !IsAlwaysAllowed(_)))
      {        
        return Wrap(sender);
      }

      return sender;
    }

    private static string Wrap(string str) => $"{FieldWrapper}{str}{FieldWrapper}";

    private static bool IsAlwaysAllowed(char c) => char.IsLetterOrDigit(c) || c == '_';

    internal static string FormatValue(this object sender)
    {
      if (sender == null)
      {
        return "null";
      }

      return sender.ToString();
    }
  }
  
  internal static class ObjectArrayExtensions
  {
    public static IEnumerable<object> Keys(this IEnumerable<object> sender) => sender.Where((k, i) => i % 2 == 0);
  }
}