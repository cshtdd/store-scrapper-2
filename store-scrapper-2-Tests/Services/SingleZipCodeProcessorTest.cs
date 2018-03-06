using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using store_scrapper_2;
using store_scrapper_2.Model;
using store_scrapper_2.DataTransmission;
using store_scrapper_2.Services;
using store_scrapper_2_Tests.Factory;
using Xunit;

namespace store_scrapper_2_Tests.Services
{
  public class SingleZipCodeProcessorTest
  {
    [Fact]
    public async Task DownloadsAndPersistsTheStoreData()
    {
      var stores = StoreNumberFactory
        .Create(10)
        .Select(StoreInfoFactory.Create)
        .ToArray();
      
      var downloader = Substitute.For<IStoreInfoDownloader>();
      downloader.DownloadAsync(Arg.Any<ZipCode>())
        .Returns(Task.FromResult<IEnumerable<StoreInfo>>(stores));

      var persistor = Substitute.For<IStoresPersistor>();
      var zipCodeDataService = Substitute.For<IZipCodeDataService>();


      var zipCode = new ZipCode("55555", 17, 45);
      await new SingleZipCodeProcessor(downloader, persistor, zipCodeDataService)
        .ProcessAsync(zipCode);

      
      await downloader
        .Received(1)
        .DownloadAsync(zipCode);

      await persistor
        .Received(1)
        .PersistAsync(Arg.Is<IEnumerable<StoreInfo>>(_ => _.SequenceEqual(stores)));

      await zipCodeDataService
        .Received(1)
        .UpdateZipCodeAsync("55555");
    }
  }
}