using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using store_scrapper_2.Model;
using store_scrapper_2.Services;
using store_scrapper_2_Tests.Factory;
using Xunit;

namespace store_scrapper_2_Tests.Services
{
  public class AllZipCodesProcessorTest
  {
    [Fact]
    public async Task ProcessesAllTheZipCodes()
    {
      var batchesReader = Substitute.For<IZipCodeBatchesReader>();
      batchesReader.ReadAllAsync().ReturnsForAnyArgs(new IEnumerable<ZipCode>[]
      {
        new [] { ZipCodeFactory.Create("00000"), ZipCodeFactory.Create("11111") },
        new [] { ZipCodeFactory.Create("22222"), ZipCodeFactory.Create("33333") },
        new [] { ZipCodeFactory.Create("44444") }
      });

      var processedZipCodes = new List<string>();
      var processor = Substitute.For<IMultipleZipCodeProcessor>();
      processor.WhenForAnyArgs(_ => _.ProcessAsync(Arg.Any<IEnumerable<ZipCode>>()))
        .Do(_ => processedZipCodes.AddRange(((IEnumerable<ZipCode>)_[0]).Select(z => z.Zip)));

      await new AllZipCodesProcessor(batchesReader, processor).ProcessAsync();

      processedZipCodes.Sort();
        
      processedZipCodes
        .ToArray()
        .ShouldBeEquivalentTo(new []
        {
          "00000",
          "11111",
          "22222",
          "33333",
          "44444"
        });
    }
  }
}