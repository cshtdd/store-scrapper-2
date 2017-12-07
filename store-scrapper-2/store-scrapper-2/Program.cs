using System.Reflection;
using System.Threading.Tasks;
using log4net;
using store_scrapper_2.Configuration;
using store_scrapper_2.Model;

namespace store_scrapper_2
{
  internal static class Program
  {
    private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    // ReSharper disable once UnusedParameter.Local
    public static async Task Main(string[] args)
    {
      Logging.Initialize();
      
      Logger.Info($"Launching Program with args={string.Join(",", args)}");

      await Initialize();
      var storeNumber = new StoreNumber(args[0]);
      await CreateSingleStoreProcessor().Process(storeNumber);
      
      Logger.Info("Ending program");
    }

    private static SingleStoreProcessor CreateSingleStoreProcessor() => IocContainer.Resolve<SingleStoreProcessor>();

    private static IPersistenceInitializer CreatePersistenceInitializer() =>
      IocContainer.Resolve<IPersistenceInitializer>();

    private static async Task Initialize()
    {
      IocContainer.Initialize();
      Mappings.Configure();
      await CreatePersistenceInitializer().InitializeAsync();
    }
  }
}