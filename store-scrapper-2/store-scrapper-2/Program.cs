using System;
using System.Threading.Tasks;
using store;
using store_scrapper_2.Configuration;
using store_scrapper_2.DataTransmission;

namespace store_scrapper_2
{
  static class Program
  {
    // ReSharper disable once UnusedParameter.Local
    public static async Task Main(string[] args)
    {
      Console.WriteLine($"Launching Program with args={string.Join(",", args)}");

      await Initialize();
      await CreateSingleStoreProcessor().Process(args[0]);
      
      Console.WriteLine("Ending program");
    }

    private static SingleStoreProcessor CreateSingleStoreProcessor() => new SingleStoreProcessor(new StoreInfoDownloader(new UrlDownloader()), new StoreInfoResponseDataService());

    private static async Task Initialize()
    {
      Mappings.Configure();
      await DbMigrator.MigrateAsync();
    }
  }
}