using System.Threading;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using store_scrapper_2.Services;
using Xunit;

namespace store_scrapper_2_Tests.Services
{
  public class CacherWithExpirationTest
  {
    private readonly ICacheWithExpiration cache = new CacheWithExpiration(new MemoryCache(new MemoryCacheOptions()));
    
    [Fact]
    public void DoesNotFindExpiredKeys()
    {
      cache.Contains("key1").Should().BeFalse();
      cache.Contains("key2").Should().BeFalse();
    
      cache.Add("key1", 100);

      cache.Contains("key1").Should().BeTrue();
      cache.Contains("key2").Should().BeFalse();

      Thread.Sleep(200);

      cache.Contains("key1").Should().BeFalse();
      cache.Contains("key2").Should().BeFalse();
    }

    [Fact]
    public void TheSameKeyCanBeAddedTwice()
    {
      cache.Contains("key1").Should().BeFalse();

      cache.Add("key1", 100);

      cache.Contains("key1").Should().BeTrue();

      cache.Add("key1", 100);

      cache.Contains("key1").Should().BeTrue();
      
      Thread.Sleep(200);

      cache.Contains("key1").Should().BeFalse();
    }
  }
}