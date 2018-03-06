using System.Collections.Generic;
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
    private readonly IConfigurationReader _configurationReader = Substitute.For<IConfigurationReader>();
    private readonly IZipCodeDataService _zipCodeDataService = Substitute.For<IZipCodeDataService>();
    private readonly ISingleZipCodeProcessor _singleZipCodeProcessor = Substitute.For<ISingleZipCodeProcessor>();
    private readonly AllZipCodesProcessor _allZipCodesProcessor;

    private readonly List<string> _processedZipCodes = new List<string>();

    public AllZipCodesProcessorTest()
    {
      _singleZipCodeProcessor.WhenForAnyArgs(_ => _.ProcessAsync(Arg.Any<ZipCode>()))
        .Do(_ => _processedZipCodes.Add(((ZipCode)_[0]).Zip));
      
      _allZipCodesProcessor = new AllZipCodesProcessor(
        _zipCodeDataService,
        _singleZipCodeProcessor,
        _delaySimulator,
        _configurationReader
      );
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
      
      await _allZipCodesProcessor.ProcessAsync();
 
      _processedZipCodes
        .ToArray()
        .Should().BeEquivalentTo(new []
        {
          "00000", "11111", "22222", "77777",
          "33333", "44444", "55555", "66666",
          "88888", "99999"
        });
      await _delaySimulator.Received(10).Delay();
    }
  }
}