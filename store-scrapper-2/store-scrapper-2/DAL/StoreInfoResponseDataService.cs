﻿using System;
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
      using (var db = CreateContext())
      {
        return await db.Stores
          .Where(_ => _.StoreNumber == storeNumber && _.SatelliteNumber == satellite)
          .AnyAsync();
      }
    }

    public async Task CreateNewAsync(StoreInfoResponse response)
    {
      var storeInfo = Mapper.Map<Store>(response);
      
      using (var db = CreateContext())
      {
        db.Stores.Add(storeInfo);
        
        await SaveContextChanges(db);
      }
    }

    public async Task UpdateAsync(StoreInfoResponse response)
    {
      using (var db = CreateContext())
      {
        var storeInfo = await db.Stores
          .Where(_ => _.StoreNumber == response.StoreNumber && _.SatelliteNumber == response.SatelliteNumber)
          .FirstAsync();

        var updatedStoreInfo = Mapper.Map(response, storeInfo);

        db.Entry(updatedStoreInfo).State = EntityState.Modified;
        
        await SaveContextChanges(db);
      }
    }
    
    private static StoreDataContext CreateContext()
    {
      return new StoreDataContext();
    }

    private static async Task SaveContextChanges(StoreDataContext db)
    {
      var changedEntries = await db.SaveChangesAsync();

      Console.WriteLine($"changedEntries={changedEntries}");

      if (changedEntries == 0)
      {
        throw new InvalidOperationException();
      }
    }
  }
}