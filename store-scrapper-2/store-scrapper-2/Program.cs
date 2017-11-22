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

      var response = await DownloadStoreData();
      Console.WriteLine($"response={response}");

      await SaveStoreData(response);

      Console.WriteLine("Ending program");
    }

    private static async Task Initialize()
    {
      Mappings.Configure();
      await DbMigrator.MigrateAsync();
    }

    private static async Task SaveStoreData(StoreInfoResponse response)
    {
      Console.WriteLine($"Saving Response");

      var dataService = CreateResponseDataService();

      bool shouldUpdateStore = await dataService.ContainsStoreAsync(response.StoreNumber, response.SatelliteNumber);

      if (shouldUpdateStore)
      {
        Console.WriteLine($"Updating Existing Store Info");
        await dataService.UpdateAsync(response);
      }
      else
      {
        Console.WriteLine($"Creating new Store Info");
        await dataService.CreateNewAsync(response);       
      }
    }

    private static async Task<StoreInfoResponse> DownloadStoreData()
    {
      var request = new StoreInfoRequest("11111", "0");

      Console.WriteLine($"Sending Request: {request}");

      return await CreateDownloader().DownloadAsync(request);
    }

    private static IStoreInfoDownloader CreateDownloader() => new StoreInfoDownloader(new UrlDownloader());
    private static IStoreInfoResponseDataService CreateResponseDataService() => new StoreInfoResponseDataService();
  }
}