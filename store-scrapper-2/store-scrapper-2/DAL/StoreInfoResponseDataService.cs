using System;
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
    private readonly IStoreDataContextFactory _contextFactory;
    private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public StoreInfoResponseDataService(IStoreDataContextFactory contextFactory)
    {
      _contextFactory = contextFactory;
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
      var storeAlreadyExists = await ContainsStoreAsync(storeInfo.StoreNumber);
      if (storeAlreadyExists)
      {
        throw new InvalidOperationException("Store already exists");
      }
      
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

      Logger.Debug($"SaveContextChanges; changedEntries={changedEntries}");

      if (changedEntries == 0)
      {
        throw new InvalidOperationException();
      }
    }
  }
}