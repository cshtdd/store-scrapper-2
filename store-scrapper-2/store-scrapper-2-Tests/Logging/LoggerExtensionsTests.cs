using System;
using System.Collections.Generic;
using FluentAssertions;
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

      
      public LogEntry(object level, object message)
        : this(level as string, message as string) { }
      public LogEntry(object level, object message, object error)
        : this(level as string, message as string, error as Exception) { }
      public LogEntry(string level, string message)
        : this(level, message, null) { }
      public LogEntry(string level, string message, Exception error)
      {
        this.Level = level;
        this.Message = message;
        this.Error = error;
      }

      public override string ToString()
      {
        var exceptionStr = Error != null ? Error.GetType().Name : "null";
        return $"{Level} {Message} {exceptionStr}";
      }
    }

    private readonly List<string> loggedEntries = new List<string>();

    private readonly ILog logger = Substitute.For<ILog>();

    public LoggerExtensionsTests()
    {
      logger.WhenForAnyArgs(_ => _.Debug(Arg.Any<string>()))
        .Do(_ => loggedEntries.Add(new LogEntry("DEBUG", _[0]).ToString()));
      logger.WhenForAnyArgs(_ => _.Debug(Arg.Any<string>(), Arg.Any<Exception>()))
        .Do(_ => loggedEntries.Add(new LogEntry("DEBUG", _[0], _[1]).ToString()));
      
      logger.WhenForAnyArgs(_ => _.Info(Arg.Any<string>()))
        .Do(_ => loggedEntries.Add(new LogEntry("INFO", _[0]).ToString()));
      logger.WhenForAnyArgs(_ => _.Info(Arg.Any<string>(), Arg.Any<Exception>()))
        .Do(_ => loggedEntries.Add(new LogEntry("INFO", _[0], _[1]).ToString()));
      
      logger.WhenForAnyArgs(_ => _.Warn(Arg.Any<string>()))
        .Do(_ => loggedEntries.Add(new LogEntry("WARN", _[0]).ToString()));
      logger.WhenForAnyArgs(_ => _.Warn(Arg.Any<string>(), Arg.Any<Exception>()))
        .Do(_ => loggedEntries.Add(new LogEntry("WARN", _[0], _[1]).ToString()));
      
      logger.WhenForAnyArgs(_ => _.Error(Arg.Any<string>()))
        .Do(_ => loggedEntries.Add(new LogEntry("ERROR", _[0]).ToString()));      
      logger.WhenForAnyArgs(_ => _.Error(Arg.Any<string>(), Arg.Any<Exception>()))
        .Do(_ => loggedEntries.Add(new LogEntry("ERROR", _[0], _[1]).ToString()));
    }

    [Fact]
    public void InfoLogsTheCorrectThings()
    {
      logger.LogInfo("msg1", "key1", 123);
      logger.LogInfo("msg2");
      logger.LogInfo("msg3", "key1", 123, "key2", "big wave");
      logger.LogInfo("msg4", new ArgumentNullException(), "key1", 123);
      logger.LogInfo("msg5", new InvalidOperationException());

      loggedEntries.ShouldBeEquivalentTo(new []
      {
        "INFO message:msg1, key1:123 null", 
        "INFO message:msg2 null", 
        "INFO message:msg3, key1:123, key2:\"big wave\" null", 
        "INFO message:msg4, key1:123 ArgumentNullException",
        "INFO message:msg5 InvalidOperationException"
      });
    }
  }
}