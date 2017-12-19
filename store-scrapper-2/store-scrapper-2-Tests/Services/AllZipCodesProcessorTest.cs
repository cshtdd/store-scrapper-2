using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using store_scrapper_2.Configuration;
using store_scrapper_2.Model;
using store_scrapper_2.Services;
using store_scrapper_2_Tests.Factory;
using Xunit;

namespace store_scrapper_2_Tests.Services
{
  public class AllZipCodesProcessorTest
  {
    private readonly IBatchDelaySimulator _batchDelaySimulator = Substitute.For<IBatchDelaySimulator>();
    private readonly IZipCodeBatchesReader _zipCodeBatchesReader = Substitute.For<IZipCodeBatchesReader>();
    private readonly IMultipleZipCodeProcessor _multipleZipCodeProcessor = Substitute.For<IMultipleZipCodeProcessor>();
    private readonly IConfigurationReader _configurationReader = Substitute.For<IConfigurationReader>();
    private readonly AllZipCodesProcessor _allZipCodesProcessor;

    private readonly List<string> _processedZipCodes = new List<string>();

    public AllZipCodesProcessorTest()
    {
      _multipleZipCodeProcessor.WhenForAnyArgs(_ => _.ProcessAsync(Arg.Any<IEnumerable<ZipCode>>()))
        .Do(_ => _processedZipCodes.AddRange(((IEnumerable<ZipCode>)_[0]).Select(z => z.Zip)));

      _allZipCodesProcessor = new AllZipCodesProcessor(
        _zipCodeBatchesReader,
        _multipleZipCodeProcessor,
        _batchDelaySimulator,
        _configurationReader
      );
    }
    
    [Fact]
    public async Task ProcessesAllTheZipCodes()
    {
      _configurationReader.ReadBool(ConfigurationKeys.ZipCodesRunContinuosly).Returns(false);
      _configurationReader.ReadInt(ConfigurationKeys.ZipCodesMaxBatchCount).Returns(0);
      _zipCodeBatchesReader.ReadAllAsync().ReturnsForAnyArgs(new IEnumerable<ZipCode>[]
      {
        new [] { ZipCodeFactory.Create("00000"), ZipCodeFactory.Create("11111") },
        new [] { ZipCodeFactory.Create("22222"), ZipCodeFactory.Create("33333") },
        new [] { ZipCodeFactory.Create("44444") }
      });

      await _allZipCodesProcessor.ProcessAsync();

      _processedZipCodes.Sort();
      _processedZipCodes
        .ToArray()
        .ShouldBeEquivalentTo(new []
        {
          "00000",
          "11111",
          "22222",
          "33333",
          "44444"
        });

      await _batchDelaySimulator.Received(3).Delay();
    }

    [Fact]
    public async Task ProcessesTheMaximumNumberOfZipCodes()
    {
      _configurationReader.ReadBool(ConfigurationKeys.ZipCodesRunContinuosly).Returns(false);
      _configurationReader.ReadInt(ConfigurationKeys.ZipCodesMaxBatchCount).Returns(2);
      _zipCodeBatchesReader.ReadAllAsync().ReturnsForAnyArgs(new IEnumerable<ZipCode>[]
      {
        new [] { ZipCodeFactory.Create("00000"), ZipCodeFactory.Create("11111") },
        new [] { ZipCodeFactory.Create("22222"), ZipCodeFactory.Create("33333") },
        new [] { ZipCodeFactory.Create("44444") }
      });
      
      await _allZipCodesProcessor.ProcessAsync();

      _processedZipCodes.Sort();
      _processedZipCodes
        .ToArray()
        .ShouldBeEquivalentTo(new []
        {
          "00000",
          "11111",
          "22222",
          "33333"
        });

      await _batchDelaySimulator.Received(2).Delay();
    }
  }
}