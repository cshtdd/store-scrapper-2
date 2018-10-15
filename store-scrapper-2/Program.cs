using System;
using System.Reflection;

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
    public static void Main(string[] args)
    {
      LogConfiguration.Initialize(LogConfiguration.Source.File);

      try
      {
        Logger.LogInfo("Launching Program", nameof(args), string.Join(",", args));

        Initialize();
        IocContainer.Resolve<IAllZipCodesProcessor>().Process();

        Logger.LogInfo("Ending program", "success", true);
      }
      catch (Exception ex)
      {
        Logger.LogError("Ending program", ex, "success", false);
        throw;
      }
    }

    private static void Initialize()
    {
      IocContainer.Initialize();
      Mappings.Configure();
      IocContainer.Resolve<IPersistenceInitializer>().Initialize();
    }
  }
}