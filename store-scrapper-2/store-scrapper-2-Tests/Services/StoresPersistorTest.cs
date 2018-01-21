using System;
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
    [Obsolete]
    private readonly StoreInfo _storeInfo = new StoreInfo
    {
      StoreNumber = "77754-4",
      Address1 = "seeded address"
    };

    private readonly IStoreInfoResponseDataService _dataService = Substitute.For<IStoreInfoResponseDataService>();
    private readonly IStorePersistenceCalculator _persistenceCalculator = Substitute.For<IStorePersistenceCalculator>();

    private readonly IStoresPersistor _storesPersistor;

    public StoresPersistorTest() => _storesPersistor = new StoresPersistor(_dataService, _persistenceCalculator);

    [Fact]
    [Obsolete]
    public async Task InsertsTheStoreDataIfItIsNew()
    {
      _dataService.ContainsStoreAsync(
          Arg.Is<IEnumerable<StoreNumber>>(_ => _.SequenceEqual(new StoreNumber[] {"77754-4"}))
      ).Returns(Task.FromResult<IEnumerable<StoreNumber>>(new StoreNumber[]{}));
      
      await _storesPersistor.PersistAsync(_storeInfo);

      await _dataService
        .Received(1)
        .CreateNewAsync(Arg.Is<IEnumerable<StoreInfo>>(_ => _.SequenceEqual(new []{ _storeInfo })));
      _persistenceCalculator
        .Received(1)
        .PreventFuturePersistence("77754-4");
    }
    
    [Fact]
    [Obsolete]
    public async Task UpdatesTheStoreDataIfItAlreadyExists()
    {
      _dataService.ContainsStoreAsync(
        Arg.Is<IEnumerable<StoreNumber>>(_ => _.SequenceEqual(new StoreNumber[] {"77754-4"}))
      ).Returns(Task.FromResult<IEnumerable<StoreNumber>>(new StoreNumber[]{"77754-4"}));
      
      await _storesPersistor.PersistAsync(_storeInfo);

      await _dataService
        .Received(1)
        .UpdateAsync(Arg.Is<IEnumerable<StoreInfo>>(_ => _.SequenceEqual(new [] { _storeInfo } )));
      _persistenceCalculator
        .Received(1)
        .PreventFuturePersistence("77754-4");
    }
    
    [Fact]
    [Obsolete]
    public async Task DoesNotDoAnyDataOperationWhenTheStoreWasPersistedRecently()
    {
      _persistenceCalculator.WasPersistedRecently("77754-4").Returns(true);
      
      await _storesPersistor.PersistAsync(_storeInfo);

      await _dataService
        .DidNotReceiveWithAnyArgs()
        .ContainsStoreAsync(Arg.Any<IEnumerable<StoreNumber>>());
      await _dataService
        .DidNotReceiveWithAnyArgs()
        .CreateNewAsync(Arg.Any<IEnumerable<StoreInfo>>());
      await _dataService
        .DidNotReceiveWithAnyArgs()
        .UpdateAsync(Arg.Any<IEnumerable<StoreInfo>>());
      _persistenceCalculator
        .DidNotReceiveWithAnyArgs()
        .PreventFuturePersistence(Arg.Any<StoreNumber>());
    }
    
    [Fact]
    public async Task PersistsMultipleStores()
    {
      var allStoreNumbers = StoreNumberFactory.Create(10).ToArray();
      var recentlyPersistedNumbers = new[] { allStoreNumbers[1], allStoreNumbers[3] };
      var numbersToPersist = allStoreNumbers.Except(recentlyPersistedNumbers).ToArray();
      var numbersThatNeedToBeUpdated = new[] { numbersToPersist[0], numbersToPersist[1], numbersToPersist[2] };
      var numbersThatNeedToBeCreated = numbersToPersist.Except(numbersThatNeedToBeUpdated).ToArray();
      var stores = allStoreNumbers.Select(StoreInfoFactory.Create).ToArray();
      
      SetupWasPersistedRecently(recentlyPersistedNumbers, true);
      _dataService.ContainsStoreAsync(Arg.Is<IEnumerable<StoreNumber>>(_ => _.SequenceEqual(numbersToPersist)))
        .Returns(numbersThatNeedToBeUpdated);

      
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

    private void SetupWasPersistedRecently(IEnumerable<StoreNumber> storeNumbers, bool wasPersistedRecently = false)
    {
      foreach (var storeNumber in storeNumbers)
      {
        _persistenceCalculator.WasPersistedRecently(storeNumber).Returns(true);
      }   
    }
  }
}