using System;
using Microsoft.Extensions.Logging;

namespace store_scrapper_2.DAL.Db
{
  internal class DataContextLoggerProvider : ILoggerProvider
  {
    private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public static ILoggerFactory CreateFactory()
    {
      var loggerFactory = new LoggerFactory();
      loggerFactory.AddProvider(new DataContextLoggerProvider());
      return loggerFactory;
    }
    
    public void Dispose() { }

    public ILogger CreateLogger(string categoryName) => new DataContextLogger();

    private class DataContextLogger : ILogger
    {
      public void Log<TState>(
        LogLevel logLevel,
        EventId eventId, 
        TState state, 
        Exception exception,
        Func<TState, Exception, string> formatter)
      { 
        Logger.Debug($"logLevel={logLevel}; eventId={eventId}; {formatter(state, exception)}");
      }

      public bool IsEnabled(LogLevel logLevel) => true;

      public IDisposable BeginScope<TState>(TState state) => null;
    }
  }
}