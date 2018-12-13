using System.Linq;
using FluentAssertions;
using store_scrapper_2.DataTransmission.Web;
using store_scrapper_2.DataTransmission.Web.Proxy;
using store_scrapper_2.DataTransmission.Web.Support;
using Xunit;

namespace store_scrapper_2_int_Tests.DataTransmission.Web.Proxy
{
  public class ProxyListReaderTest
  {
    [Fact]
    public void DownloadsSomeProxies()
    {
      new ProxyListReader(new UrlDownloader(new WebRequestExecutor(), new WebRequestFactory()))
        .Read()
        .ToArray()
        .Length
        .Should()
        .BeGreaterThan(5);
    }
  }
}