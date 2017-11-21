using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using store;
using store_scrapper_2.DataTransmission;
using store_scrapper_2.DAL;

namespace store_scrapper_2
{
  static class Program
  {
    // ReSharper disable once UnusedParameter.Local
    public static async Task Main(string[] args)
    {
      Console.WriteLine($"Launching Program with args={string.Join(",", args)}");

      ConfigureMappings();
      
      await MigrateTheDatabase();

      var response = await DownloadStoreData();
      Console.WriteLine($"response={response}");

      await SaveStoreData(response);

      Console.WriteLine("Ending program");
    }

    private static void ConfigureMappings()
    {
      Mapper.Initialize(_ =>
      {
        _.CreateMap<StoreInfoResponse, Store>();
      });
    }

    private static async Task MigrateTheDatabase()
    {
      Console.WriteLine("Migrating the database");
      await new StoreDataContext().Database.MigrateAsync();
    }

    private static async Task SaveStoreData(StoreInfoResponse response)
    {
      Console.WriteLine($"Saving Response");

      await CreateResponseDataService().SaveAsync(response);
    }

    private static async Task<StoreInfoResponse> DownloadStoreData()
    {
      var request = new StoreInfoRequest("11111", "0");

      Console.WriteLine($"Sending Request: {request}");

      return await CreateDownloader().DownloadAsync(request);
    }

    private static StoreInfoDownloader CreateDownloader() => new StoreInfoDownloader(new UrlDownloader());
    private static IStoreInfoResponseDataService CreateResponseDataService() => new StoreInfoResponseDataService();
  }
}
