using System.Collections.Generic;
using store_scrapper_2.Model;

namespace store_scrapper_2_Tests.Factory
{
  public static class StoreNumberFactory
  {
    public static IEnumerable<StoreNumber> Create(int n = 1)
    {
      for (var i = 0; i < n; i++)
      {
        yield return new StoreNumber(i, 1);
      }
    }
  }
}