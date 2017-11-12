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
      var downloader = new StoreInfoDownloader(new UrlDownloader());

      Console.WriteLine($"Launching Program with args={string.Join(",", args)}");

      var request = new StoreInfoRequest("11111", "0");
      
      Console.WriteLine($"Sending Request: {request}");

      var response = await downloader.DownloadAsync(request);

      Console.WriteLine(response);
    }
  }
}
