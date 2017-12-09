using System.Threading.Tasks;
using NSubstitute;
using store_scrapper_2;
using store_scrapper_2.DataTransmission;
using store_scrapper_2.Services;
using Xunit;

namespace store_scrapper_2_Tests.Services
{
  public class SingleStorePersistorTest
  {
    private readonly StoreInfo _storeInfo = new StoreInfo
    {
      StoreNumber = "77754-4",
      Address1 = "seeded address"
    };

    private readonly IStoreInfoResponseDataService _dataService = Substitute.For<IStoreInfoResponseDataService>();

    [Fact]
    public async void InsertsTheStoreDataIfItIsNew()
    {
      _dataService.ContainsStoreAsync("77754-4").Returns(Task.FromResult(false));
      
      await new SingleStorePersistor(_dataService).PersistAsync(_storeInfo);

      await _dataService
        .Received(1)
        .CreateNewAsync(Arg.Is(_storeInfo));
    }
    
    [Fact]
    public async void UpdatesTheStoreDataIfItAlreadyExists()
    {
      _dataService.ContainsStoreAsync("77754-4").Returns(Task.FromResult(true));
      
      await new SingleStorePersistor(_dataService).PersistAsync(_storeInfo);

      await _dataService
        .Received(1)
        .UpdateAsync(Arg.Is(_storeInfo));
    }    
  }
}