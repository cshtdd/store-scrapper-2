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

      var requestUrl = new StoreInfoRequest("11111").ToString();

      var rawResponse1 = await downloader.DownloadAsync(requestUrl);
      var rawResponse2 = downloader.Download(requestUrl);

      Console.WriteLine($"The two responses match {rawResponse1 == rawResponse2}");
      
      Console.WriteLine("Received Response1:");
      Console.WriteLine(rawResponse1);
    }
  }
}
