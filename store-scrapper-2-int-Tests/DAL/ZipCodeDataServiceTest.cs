using System;
using System.Linq;

using FluentAssertions;
using store_scrapper_2.Model;
using store_scrapper_2;
using store_scrapper_2_int_Tests.Utils;
using Xunit;

namespace store_scrapper_2_int_Tests.DAL
{
  public class ZipCodeDataServiceTest : DatabaseTest
  {
    private readonly ZipCodeDataService _dataService;

    public ZipCodeDataServiceTest()
    {
      _dataService = new ZipCodeDataService(ContextFactory);
    }

    [Fact]
    public void ReadsAllTheZipCodes()
    {
      CreatePersistenceInitializer().Initialize();

      var zips = _dataService.All().ToList();

      zips.Count.Should().Be(13);
      
      zips.Select(_ => _.ZipCode.Zip)
        .Distinct()
        .Count()
        .Should()
        .Be(13);
      
      zips.Select(_ => _.UpdateTimeUtc)
        .Distinct()
        .Count()
        .Should()
        .Be(1);

      zips.Select(_ => _.UpdateTimeUtc)
        .All(_ => _ == DateTime.MinValue)
        .Should()
        .BeTrue();
    }

    [Fact]
    public void UpdatesTheZipCode()
    {
      CreatePersistenceInitializer().Initialize();

      _dataService.All()
        .First(_ => _.ZipCode.Zip == "601")
        .UpdateTimeUtc
        .Should()
        .Be(DateTime.MinValue);
      
      _dataService.All()
        .First(_ => _.ZipCode.Zip == "605")
        .UpdateTimeUtc
        .Should()
        .Be(DateTime.MinValue);

      _dataService.UpdateZipCode("601");

      _dataService.All()
        .First(_ => _.ZipCode.Zip == "601")
        .UpdateTimeUtc
        .Should()
        .BeCloseTo(DateTime.UtcNow, 1000);
      
      _dataService.All()
        .First(_ => _.ZipCode.Zip == "605")
        .UpdateTimeUtc
        .Should()
        .Be(DateTime.MinValue);
    }
  }
}