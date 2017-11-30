using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using log4net;
using log4net.Config;
using store_scrapper_2.Configuration;
using store_scrapper_2.DataTransmission;

namespace store_scrapper_2
{
  internal static class Program
  {
    private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    // ReSharper disable once UnusedParameter.Local
    public static async Task Main(string[] args)
    {
      ConfigureLogging();
      
      Logger.Info($"Launching Program with args={string.Join(",", args)}");

      await Initialize();
      await CreateSingleStoreProcessor().Process(args[0]);
      
      Logger.Info("Ending program");
    }

    private static void ConfigureLogging()
    {
      var log4NetConfig = new XmlDocument();
      log4NetConfig.Load(File.OpenRead("log4net.config"));

      var repo = LogManager.CreateRepository(
        Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));

      XmlConfigurator.Configure(repo, log4NetConfig["log4net"]);
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