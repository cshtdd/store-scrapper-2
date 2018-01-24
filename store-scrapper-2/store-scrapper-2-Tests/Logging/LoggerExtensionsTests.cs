using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using NSubstitute;
using store_scrapper_2.Logging;
using Xunit;

namespace store_scrapper_2_Tests.Logging
{
  public class LoggerExtensionsTests
  {
    private struct LogEntry
    {
      public string Level { get; set; }
      public string Message { get; set; }
      public Exception Error { get; set; }

      public override string ToString()
      {
        var exceptionStr = Error != null ? Error.GetType().Name : "null";
        return $"{Level} {Message} {exceptionStr}";
      }
    }

    private readonly List<LogEntry> loggedEntries = new List<LogEntry>();

    private readonly ILog logger = Substitute.For<ILog>();

    public LoggerExtensionsTests()
    {
      logger.WhenForAnyArgs(_ => _.Info(Arg.Any<string>()))
        .Do(_ => loggedEntries.Add(new LogEntry
        {
          Level = "INFO",
          Message = _[0] as string
        }));
    }

    [Fact]
    public void InfoLogsTheCorrectThings()
    {
      logger.Info("msg1", "key1", 123);
      logger.Info("msg2");
      logger.Info("msg3", "key1", 123, "key2", "big wave");

      loggedEntries.Select(_ => _.ToString()).SequenceEqual(new []
      {
        "INFO message:msg1, key1:123 null", 
        "INFO message:msg2 null", 
        "INFO message:msg1, key1:123, key2:\"big wave\" null" 
      });
    }
  }
}