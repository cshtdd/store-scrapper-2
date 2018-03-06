using System;
using FluentAssertions;
using Xunit;
using store_scrapper_2.Model;

namespace store_scrapper_2_Tests.Model
{
  public class ZipCodeTest
  {
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
        .Be("Zip:33123, Latitude:\"22.00000000\", Longitude:\"25.00000000\"");
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