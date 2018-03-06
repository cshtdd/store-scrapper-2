using System;
using System.Reflection;
using System.Threading.Tasks;
using log4net;
using store_scrapper_2.Configuration;
using store_scrapper_2.Logging;
using store_scrapper_2.Services;

namespace store_scrapper_2
{
  internal static class Program
  {
    private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    // ReSharper disable once UnusedParameter.Local
    public static async Task Main(string[] args)
    {
      LogConfiguration.Initialize(LogConfiguration.Source.File);

      try
      {
        Logger.LogInfo("Launching Program", nameof(args), string.Join(",", args));

        await InitializeAsync();
        await IocContainer.Resolve<IAllZipCodesProcessor>().ProcessAsync();

        Logger.LogInfo("Ending program", "success", true);
      }
      catch (Exception ex)
      {
        Logger.LogError("Ending program", ex, "success", false);
        throw;
      }
    }

    private static async Task InitializeAsync()
    {
      IocContainer.Initialize();
      Mappings.Configure();
      await IocContainer.Resolve<IPersistenceInitializer>().InitializeAsync();
    }
  }
}