using System;
using FluentAssertions;
using store_scrapper_2.DataTransmission;
using Xunit;

namespace store_scrapper_2_Tests.DataTransmission
{
  public class ZipCodeTest
  {
    [Fact]
    public void GeneratesTheUrlBasedOnTheZipCode()
    {
      new ZipCode("12345", 10.23m, -20.7686m)
        .ToUrl()
        .Should()
        .Be(
          "https://locator-svc.subway.com/v2//GetLocations.ashx?q=%7B%22InputText%22%3A%22%22%2C%22GeoCode%22%3A%7B%22Latitude%22%3A10.23%2C%22Longitude%22%3A-20.7686%2C%22Accuracy%22%3Anull%2C%22CountryCode%22%3Anull%2C%22RegionCode%22%3Anull%2C%22PostalCode%22%3A%2212345%22%2C%22City%22%3Anull%2C%22LocalityType%22%3Anull%7D%2C%22DetectedLocation%22%3A%7B%22Latitude%22%3A0%2C%22Longitude%22%3A0%2C%22Accuracy%22%3A0%7D%2C%22Paging%22%3A%7B%22StartIndex%22%3A1%2C%22PageSize%22%3A50%7D%2C%22ConsumerParameters%22%3A%7B%22metric%22%3Afalse%2C%22culture%22%3A%22en-US%22%2C%22country%22%3A%22US%22%2C%22size%22%3A%22D%22%2C%22template%22%3A%22%22%2C%22rtl%22%3Afalse%2C%22clientId%22%3A%2217%22%2C%22key%22%3A%22SUBWAY_PROD%22%7D%2C%22Filters%22%3A%5B%5D%2C%22LocationType%22%3A1%7D"
        );
    }

    [Fact]
    public void CanBeComparedToAnotherZipCode()
    {
      new ZipCode("12345", 12m, 23m)
        .Should()
        .Be(new ZipCode("12345", 12m, 23m));
      
      new ZipCode("12345", 12m, 23m)
        .Should()
        .NotBe(new ZipCode("12345", 82m, 23m));
      
      new ZipCode("12345", 12m, 23m)
        .Should()
        .NotBe(new ZipCode("12345", 12m, 83m));
      
      new ZipCode("12345", 12m, 23m)
        .Should()
        .NotBe(new ZipCode("12340", 12m, 23m));
    }

    [Fact]
    public void ToStringShowsTheZipCode()
    {
      new ZipCode("33123", 22, 25)
        .ToString()
        .Should()
        .Be("Zip=33123; Latitude=22.00000000; Longitude=25.00000000;");
    }

    [Theory]
    [InlineData("123456")]
    [InlineData("12")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("a-&%B")]
    [InlineData("1233A")]
    public void CannotBeConstructedOutOfInvalidValues(string zipCode)
    {
      ((Action) (() =>
      {
        new ZipCode(zipCode, -23.45m, 67.89m);
      })).ShouldThrow<ArgumentException>($"{zipCode} should have caused an error");
    }

    [Theory]
    [InlineData(90.01)]
    [InlineData(-90.01)]
    [InlineData(-91)]
    [InlineData(91)]
    public void CannotBeConstructedOutOfInvalidLatitude(decimal latitude)
    {
      ((Action) (() =>
      {
        new ZipCode("33123", latitude, 67.89m);
      })).ShouldThrow<ArgumentException>($"{latitude} should have caused an error");
    }
    
    [Theory]
    [InlineData(180.01)]
    [InlineData(-180.01)]
    [InlineData(-181)]
    [InlineData(181)]
    public void CannotBeConstructedOutOfInvalidLongitude(decimal longitude)
    {
      ((Action) (() =>
      {
        new ZipCode("33123", -12.34m, longitude);
      })).ShouldThrow<ArgumentException>($"{longitude} should have caused an error");
    }
  }
}