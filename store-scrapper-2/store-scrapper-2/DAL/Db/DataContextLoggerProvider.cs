using System;
using Microsoft.Extensions.Logging;

namespace store_scrapper_2.DAL.Db
{
  internal class DataContextLoggerProvider : ILoggerProvider
  {
    private readonly bool _isEnabled;
    private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public static ILoggerFactory CreateFactory(bool isEnabled)
    {
      var loggerFactory = new LoggerFactory();
      loggerFactory.AddProvider(new DataContextLoggerProvider(isEnabled));
      return loggerFactory;
    }
    
    private DataContextLoggerProvider(bool isEnabled) => _isEnabled = isEnabled;

    public void Dispose() { }

    public ILogger CreateLogger(string categoryName) => new DataContextLogger(_isEnabled);

    private class DataContextLogger : ILogger
    {
      private readonly bool _isEnabled;

      public DataContextLogger(bool isEnabled) => _isEnabled = isEnabled;

      public void Log<TState>(
        LogLevel logLevel,
        EventId eventId, 
        TState state, 
        Exception exception,
        Func<TState, Exception, string> formatter)
      { 
        Logger.Debug($"logLevel={logLevel}; eventId={eventId}; {formatter(state, exception)}");
      }

      public bool IsEnabled(LogLevel logLevel) => _isEnabled;

      public IDisposable BeginScope<TState>(TState state) => null;
    }
  }
}