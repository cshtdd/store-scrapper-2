using FluentAssertions;
using NSubstitute;
using store_scrapper_2.DataTransmission.Web;
using store_scrapper_2.DataTransmission.Web.Proxy;
using Xunit;

namespace store_scrapper_2_Tests.DataTransmission.Web.Proxy
{
  public class ProxyListRetrieverTest
  {
    private readonly IUrlDownloader urlDownloader = Substitute.For<IUrlDownloader>();
    private readonly ProxyListRetriever reader;

    public ProxyListRetrieverTest()
    {
      reader = new ProxyListRetriever(urlDownloader);
    }
    
    [Fact]
    public void DownloadsTheProxyList()
    {
      reader.Read();

      urlDownloader.Received().Download("https://raw.githubusercontent.com/clarketm/proxy-list/master/proxy-list.txt");
    }
  }
}