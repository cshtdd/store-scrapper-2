﻿using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using store_scrapper_2.DataTransmission;
using store_scrapper_2.DAL;
using store_scrapper_2.Model;

namespace store_scrapper_2_int_Tests.Utils
{
  public static class StoreDataContextExtensions
  {
    public static async Task ShouldContainStoreEquivalentTo(this StoreDataContext context, StoreInfoResponse response)
    {
      var dbStore = await context.Stores.FirstAsync(_ => response.StoreNumber == new StoreNumber(_.StoreNumber, _.SatelliteNumber));
      dbStore.ShouldBeEquivalentTo(response);
    }
  }
}