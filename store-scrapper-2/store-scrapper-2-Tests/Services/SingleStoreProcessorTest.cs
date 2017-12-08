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
        .Returns(Task.FromResult(new StoreInfoResponse()));

      await new SingleStoreProcessor(downloader, Substitute.For<IStoreInfoResponseDataService>())
        .Process("55555-3");

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
        .Returns(Task.FromResult(seededResponse));

      var dataService = Substitute.For<IStoreInfoResponseDataService>();
      dataService.ContainsStoreAsync("77754-4").Returns(Task.FromResult(false));
      
      await new SingleStoreProcessor(downloader, dataService)
        .Process("77754-4");

      await dataService
        .Received(1)
        .CreateNewAsync(Arg.Is(seededResponse));
    }
    
    [Fact]
    public async void UpdatesTheStoreDataIfItAlreadyExists()
    {
      var seededResponse = new StoreInfoResponse { Address1 = "seeded address" };

      var downloader = Substitute.For<IStoreInfoDownloader>();
      downloader.DownloadAsync(Arg.Any<StoreInfoRequest>())
        .Returns(Task.FromResult(seededResponse));

      var dataService = Substitute.For<IStoreInfoResponseDataService>();
      dataService.ContainsStoreAsync("77754-4").Returns(Task.FromResult(true));
      
      await new SingleStoreProcessor(downloader, dataService)
        .Process("77754-4");

      await dataService
        .Received(1)
        .UpdateAsync(Arg.Is(seededResponse));
    }
  }
}