using System.Threading.Tasks;
using NSubstitute;
using store_scrapper_2;
using store_scrapper_2.DataTransmission;
using Xunit;

namespace store_screapper_2_Tests
{
  public class SingleStoreProcessorTest
  {
    [Fact]
    public async void DownloadsTheStoreData()
    {
      var downloader = Substitute.For<IStoreInfoDownloader>();
      downloader.DownloadAsync(Arg.Is<StoreInfoRequest>(_ => _.FullStoreNumber == "55555-3"))
        .Returns(Task.FromResult(new StoreInfoResponse()));
      
      var processor = new SingleStoreProcessor(downloader, null);

      await processor.Process("55555-3");

      downloader
        .Received(1)
        .DownloadAsync(Arg.Is<StoreInfoRequest>(_ => _.FullStoreNumber == "55555-3"));
    }
  }
}