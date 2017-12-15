using System.Reflection;
using System.Threading.Tasks;
using log4net;
using store_scrapper_2.Configuration;
using store_scrapper_2.Services;

namespace store_scrapper_2
{
  internal static class Program
  {
    private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    // ReSharper disable once UnusedParameter.Local
    public static async Task Main(string[] args)
    {
      Logging.Initialize();
      
      Logger.Info($"Launching Program with {nameof(args)}={string.Join(",", args)}");

      var zip = args[0];
      await InitializeAsync();
      var zipCode = await CreateZipCodeDataService().ReadAsync(zip);
      await CreateProcessor().ProcessAsync(zipCode);
      
      Logger.Info("Ending program");
    }

    private static IZipCodeDataService CreateZipCodeDataService() => IocContainer.Resolve<IZipCodeDataService>(); 
    private static SingleZipCodeProcessor CreateProcessor() => IocContainer.Resolve<SingleZipCodeProcessor>();
    private static IPersistenceInitializer CreatePersistenceInitializer() =>
      IocContainer.Resolve<IPersistenceInitializer>();

    private static async Task InitializeAsync()
    {
      IocContainer.Initialize();
      Mappings.Configure();
      await CreatePersistenceInitializer().InitializeAsync();
    }
  }
}