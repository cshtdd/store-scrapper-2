using System.Collections.Generic;
using System.Linq;

namespace store_scrapper_2.Services
{
  public static class EnumerableExtensions
  {
    public static IEnumerable<IEnumerable<T>> ToBatches<T>(this IEnumerable<T> items, int batchSize)
    {
      return items.Select((item, inx) => new { item, inx })
        .GroupBy(x => x.inx / batchSize)
        .Select(g => g.Select(x => x.item));
    }
  }
}