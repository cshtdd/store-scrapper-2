using FluentAssertions;
using store_scrapper_2.Model;
using Xunit;

namespace store_scrapper_2_Tests.Model
{
  public class StoreNumberTest
  {
    [Fact]
    public void CanBeCreatedOutOfTwoNumbers()
    {
      new StoreNumber(12323, 1)
        .ToString()
        .Should()
        .Be("12323-1");
    }

    [Fact]
    public void CanBeCreatedOutOfTwoNumericStrings()
    {
      new StoreNumber("11111", "3")
        .ToString()
        .Should()
        .Be("11111-3");
    }
    
    [Fact]
    public void CanBeCreatedOutOfAFullNumber()
    {
      new StoreNumber("11111-3")
        .ToString()
        .Should()
        .Be("11111-3");
    }

    [Fact]
    public void CanBeCompared()
    {
      new StoreNumber("22222", "3")
        .Should()
        .Be(new StoreNumber(22222, 3));
      
      new StoreNumber("55555", "3")
        .Should()
        .NotBe(new StoreNumber(22222, 3));
    }
    
    [Fact]
    public void CanBeComparedUsingTheEqualsSign()
    {
      (new StoreNumber("22222", "3") == new StoreNumber(22222, 3)).Should().BeTrue();
      (new StoreNumber("22222", "3") != new StoreNumber(22222, 3)).Should().BeFalse();
      
      (new StoreNumber("55555", "3") == new StoreNumber(22222, 3)).Should().BeFalse();
      (new StoreNumber("55555", "3") != new StoreNumber(22222, 3)).Should().BeTrue();
    }
  }
}