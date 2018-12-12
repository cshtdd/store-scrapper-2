using FluentAssertions;
using store_scrapper_2.DataTransmission.Web;
using Xunit;

namespace store_scrapper_2_Tests.DataTransmission.Web
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

    [Fact]
    public void CanBeCompared()
    {
      new ProxyInfo("192.168.1.1", 8080)
        .Should()
        .Be(new ProxyInfo("192.168.1.1", 8080));
      
      new ProxyInfo("192.168.1.1", 8080)
        .Should()
        .NotBe(new ProxyInfo("192.168.1.1", 80));
    }

    [Fact]
    public void CanBeComparedUsingTheEqualsSign()
    {
      (new ProxyInfo("192.168.1" + ".1", 8080) == new ProxyInfo("192.168.1.1", 8080)).Should().BeTrue();
      (new ProxyInfo("127.0.0.1", 8080) != new ProxyInfo("192.168.1.1", 8000 + 80)).Should().BeTrue();
    }

    [Fact]
    public void CanBeImplicitlyBuiltOutOfAString()
    {
      ProxyInfo p = "127.0.0.1:8080";
      p.Should().Be(new ProxyInfo("127.0.0.1", 8080));
    }
  }
}