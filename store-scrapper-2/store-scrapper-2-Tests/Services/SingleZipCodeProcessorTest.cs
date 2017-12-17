using System.Collections.Generic;
using System.Threading.Tasks;
using NSubstitute;
using store_scrapper_2;
using store_scrapper_2.Model;
using store_scrapper_2.DataTransmission;
using store_scrapper_2.Services;
using Xunit;

namespace store_scrapper_2_Tests.Services
{
  public class SingleZipCodeProcessorTest
  {
    [Fact]
    public async Task DownloadsAndPersistsTheStoreData()
    {
      var downloader = Substitute.For<IStoreInfoDownloader>();
      downloader.DownloadAsync(Arg.Any<ZipCode>())
        .Returns(Task.FromResult((IEnumerable<StoreInfo>) new []
        {
          new StoreInfo{ StoreNumber = "55555-3" },
          new StoreInfo{ StoreNumber = "66666-7" }
        }));

      var persistor = Substitute.For<ISingleStorePersistor>();
      var zipCodeDataService = Substitute.For<IZipCodeDataService>();


      var zipCode = new ZipCode("55555", 17, 45);
      await new SingleZipCodeProcessor(downloader, persistor, zipCodeDataService)
        .ProcessAsync(zipCode);

      
      await downloader
        .Received(1)
        .DownloadAsync(Arg.Is(zipCode));

      
      await persistor
        .Received(2)
        .PersistAsync(Arg.Any<StoreInfo>());

      await persistor
        .Received(1)
        .PersistAsync(Arg.Is<StoreInfo>(_ => _.StoreNumber == "55555-3"));

      await persistor
        .Received(1)
        .PersistAsync(Arg.Is<StoreInfo>(_ => _.StoreNumber == "66666-7"));

      await zipCodeDataService
        .Received(1)
        .UpdateZipCodeAsync("55555");
    }
  }
}