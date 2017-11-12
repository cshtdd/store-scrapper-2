using System;
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

      var downloader = new UrlDownloader();

      var requestUrl = new StoreInfoRequest("11111", "0").ToString();

      var responseJson = await downloader.DownloadAsync(requestUrl);

      Console.WriteLine("Received Response:");
      
      var response = StoreInfoResponse.Parse(responseJson);

      Console.WriteLine(response);
    }
  }
}
