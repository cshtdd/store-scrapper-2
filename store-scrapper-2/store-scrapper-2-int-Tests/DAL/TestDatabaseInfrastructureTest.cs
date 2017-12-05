﻿using FluentAssertions;
using store_scrapper_2_int_Tests.Utils;
using Xunit;

namespace store_scrapper_2_int_Tests.DAL
{
  public class TestDatabaseInfrastructureTest : DatabaseTest
  {
    [Fact]
    public void TestDatabaseDoesNotExist()
    {
      DatabaseExists.Should().BeFalse();
    }
  }
}