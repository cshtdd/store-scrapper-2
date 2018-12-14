using System.Linq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Storage;
using store_scrapper_2.Configuration;
using store_scrapper_2.DataTransmission.Web;
using store_scrapper_2.DataTransmission.Web.Proxy;
using store_scrapper_2.DataTransmission.Web.Support;
using store_scrapper_2_int_Tests.Utils;
using Xunit;

namespace store_scrapper_2_int_Tests.DataTransmission.Web.Proxy
{
  public class ProxyListReaderTest
  {
    [Fact]
    public void DownloadsSomeProxies()
    {
      var urlDownloader = new UrlDownloader(
        new WebRequestExecutor(),
        new WebRequestFactory(DatabaseTest.ConfigurationReader)
      );
      new ProxyListReader(urlDownloader)
        .Read()
        .ToArray()
        .Length
        .Should()
        .BeGreaterThan(5);
    }
  }
}