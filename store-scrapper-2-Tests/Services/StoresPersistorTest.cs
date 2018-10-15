using System.Collections.Generic;
using System.Linq;

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
    public void PersistsMultipleStores()
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
      _dataService.ContainsStore(Arg.Is<IEnumerable<StoreNumber>>(_ => _.SequenceEqual(numbersToPersist)))
        .Returns(numbersThatNeedToBeUpdated);
      
      var numbersThatNeedToBeCreated = numbersToPersist.Except(numbersThatNeedToBeUpdated).ToArray();    
      
      
      _storesPersistor.Persist(stores);

      
      _dataService.Received(1)
        .ContainsStore(Arg.Is<IEnumerable<StoreNumber>>(_ => _.SequenceEqual(numbersToPersist)));
      _dataService.Received(1)
        .Update(Arg.Is<IEnumerable<StoreInfo>>(_ => _.Select(s => s.StoreNumber).SequenceEqual(numbersThatNeedToBeUpdated)));
      _dataService.Received(1)
        .CreateNew(Arg.Is<IEnumerable<StoreInfo>>(_ => _.Select(s => s.StoreNumber).SequenceEqual(numbersThatNeedToBeCreated)));
      
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
    public void DoesNotDoResourceIntensiveOperationsWhenAnEmptyListIsPassed()
    {
      _storesPersistor.Persist(null);
      _storesPersistor.Persist(new StoreInfo[] {});
      
      _persistenceCalculator.DidNotReceiveWithAnyArgs().PreventFuturePersistence(Arg.Any<StoreNumber>());
      _dataService.DidNotReceiveWithAnyArgs().ContainsStore(Arg.Any<IEnumerable<StoreNumber>>());
      _dataService.DidNotReceiveWithAnyArgs().CreateNew(Arg.Any<IEnumerable<StoreInfo>>());
      _dataService.DidNotReceiveWithAnyArgs().Update(Arg.Any<IEnumerable<StoreInfo>>());
    }
    
    [Fact]
    public void DoesNotDoResourceIntensiveOperationsWhenNoStoreShouldBePersisted()
    {
      var allStoreNumbers = StoreNumberFactory.Create(10).ToArray();
      var stores = allStoreNumbers.Select(StoreInfoFactory.Create).ToArray();
      foreach (var storeNumber in allStoreNumbers)
      {
        _persistenceCalculator.WasPersistedRecently(storeNumber).Returns(true);
      }

      _storesPersistor.Persist(stores);
      
      _persistenceCalculator.DidNotReceiveWithAnyArgs().PreventFuturePersistence(Arg.Any<StoreNumber>());
      _dataService.DidNotReceiveWithAnyArgs().ContainsStore(Arg.Any<IEnumerable<StoreNumber>>());
      _dataService.DidNotReceiveWithAnyArgs().CreateNew(Arg.Any<IEnumerable<StoreInfo>>());
      _dataService.DidNotReceiveWithAnyArgs().Update(Arg.Any<IEnumerable<StoreInfo>>());
    }
  }
}