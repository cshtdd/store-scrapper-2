using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using store_scrapper_2;
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
      var dataService = Substitute.For<IZipCodeDataService>();
      dataService.AllAsync().ReturnsForAnyArgs(new[]
      {
        ZipCodeFactory.Create("00000"), 
        ZipCodeFactory.Create("11111"), 
        ZipCodeFactory.Create("22222"), 
        ZipCodeFactory.Create("33333"), 
        ZipCodeFactory.Create("44444"), 
        ZipCodeFactory.Create("55555"), 
        ZipCodeFactory.Create("66666"), 
        ZipCodeFactory.Create("77777"), 
        ZipCodeFactory.Create("88888"), 
        ZipCodeFactory.Create("99999") 
      });

      var batches = (await new ZipCodeBatchesReader(dataService).ReadAllAsync(4)).ToArray();

      batches.Length.Should().Be(3);
      batches
        .Select(_ => _.Count())
        .ToArray()
        .Should()
        .BeEquivalentTo(new [] { 4, 4, 2 });
      
      batches[0].Select(_ => _.Zip).ShouldBeEquivalentTo(new [] {"00000", "11111", "22222", "33333"});
      batches[1].Select(_ => _.Zip).ShouldBeEquivalentTo(new [] {"44444", "55555", "66666", "77777"});
      batches[2].Select(_ => _.Zip).ShouldBeEquivalentTo(new [] {"88888", "99999"});
    }
  }
}