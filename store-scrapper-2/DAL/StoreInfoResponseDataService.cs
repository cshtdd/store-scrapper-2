using System;
using System.Collections.Generic;
using System.Linq;

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using store_scrapper_2.DataTransmission;
using store_scrapper_2.DAL;
using store_scrapper_2.DAL.Db;
using store_scrapper_2.Model;
using store_scrapper_2.Logging;

namespace store_scrapper_2
{
  public class StoreInfoResponseDataService : IStoreInfoResponseDataService
  {
    private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private readonly IStoreDataContextFactory _contextFactory;

    public StoreInfoResponseDataService(IStoreDataContextFactory contextFactory) => _contextFactory = contextFactory;

    public void CreateNew(IEnumerable<StoreInfo> storesEnumerableParam)
    {
      var stores = (storesEnumerableParam ?? new StoreInfo[]{}).ToArray();
      if (stores.Length == 0)
      {
        return;
      }
      
      using (var db = _contextFactory.Create())
      {
        var newDbStores = stores.Select(_ => Mapper.Map<Store>(_));
        db.Stores.AddRange(newDbStores);
        SaveContextChanges(db);
      }
    }
    
    public IEnumerable<StoreNumber> ContainsStore(IEnumerable<StoreNumber> storesNumbersEnumerableParam)
    {
      var storeNumbers = (storesNumbersEnumerableParam ?? new StoreNumber[]{}).ToArray();
      if (storeNumbers.Length == 0)
      {
        return new StoreNumber[]{};
      }

      var optimizedStoreNumbers = storeNumbers
        .Select(_ => _.Store)
        .Select(_ => _.ToString())
        .ToArray();

      using (var db = _contextFactory.Create())
      {
        var almostAccurateDbStoreNumbersList = db.Stores
          .Where(_ => optimizedStoreNumbers.Contains(_.StoreNumber))
          .Select(_ => new StoreNumber(_.StoreNumber, _.SatelliteNumber))
          .ToArray();

        var dbStoresNumbers = almostAccurateDbStoreNumbersList
          .Where(storeNumbers.Contains)
          .ToArray();
        
        return dbStoresNumbers;
      }
    }

    public void Update(IEnumerable<StoreInfo> storesEnumerableParam)
    {
      var stores = (storesEnumerableParam ?? new StoreInfo[]{}).ToArray();
      if (stores.Length == 0)
      {
        return;
      }

      var storeNumbers = stores
        .Select(_ => _.StoreNumber)
        .OrderBy(_ => _.ToString())
        .ToArray();

      var optimizedStoreNumbers = storeNumbers
        .Select(_ => _.Store)
        .Select(_ => _.ToString())
        .ToArray();

      using (var db = _contextFactory.Create())
      {
        var almostAccurateDbStoresList = db.Stores
          .Where(_ => optimizedStoreNumbers.Contains(_.StoreNumber))
          .ToArray();
               
        var updatedDbStores = almostAccurateDbStoresList
          .Where(_ => _.ExistsIn(storeNumbers))
          .Select(_ => _.UpdateFrom(stores))
          .ToArray();

        var dbStoreNumbers = updatedDbStores
          .Select(_ => _.ReadStoreNumber())
          .OrderBy(_ => _.ToString())
          .ToArray();

        if (!storeNumbers.SequenceEqual(dbStoreNumbers))
        {
          throw new InvalidOperationException(
            LogFormatter.Format(new object[]
            {
              "error", "Update records mismatch", 
              "storeNumbersCount", storeNumbers.Length, 
              "dbStoreNumbersCount", dbStoreNumbers.Length
            }));
        }

        foreach (var entity in updatedDbStores)
        {
          db.Entry(entity).State = EntityState.Modified;
        }
        
        SaveContextChanges(db);
      }
    }

    private static void SaveContextChanges(DbContext db)
    {
      var changedEntries = db.SaveChanges();

      Logger.LogDebug("SaveContextChanges", nameof(changedEntries), changedEntries);

      if (changedEntries == 0)
      {
        throw new InvalidOperationException("A db operation produced no changes");
      }
    }
  }

  internal static class StoreExtensions
  {
    internal static bool ExistsIn(this Store sender, IEnumerable<StoreNumber> list)
    {
      var senderStoreNumber = sender.ReadStoreNumber();
      return list.Contains(senderStoreNumber);
    }

    internal static Store UpdateFrom(this Store sender, IEnumerable<StoreInfo> list)
    {
      var source = sender.FindIn(list);
      return Mapper.Map(source, sender);
    }
    
    private static StoreInfo FindIn(this Store sender, IEnumerable<StoreInfo> list)
    {
      var senderStoreNumber = sender.ReadStoreNumber();
      return list.First(_ => _.StoreNumber == senderStoreNumber);
    }

    internal static StoreNumber ReadStoreNumber(this Store sender)
    {
      return new StoreNumber(sender.StoreNumber, sender.SatelliteNumber);
    }
  } 
}