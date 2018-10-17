using System.IO;
using System.Reflection;
using System.Xml;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace store_scrapper_2.Configuration
{
  public static class LogConfiguration
  {
    public enum Source
    {
      Memory,
      File
    }
    
    public static void Initialize(Source source = Source.Memory)
    {
      if (source == Source.File)
      {
        FileConfiguration.Load("log4net.config");
        return;
      }
      
      MemoryConfiguration.Load();
    }

    public static void Shutdown()
    {
      LogManager.Flush(30000);
      LogManager.Shutdown();
    }
    
    private static class FileConfiguration
    {     
      public static void Load(string configFilename)
      {
        var log4NetConfig = new XmlDocument();
        log4NetConfig.Load(File.OpenRead(configFilename));

        var repo = LogManager.CreateRepository(
          Assembly.GetEntryAssembly(),
          typeof(Hierarchy));

        XmlConfigurator.Configure(repo, log4NetConfig["log4net"]);
      }
    }

    private static class MemoryConfiguration
    {
      private static string GetLogFilename() => $"logs/{Assembly.GetEntryAssembly().GetName().Name}.log";
      private static string GetLogFormat() => "level:%p, timestamp:\"%utcdate{yyyy-MM-dd HH:mm:ss}\", actor:%logger{1}, %message%newline";

      public static void Load()
      {
        var hierarchy = (Hierarchy)LogManager.GetRepository(Assembly.GetEntryAssembly());

        var appenders = new[]
        {
          GetRollingFileAppender(),
          GetConsoleAppender()
        };
      
        foreach (var appender in appenders)
        {
          (appender as IOptionHandler)?.ActivateOptions();
          hierarchy.Root.AddAppender(appender);       
        }
      
        hierarchy.Root.Level = Level.All;
        hierarchy.Configured = true;
      }

      private static IAppender GetConsoleAppender() => new ConsoleAppender
      {
        Layout = GetLayout()
      };

      private static IAppender GetRollingFileAppender() => new RollingFileAppender
      {
        LockingModel = GetLockingModel(),
        File = GetLogFilename(),
        StaticLogFileName = true,
        AppendToFile = true,
        RollingStyle = RollingFileAppender.RollingMode.Composite,
        MaxSizeRollBackups = 10,
        MaximumFileSize = "5MB",
        Layout = GetLayout()
      };

      private static FileAppender.MinimalLock GetLockingModel() => new FileAppender.MinimalLock();

      private static PatternLayout GetLayout()
      {
        var patternLayout = new PatternLayout
        {
          ConversionPattern = GetLogFormat()
        };
        patternLayout.ActivateOptions();
        return patternLayout;
      }

    }    
  }
}