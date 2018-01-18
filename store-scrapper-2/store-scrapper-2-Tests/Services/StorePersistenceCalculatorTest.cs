using FluentAssertions;
using NSubstitute;
using store_scrapper_2.Configuration;
using store_scrapper_2.Services;
using Xunit;

namespace store_scrapper_2_Tests.Services
{
  public class StorePersistenceCalculatorTest
  {
    private readonly IConfigurationReader _configurationReader;
    private readonly ICacheWithExpiration _cacheWithExpiration;
    private readonly IStorePersistenceCalculator _calculator;
    
    public StorePersistenceCalculatorTest()
    {
      _configurationReader = Substitute.For<IConfigurationReader>();
      _configurationReader.ReadUInt(ConfigurationKeys.StoresWriteCacheExpirationMs)
        .Returns(3000u);
      
      _cacheWithExpiration = Substitute.For<ICacheWithExpiration>();

      _calculator = new StorePersistenceCalculator(_cacheWithExpiration, _configurationReader);
    }
    
    [Fact]
    public void DeterminesWhenAStoreWasRecentlyPersisted()
    {
      _cacheWithExpiration.Contains("11111-0").Returns(true);
      
      _calculator.WasPersistedRecently("11111-0").Should().BeTrue();
      _calculator.WasPersistedRecently("22222-0").Should().BeFalse();
    }
    
    [Fact]
    public void PreventsAStoreFromFuturePersistence()
    {
      _calculator.PreventFuturePersistence("11111-0");
      
      _cacheWithExpiration.Received(1).Add("11111-0", 3000u);
    }
  }
}