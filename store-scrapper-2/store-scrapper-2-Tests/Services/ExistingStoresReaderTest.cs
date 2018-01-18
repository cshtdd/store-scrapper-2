using System;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using store_scrapper_2;
using store_scrapper_2.Model;
using store_scrapper_2.Services;
using Xunit;

namespace store_scrapper_2_Tests.Services
{
  public class ExistingStoresReaderTest
  {
    private readonly IStoreInfoResponseDataService _dataService;
    private readonly IExistingStoresReader _reader;

    public ExistingStoresReaderTest()
    {
      _dataService = Substitute.For<IStoreInfoResponseDataService>();
      
      _reader = new ExistingStoresReader(_dataService);
    }
    
    [Fact]
    public async Task ReadsAllTheStores()
    {
      await _reader.InitializeAsync();

      await _dataService.Received(1).AllStoreNumbersAsync();
    }

    [Fact]
    public async Task CannotBeInitializedMultipleTimes()
    {
      await _reader.InitializeAsync();
      ((Func<Task>) (async () =>
      {
        await _reader.InitializeAsync();
      })).ShouldThrow<InvalidOperationException>(); 
    }

    [Fact]
    public void ContainsStoreFailsWhenNotInitialized()
    {
      ((Action)(() =>
      {
        _reader.ContainsStore("11111-1");
      })).ShouldThrow<InvalidOperationException>();
    }

    [Fact]
    public async Task FindsTheExistingStores()
    {
      _dataService.AllStoreNumbersAsync().Returns(new StoreNumber[]
      {
        "11111-1",
        "22222-1",
        "33333-1"
      });
      await _reader.InitializeAsync();

      _reader.ContainsStore("99999-1").Should().BeFalse();
      _reader.ContainsStore("11111-1").Should().BeTrue();
      _reader.ContainsStore("22222-1").Should().BeTrue();
    }
  }
}