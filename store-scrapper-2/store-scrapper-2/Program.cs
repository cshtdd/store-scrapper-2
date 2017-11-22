using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Repository;
using store;
using store_scrapper_2.Configuration;
using store_scrapper_2.DataTransmission;

namespace store_scrapper_2
{
  static class Program
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    // ReSharper disable once UnusedParameter.Local
    public static async Task Main(string[] args)
    {
      ConfigureLogging();
      
      log.Info($"Launching Program with args={string.Join(",", args)}");

      await Initialize();
      await CreateSingleStoreProcessor().Process(args[0]);
      
      log.Info("Ending program");
    }

    private static void ConfigureLogging()
    {
      BasicConfigurator.Configure(
        LogManager.GetRepository(Assembly.GetEntryAssembly()));
    }

    private static SingleStoreProcessor CreateSingleStoreProcessor() => new SingleStoreProcessor(
      new StoreInfoDownloader(new UrlDownloader()), new StoreInfoResponseDataService()
    );

    private static async Task Initialize()
    {
      Mappings.Configure();
      await DbMigrator.MigrateAsync();
    }
  }
}