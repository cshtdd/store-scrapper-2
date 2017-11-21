using System;
using System.Threading.Tasks;
using AutoMapper;
using store_scrapper_2.DataTransmission;
using store_scrapper_2.DAL;

namespace store
{
  public class StoreInfoResponseDataService : IStoreInfoResponseDataService
  {
    public async Task SaveAsync(StoreInfoResponse response)
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
  }
}