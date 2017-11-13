using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using store_scrapper_2.DataTransmission;

namespace store_scrapper_2
{
  static class Program
  {
    // ReSharper disable once UnusedParameter.Local
    public static async Task Main(string[] args)
    {
      Console.WriteLine($"Launching Program with args={string.Join(",", args)}");
      
      var response = await DownloadStore();
      Console.WriteLine($"response={response}");

      Console.WriteLine(new string('=', 16));

      Console.WriteLine("AAAAA");
      
      Console.WriteLine(new string('=', 16));
    }

    private static async Task<StoreInfoResponse> DownloadStore()
    {
      var request = new StoreInfoRequest("11111", "0");

      Console.WriteLine($"Sending Request: {request}");

      return await CreateDownloader().DownloadAsync(request);
    }

    private static StoreInfoDownloader CreateDownloader() => new StoreInfoDownloader(new UrlDownloader());
  }
}
