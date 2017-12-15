using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using store_scrapper_2.Model;
using store_scrapper_2.Services;
using store_scrapper_2_Tests.Factory;
using Xunit;

namespace store_scrapper_2_Tests.Services
{
  public class MultipleZipCodeProcessorTest
  {
    [Fact]
    public async void ProcessesAllTheZipCodes()
    {
      var processedZipCodes = new List<string>();
      var seededInputs = new[]
      {
        ZipCodeFactory.Create("11111"),
        ZipCodeFactory.Create("22222"),
        ZipCodeFactory.Create("33333"),
        ZipCodeFactory.Create("44444"),
        ZipCodeFactory.Create("55555")
      };
      
      var singleZipCodeProcessor = Substitute.For<ISingleZipCodeProcessor>();
      singleZipCodeProcessor.WhenForAnyArgs(_ => _.ProcessAsync(Arg.Any<ZipCode>()))
        .Do(_ => processedZipCodes.Add(((ZipCode)_[0]).Zip));
      
      var processor = new MultipleZipCodeProcessor(singleZipCodeProcessor);

      await processor.ProcessAsync(seededInputs);
      
      processedZipCodes.Sort();
        
      processedZipCodes
        .ShouldBeEquivalentTo(new []
        {
          "11111",
          "22222",
          "33333",
          "44444",
          "55555"
        });
    }
  }
}