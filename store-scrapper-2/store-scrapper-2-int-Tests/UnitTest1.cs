using System;
using FluentAssertions;
using Xunit;

namespace store_scrapper_2_int_Tests
{
  public class UnitTest1
  {
    [Fact]
    public void Test1()
    {
      2.Should().BeGreaterThan(1);
    }
  }
}