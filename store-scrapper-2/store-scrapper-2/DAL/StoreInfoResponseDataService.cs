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

    public async Task CreateNewAsync(StoreInfoResponse response)
    {
      var storeAlreadyExists = await ContainsStoreAsync(response.StoreNumber);
      if (storeAlreadyExists)
      {
        throw new InvalidOperationException("Store already exists");
      }
      
      var storeInfo = Mapper.Map<Store>(response);
      
      using (var db = _contextFactory.Create())
      {
        db.Stores.Add(storeInfo);
        
        await SaveContextChanges(db);
      }
    }

    public async Task UpdateAsync(StoreInfoResponse response)
    {
      using (var db = _contextFactory.Create())
      {
        var storeInfo = await db.Stores
          .Where(_ => response.StoreNumber == new StoreNumber(_.StoreNumber, _.SatelliteNumber))
          .FirstAsync();

        var updatedStoreInfo = Mapper.Map(response, storeInfo);

        db.Entry(updatedStoreInfo).State = EntityState.Modified;
        
        await SaveContextChanges(db);
      }
    }

    private static async Task SaveContextChanges(DbContext db)
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