using System;
using System.IO;
using System.Net;
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

      var rawResponse = await downloader.Download(requestUrl);

      Console.WriteLine("Received Response:");
      Console.WriteLine(rawResponse);
    }
  }
}