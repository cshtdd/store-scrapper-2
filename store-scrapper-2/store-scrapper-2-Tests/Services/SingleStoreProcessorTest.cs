using System.Collections.Generic;
using System.Threading.Tasks;
using NSubstitute;
using store_scrapper_2;
using store_scrapper_2.DataTransmission;
using store_scrapper_2.Services;
using Xunit;

namespace store_scrapper_2_Tests.Services
{
  public class SingleStoreProcessorTest
  {
    [Fact]
    public async void DownloadsTheStoreData()
    {
      var downloader = Substitute.For<IStoreInfoDownloader>();
      downloader.DownloadAsync(Arg.Any<StoreInfoRequest>())
        .Returns(Task.FromResult((IEnumerable<StoreInfoResponse>) new [] { new StoreInfoResponse() }));

      await new SingleStoreProcessor(downloader, Substitute.For<IStoreInfoResponseDataService>())
        .ProcessAsync("55555-3");

      await downloader
        .Received(1)
        .DownloadAsync(Arg.Is<StoreInfoRequest>(_ => _.StoreNumber == "55555-3"));
    }

    [Fact]
    public async void InsertsTheStoreDataIfItIsNew()
    {
      var seededResponse = new StoreInfoResponse { Address1 = "seeded address" };

      var downloader = Substitute.For<IStoreInfoDownloader>();
      downloader.DownloadAsync(Arg.Any<StoreInfoRequest>())
        .Returns(Task.FromResult((IEnumerable<StoreInfoResponse>) new [] { seededResponse }));

      var dataService = Substitute.For<IStoreInfoResponseDataService>();
      dataService.ContainsStoreAsync("77754-4").Returns(Task.FromResult(false));
      
      await new SingleStoreProcessor(downloader, dataService)
        .ProcessAsync("77754-4");

      await dataService
        .Received(1)
        .CreateNewAsync(Arg.Is(seededResponse));
    }
    
    [Fact]
    public async void UpdatesTheStoreDataIfItAlreadyExists()
    {
      var seededResponse = new StoreInfoResponse
      {
        StoreNumber = "77754-4",
        Address1 = "seeded address"
      };

      var downloader = Substitute.For<IStoreInfoDownloader>();
      downloader.DownloadAsync(Arg.Any<StoreInfoRequest>())
        .Returns(Task.FromResult((IEnumerable<StoreInfoResponse>) new [] { seededResponse }));

      var dataService = Substitute.For<IStoreInfoResponseDataService>();
      dataService.ContainsStoreAsync("77754-4").Returns(Task.FromResult(true));
      
      await new SingleStoreProcessor(downloader, dataService)
        .ProcessAsync("77754-4");

      await dataService
        .Received(1)
        .UpdateAsync(Arg.Is(seededResponse));
    }
  }
}