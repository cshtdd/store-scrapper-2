using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using store_scrapper_2.DataTransmission;
using store_scrapper_2.DAL;

namespace store
{
  public class StoreInfoResponseDataService : IStoreInfoResponseDataService
  {
    public async Task<bool> ContainsStoreAsync(string storeNumber, string satellite)
    {
      using (var db = new StoreDataContext())
      {
        return await db.Stores
          .Where(_ => _.StoreNumber == storeNumber && _.SatelliteNumber == satellite)
          .AnyAsync();
      }      
    }

    public async Task CreateNewAsync(StoreInfoResponse response)
    {
      var storeInfo = Mapper.Map<Store>(response);
      
      using (var db = new StoreDataContext())
      {
        db.Stores.Add(storeInfo);
        
        var changedEntries = await db.SaveChangesAsync();

        Console.WriteLine($"changedEntries={changedEntries}");

        if (changedEntries == 0)
        {
          throw new InvalidOperationException();
        }
      }
    }

    public async Task UpdateAsync(StoreInfoResponse response)
    {
      using (var db = new StoreDataContext())
      {
        var storeInfo = await db.Stores
          .Where(_ => _.StoreNumber == response.StoreNumber && _.SatelliteNumber == response.SatelliteNumber)
          .FirstAsync();

        var updatedStoreInfo = Mapper.Map<StoreInfoResponse, Store>(response, storeInfo);

        db.Entry(updatedStoreInfo).State = EntityState.Modified;
        
        var changedEntries = await db.SaveChangesAsync();

        Console.WriteLine($"changedEntries={changedEntries}");

        if (changedEntries == 0)
        {
          throw new InvalidOperationException();
        }
      }
    }
  }
}