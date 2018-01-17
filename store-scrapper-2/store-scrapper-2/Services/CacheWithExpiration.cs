using System;
using Microsoft.Extensions.Caching.Memory;

namespace store_scrapper_2.Services
{
  public class CacheWithExpiration : ICacheWithExpiration
  {
    private readonly IMemoryCache _memoryCache;

    public CacheWithExpiration(IMemoryCache memoryCache) => _memoryCache = memoryCache;

    public bool Contains(string key) => _memoryCache.TryGetValue(key, out var _);

    public void Add(string key, uint expirationMs)
    { 
      var cacheEntryOptions = new MemoryCacheEntryOptions()
        .SetSlidingExpiration(TimeSpan.FromMilliseconds(expirationMs));
      _memoryCache.Set(key, 1, cacheEntryOptions);
    }
  }
}