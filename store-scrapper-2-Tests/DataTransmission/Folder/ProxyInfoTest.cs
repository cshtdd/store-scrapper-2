using FluentAssertions;
using store_scrapper_2.DataTransmission.Proxy;
using Xunit;

namespace store_scrapper_2_Tests.DataTransmission.Folder
{
  public class ProxyInfoTest
  {
    [Fact]
    public void CanBeCreated()
    {
      new ProxyInfo("127.0.0.1", 80)
        .ToString()
        .Should()
        .Be("127.0.0.1:80");
    }
  }
}