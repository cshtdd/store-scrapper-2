using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using store_scrapper_2;
using store_scrapper_2.Configuration;
using store_scrapper_2.Model;
using store_scrapper_2.Services;
using store_scrapper_2_Tests.Factory;
using Xunit;

namespace store_scrapper_2_Tests.Services
{
  public class AllZipCodesProcessorTest
  {
    private readonly IDelaySimulator _delaySimulator = Substitute.For<IDelaySimulator>();
    private readonly IZipCodeBatchesReader _zipCodeBatchesReader = Substitute.For<IZipCodeBatchesReader>();
    private readonly IMultipleZipCodeProcessor _multipleZipCodeProcessor = Substitute.For<IMultipleZipCodeProcessor>();
    private readonly IConfigurationReader _configurationReader = Substitute.For<IConfigurationReader>();
    private readonly IZipCodeDataService _zipCodeDataService = Substitute.For<IZipCodeDataService>();
    private readonly ISingleZipCodeProcessor _singleZipCodeProcessor = Substitute.For<ISingleZipCodeProcessor>();
    private readonly AllZipCodesProcessor _allZipCodesProcessor;

    private readonly List<string> _processedZipCodes = new List<string>();

    public AllZipCodesProcessorTest()
    {
      _multipleZipCodeProcessor.WhenForAnyArgs(_ => _.ProcessAsync(Arg.Any<IEnumerable<ZipCode>>()))
        .Do(_ => _processedZipCodes.AddRange(((IEnumerable<ZipCode>)_[0]).Select(z => z.Zip)));

      _singleZipCodeProcessor.WhenForAnyArgs(_ => _.ProcessAsync(Arg.Any<ZipCode>()))
        .Do(_ => _processedZipCodes.Add(((ZipCode)_[0]).Zip));
      
      _allZipCodesProcessor = new AllZipCodesProcessor(
        _zipCodeBatchesReader,
        _multipleZipCodeProcessor,
        _zipCodeDataService,
        _singleZipCodeProcessor,
        _delaySimulator,
        _configurationReader
      );
    }
    
    [Obsolete]
    [Fact]
    public async Task ProcessesAllTheZipCodes()
    {
      _configurationReader.ReadBool(ConfigurationKeys.ZipCodesRunContinuosly).Returns(false);
      _configurationReader.ReadUInt(ConfigurationKeys.ZipCodesMaxBatchCount).Returns(0u);
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

      await _delaySimulator.Received(3).Delay();
    }
    
    [Obsolete]
    [Fact]
    public async Task ProcessesAllTheZipCodesWhenConfigurationKeysAreNotSet()
    {
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

      await _delaySimulator.Received(3).Delay();
    }

    [Obsolete]
    [Fact]
    public async Task ProcessesTheMaximumNumberOfZipCodes()
    {
      _configurationReader.ReadBool(ConfigurationKeys.ZipCodesRunContinuosly).Returns(false);
      _configurationReader.ReadUInt(ConfigurationKeys.ZipCodesMaxBatchCount).Returns(2u);
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

      await _delaySimulator.Received(2).Delay();
    }

    [Fact]
    public async Task ProcessesAllTheZipCodes2()
    {
      await _allZipCodesProcessor.ProcessAsync2();

      await _zipCodeDataService.Received(1).AllAsync();
    }

    [Fact]
    public async Task ProcessesEachZipCodeOldestFirst()
    {
      _zipCodeDataService.AllAsync().ReturnsForAnyArgs(new[]
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
      
      await _allZipCodesProcessor.ProcessAsync2();
 
      _processedZipCodes
        .ToArray()
        .ShouldBeEquivalentTo(new []
        {
          "00000", "11111", "22222", "77777",
          "33333", "44444", "55555", "66666",
          "88888", "99999"
        });
    }
  }
}