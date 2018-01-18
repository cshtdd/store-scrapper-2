using System.Threading.Tasks;
using NSubstitute;
using store_scrapper_2;
using store_scrapper_2.DataTransmission;
using store_scrapper_2.Model;
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
    private readonly IStorePersistenceCalculator _persistenceCalculator = Substitute.For<IStorePersistenceCalculator>();
    
    [Fact]
    public async Task InsertsTheStoreDataIfItIsNew()
    {
      _dataService.ContainsStoreAsync("77754-4").Returns(Task.FromResult(false));
      
      await new SingleStorePersistor(_dataService, _persistenceCalculator).PersistAsync(_storeInfo);

      await _dataService
        .Received(1)
        .CreateNewAsync(Arg.Is(_storeInfo));
      _persistenceCalculator
        .Received(1)
        .PreventFuturePersistence("77754-4");
    }
    
    [Fact]
    public async Task UpdatesTheStoreDataIfItAlreadyExists()
    {
      _dataService.ContainsStoreAsync("77754-4").Returns(Task.FromResult(true));
      
      await new SingleStorePersistor(_dataService, _persistenceCalculator).PersistAsync(_storeInfo);

      await _dataService
        .Received(1)
        .UpdateAsync(Arg.Is(_storeInfo));
      _persistenceCalculator
        .Received(1)
        .PreventFuturePersistence("77754-4");
    }
    
    [Fact]
    public async Task DoesNotDoAnyDataOperationWhenTheStoreWasPersistedRecently()
    {
      _persistenceCalculator.WasPersistedRecently("77754-4").Returns(true);
      
      await new SingleStorePersistor(_dataService, _persistenceCalculator).PersistAsync(_storeInfo);

      await _dataService
        .DidNotReceiveWithAnyArgs()
        .ContainsStoreAsync(Arg.Any<StoreNumber>());
      await _dataService
        .DidNotReceiveWithAnyArgs()
        .CreateNewAsync(Arg.Any<StoreInfo>());
      await _dataService
        .DidNotReceiveWithAnyArgs()
        .UpdateAsync(Arg.Any<StoreInfo>());
      _persistenceCalculator
        .DidNotReceiveWithAnyArgs()
        .PreventFuturePersistence(Arg.Any<StoreNumber>());
    }
  }
}