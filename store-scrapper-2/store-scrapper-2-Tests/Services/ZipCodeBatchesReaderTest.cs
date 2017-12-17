using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using store_scrapper_2;
using store_scrapper_2.Configuration;
using store_scrapper_2.Services;
using store_scrapper_2_Tests.Factory;
using Xunit;

namespace store_scrapper_2_Tests.Services
{
  public class ZipCodeBatchesReaderTest
  {
    [Fact]
    public async Task ReadsAllTheZipCodesInBatches()
    {
      var configurationReader = Substitute.For<IConfigurationReader>();
      configurationReader.Read(ConfigurationKeys.ZipCodesBatchSize)
        .Returns("4");
      
      var dataService = Substitute.For<IZipCodeDataService>();
      dataService.AllAsync().ReturnsForAnyArgs(new[]
      {
        ZipCodeInfoFactory.Create("00000", "2011-10-10"),
        ZipCodeInfoFactory.Create("11111", "2011-10-10"), 
        ZipCodeInfoFactory.Create("22222", "2011-10-10"), 
        ZipCodeInfoFactory.Create("33333", "2012-10-10"), 
        ZipCodeInfoFactory.Create("44444", "2012-10-10"), 
        ZipCodeInfoFactory.Create("55555", "2012-10-10"), 
        ZipCodeInfoFactory.Create("66666", "2012-10-10"), 
        ZipCodeInfoFactory.Create("77777", "2011-10-10"), 
        ZipCodeInfoFactory.Create("88888", "2012-10-10"), 
        ZipCodeInfoFactory.Create("99999", "2012-10-10") 
      });

      var reader = new ZipCodeBatchesReader(dataService, configurationReader);
      var batches = (await reader.ReadAllAsync()).ToArray();

      batches.Length.Should().Be(3);
      batches
        .Select(_ => _.Count())
        .ToArray()
        .Should()
        .BeEquivalentTo(new [] { 4, 4, 2 });
      
      batches[0].Select(_ => _.Zip).ShouldBeEquivalentTo(new [] {"00000", "11111", "22222", "77777"});
      batches[1].Select(_ => _.Zip).ShouldBeEquivalentTo(new [] {"33333", "44444", "55555", "66666"});
      batches[2].Select(_ => _.Zip).ShouldBeEquivalentTo(new [] {"88888", "99999"});
    }
  }
}