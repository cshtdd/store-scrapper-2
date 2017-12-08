using System.Collections.Generic;
using System.Linq;
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
    public async void DownloadsAndPersistsTheStoreData()
    {
      var downloader = Substitute.For<IStoreInfoDownloader>();
      downloader.DownloadAsync(Arg.Any<StoreInfoRequest>())
        .Returns(Task.FromResult((IEnumerable<StoreInfoResponse>) new []
        {
          new StoreInfoResponse{ StoreNumber = "55555-3" },
          new StoreInfoResponse{ StoreNumber = "66666-7" }
        }));

      var persistor = Substitute.For<ISingleStorePersistor>();
      
      
      await new SingleStoreProcessor(downloader, persistor)
        .ProcessAsync("55555-3");

      
      await downloader
        .Received(1)
        .DownloadAsync(Arg.Is<StoreInfoRequest>(_ => _.StoreNumber == "55555-3"));

      
      persistor.Received(2).PersistAsync(Arg.Any<StoreInfoResponse>());

      await persistor
        .Received(1)
        .PersistAsync(Arg.Is<StoreInfoResponse>(_ => _.StoreNumber == "55555-3"));

      await persistor
        .Received(1)
        .PersistAsync(Arg.Is<StoreInfoResponse>(_ => _.StoreNumber == "66666-7"));
    }
  }
}