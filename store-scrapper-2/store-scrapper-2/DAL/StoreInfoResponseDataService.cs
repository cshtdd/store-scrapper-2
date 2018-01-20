using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using store_scrapper_2.DataTransmission;
using store_scrapper_2.DAL;
using store_scrapper_2.DAL.Db;
using store_scrapper_2.Model;

namespace store_scrapper_2
{
  public class StoreInfoResponseDataService : IStoreInfoResponseDataService
  {
    private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private readonly IStoreDataContextFactory _contextFactory;

    public StoreInfoResponseDataService(IStoreDataContextFactory contextFactory) => _contextFactory = contextFactory;

    public async Task<IEnumerable<StoreNumber>> ContainsStoreAsync(IEnumerable<StoreNumber> storesNumbersEnumerableParam)
    {
      using (var db = _contextFactory.Create())
      {
        var storeNumbers = storesNumbersEnumerableParam
          .ToArray();
        
        var optimizedStoreNumbers = storeNumbers
          .Select(_ => _.Store)
          .Select(_ => _.ToString())
          .ToArray();

        var almostAccurateDbStoreNumbersList = await db.Stores
          .Where(_ => optimizedStoreNumbers.Contains(_.StoreNumber))
          .Select(_ => new StoreNumber(_.StoreNumber, _.SatelliteNumber))
          .ToArrayAsync();

        var dbStoresNumbers = almostAccurateDbStoreNumbersList
          .Where(storeNumbers.Contains)
          .ToArray();
        
        return dbStoresNumbers;
      }
    }

    public async Task CreateNewAsync(IEnumerable<StoreInfo> stores)
    {
      using (var db = _contextFactory.Create())
      {
        var newDbStores = stores.Select(_ => Mapper.Map<Store>(_));
        await db.Stores.AddRangeAsync(newDbStores);
        await SaveContextChangesAsync(db);
      }
    }

    public async Task UpdateAsync(IEnumerable<StoreInfo> storesEnumerableParam)
    {
      using (var db = _contextFactory.Create())
      {
        var stores = storesEnumerableParam.ToArray();
        var storeNumbers = stores
          .Select(_ => _.StoreNumber)
          .OrderBy(_ => _.ToString())
          .ToArray();

        var optimizedStoreNumbers = storeNumbers
          .Select(_ => _.Store)
          .Select(_ => _.ToString())
          .ToArray();

        var almostAccurateDbStoresList = await db.Stores
          .Where(_ => optimizedStoreNumbers.Contains(_.StoreNumber))
          .ToArrayAsync();
               
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
            $"Update records mismatch; storeNumbersCount={storeNumbers.Length}; dbStoreNumbersCount={dbStoreNumbers.Length}"
            );
        }

        foreach (var entity in updatedDbStores)
        {
          db.Entry(entity).State = EntityState.Modified;
        }
        
        await SaveContextChangesAsync(db);
      }
    }

    public async Task<bool> ContainsStoreAsync(StoreNumber storeNumber)
    {
      using (var db = _contextFactory.Create())
      {
        return await db.Stores
          .Where(_ => storeNumber == new StoreNumber(_.StoreNumber, _.SatelliteNumber))
          .AnyAsync();
      }
    }

    public async Task CreateNewAsync(StoreInfo storeInfo)
    {
      var store = Mapper.Map<Store>(storeInfo);
      
      using (var db = _contextFactory.Create())
      {
        db.Stores.Add(store);
        
        await SaveContextChangesAsync(db);
      }
    }

    public async Task UpdateAsync(StoreInfo storeInfo)
    {
      using (var db = _contextFactory.Create())
      {
        var existingStore = await db.Stores
          .Where(_ => storeInfo.StoreNumber == new StoreNumber(_.StoreNumber, _.SatelliteNumber))
          .FirstAsync();

        var updatedStore = Mapper.Map(storeInfo, existingStore);

        db.Entry(updatedStore).State = EntityState.Modified;
        
        await SaveContextChangesAsync(db);
      }
    }

    private static async Task SaveContextChangesAsync(DbContext db)
    {
      var changedEntries = await db.SaveChangesAsync();

      Logger.Debug($"SaveContextChangesAsync; {nameof(changedEntries)}={changedEntries}");

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