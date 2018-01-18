using System.Threading.Tasks;
using NSubstitute;
using store_scrapper_2;
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
  }
}