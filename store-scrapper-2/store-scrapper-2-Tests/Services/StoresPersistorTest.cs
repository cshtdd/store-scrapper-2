using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using store_scrapper_2;
using store_scrapper_2.DataTransmission;
using store_scrapper_2.Model;
using store_scrapper_2.Services;
using store_scrapper_2_Tests.Factory;
using Xunit;

namespace store_scrapper_2_Tests.Services
{
  public class StoresPersistorTest
  {
    private readonly IStoreInfoResponseDataService _dataService = Substitute.For<IStoreInfoResponseDataService>();
    private readonly IStorePersistenceCalculator _persistenceCalculator = Substitute.For<IStorePersistenceCalculator>();

    private readonly IStoresPersistor _storesPersistor;

    public StoresPersistorTest() => _storesPersistor = new StoresPersistor(_dataService, _persistenceCalculator);

    [Fact]
    public async Task PersistsMultipleStores()
    {
      var allStoreNumbers = StoreNumberFactory.Create(10).ToArray();
      var stores = allStoreNumbers.Select(StoreInfoFactory.Create).ToArray();

      var recentlyPersistedNumbers = new[] { allStoreNumbers[1], allStoreNumbers[3] };
      foreach (var storeNumber in recentlyPersistedNumbers)
      {
        _persistenceCalculator.WasPersistedRecently(storeNumber).Returns(true);
      }
      
      var numbersToPersist = allStoreNumbers.Except(recentlyPersistedNumbers).ToArray();
      var numbersThatNeedToBeUpdated = new[] { numbersToPersist[0], numbersToPersist[1], numbersToPersist[2] };
      _dataService.ContainsStoreAsync(Arg.Is<IEnumerable<StoreNumber>>(_ => _.SequenceEqual(numbersToPersist)))
        .Returns(numbersThatNeedToBeUpdated);
      
      var numbersThatNeedToBeCreated = numbersToPersist.Except(numbersThatNeedToBeUpdated).ToArray();    
      
      
      await _storesPersistor.PersistAsync(stores);

      
      await _dataService.Received(1)
        .ContainsStoreAsync(Arg.Is<IEnumerable<StoreNumber>>(_ => _.SequenceEqual(numbersToPersist)));
      await _dataService.Received(1)
        .UpdateAsync(Arg.Is<IEnumerable<StoreInfo>>(_ => _.Select(s => s.StoreNumber).SequenceEqual(numbersThatNeedToBeUpdated)));
      await _dataService.Received(1)
        .CreateNewAsync(Arg.Is<IEnumerable<StoreInfo>>(_ => _.Select(s => s.StoreNumber).SequenceEqual(numbersThatNeedToBeCreated)));
      
      foreach (var storeNumber in allStoreNumbers)
      {
        _persistenceCalculator.Received(1).WasPersistedRecently(storeNumber);
      }
      foreach (var storeNumber in numbersToPersist)
      {
        _persistenceCalculator.Received(1).PreventFuturePersistence(storeNumber);
      }    
      foreach (var storeNumber in recentlyPersistedNumbers)
      {
        _persistenceCalculator.DidNotReceive().PreventFuturePersistence(storeNumber);
      }
    }

    [Fact]
    public async Task DoesNotDoResourceIntensiveOperationsWhenAnEmptyListIsPassed()
    {
      await _storesPersistor.PersistAsync(null);
      await _storesPersistor.PersistAsync(new StoreInfo[] {});
      
      _persistenceCalculator.DidNotReceiveWithAnyArgs().PreventFuturePersistence(Arg.Any<StoreNumber>());
      await _dataService.DidNotReceiveWithAnyArgs().ContainsStoreAsync(Arg.Any<IEnumerable<StoreNumber>>());
      await _dataService.DidNotReceiveWithAnyArgs().CreateNewAsync(Arg.Any<IEnumerable<StoreInfo>>());
      await _dataService.DidNotReceiveWithAnyArgs().UpdateAsync(Arg.Any<IEnumerable<StoreInfo>>());
    }
    
    [Fact]
    public async Task DoesNotDoResourceIntensiveOperationsWhenNoStoreShouldBePersisted()
    {
      var allStoreNumbers = StoreNumberFactory.Create(10).ToArray();
      var stores = allStoreNumbers.Select(StoreInfoFactory.Create).ToArray();
      foreach (var storeNumber in allStoreNumbers)
      {
        _persistenceCalculator.WasPersistedRecently(storeNumber).Returns(true);
      }

      await _storesPersistor.PersistAsync(stores);
      
      _persistenceCalculator.DidNotReceiveWithAnyArgs().PreventFuturePersistence(Arg.Any<StoreNumber>());
      await _dataService.DidNotReceiveWithAnyArgs().ContainsStoreAsync(Arg.Any<IEnumerable<StoreNumber>>());
      await _dataService.DidNotReceiveWithAnyArgs().CreateNewAsync(Arg.Any<IEnumerable<StoreInfo>>());
      await _dataService.DidNotReceiveWithAnyArgs().UpdateAsync(Arg.Any<IEnumerable<StoreInfo>>());
    }
  }
}