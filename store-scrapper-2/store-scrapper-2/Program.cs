using System.Linq;
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

      await InitializeAsync();

      var zipCodeDataService = CreateZipCodeDataService();
      var zipCodes = await Task.WhenAll(args.Select(zipCodeDataService.ReadAsync));
      
      await CreateProcessor().ProcessAsync(zipCodes);
      
      Logger.Info("Ending program");
    }

    private static IZipCodeDataService CreateZipCodeDataService() => IocContainer.Resolve<IZipCodeDataService>(); 
    private static IMultipleZipCodeProcessor CreateProcessor() => IocContainer.Resolve<IMultipleZipCodeProcessor>();
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