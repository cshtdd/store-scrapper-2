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
  public class SingleZipCodeProcessorTest
  {
    [Fact]
    public async void DownloadsAndPersistsTheStoreData()
    {
      var downloader = Substitute.For<IStoreInfoDownloader>();
      downloader.DownloadAsync(Arg.Any<ZipCode>())
        .Returns(Task.FromResult((IEnumerable<StoreInfo>) new []
        {
          new StoreInfo{ StoreNumber = "55555-3" },
          new StoreInfo{ StoreNumber = "66666-7" }
        }));

      var persistor = Substitute.For<ISingleStorePersistor>();
      
      
      await new SingleZipCodeProcessor(downloader, persistor)
        .ProcessAsync(new ZipCode("55555", 17, 45));

      
      await downloader
        .Received(1)
        .DownloadAsync(Arg.Is<ZipCode>(_ => _.Zip == "55555" && _.Latitude == 17 && _.Longitude == 45));

      
      await persistor
        .Received(2)
        .PersistAsync(Arg.Any<StoreInfo>());

      await persistor
        .Received(1)
        .PersistAsync(Arg.Is<StoreInfo>(_ => _.StoreNumber == "55555-3"));

      await persistor
        .Received(1)
        .PersistAsync(Arg.Is<StoreInfo>(_ => _.StoreNumber == "66666-7"));
    }
  }
}