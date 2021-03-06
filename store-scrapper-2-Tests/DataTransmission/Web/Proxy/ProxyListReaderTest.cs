using System.Linq;
using FluentAssertions;
using NSubstitute;
using store_scrapper_2.DataTransmission.Web;
using store_scrapper_2.DataTransmission.Web.Proxy;
using Xunit;

namespace store_scrapper_2_Tests.DataTransmission.Web.Proxy
{
  public class ProxyListReaderTest
  {
    private const string ProxyListUrl = "https://raw.githubusercontent.com/clarketm/proxy-list/master/proxy-list.txt";
    private const string ProxyListStatusUrl = "https://raw.githubusercontent.com/clarketm/proxy-list/master/proxy-list-status.txt";
    
    private readonly IUrlDownloader urlDownloader = Substitute.For<IUrlDownloader>();
    private readonly ProxyListReader reader;

    public ProxyListReaderTest()
    {
      reader = new ProxyListReader(urlDownloader);
    }
    
    [Fact]
    public void DownloadsTheProxyList()
    {
      reader.Read();

      urlDownloader.Received().Download(ProxyListUrl);
    }

    [Fact]
    public void DownloadsTheProxyStatus()
    {
      reader.Read();

      urlDownloader.Received().Download(ProxyListStatusUrl);      
    }
    
    [Fact]
    public void CorrectlyParsesTheProxyList()
    {
      urlDownloader.Download(ProxyListStatusUrl)
        .Returns(@"162.17.188.105: failure
140.227.200.215: success
107.150.10.35: success
70.28.43.61: success
179.43.81.135: success
186.15.233.218: success
185.54.101.122: success
24.37.245.42: success
73.239.197.175: failure
204.245.9.15: failure
103.194.89.161: success
87.248.171.166: success

SUCCESS rate:
95.00%

FAILURE rate:
5.00%");
      
      urlDownloader.Download(ProxyListUrl)
        .Returns(@"Proxy list updated at Tue, 11 Dec 18 05:55:08 +0300
Mirrors=http://spys.me/proxy.txt https://twitter.com/spys_one https://t.me/spys_one
IP address:Port Country-Anonymity(Noa/Anm/Hia)-SSL_support(S)-Google_passed(+)

107.150.10.35:3128 US-N - 
70.28.43.61:8080 CA-H-S + 
179.43.81.135:80 PE-H + 
186.15.233.218:45999 CR-H-S + 
185.54.101.122:8080 PL-N-S + 
24.37.245.42:46795 CA-H-S + 
73.239.197.175:8080 US-N-S + 
204.245.9.15:50928 GB-H-S + 
103.194.89.161:8080 IN-H-S! + 
87.248.171.166:44576 MD-H-S! + 

Free HTTP/HTTPS(SSL) proxy list only. Text format. Updated hourly.");

      var proxies = reader.Read().ToArray();

      proxies.Length.Should().Be(6);
      proxies[0].Should().Be(new ProxyInfo("107.150.10.35:3128"));
      proxies[1].Should().Be(new ProxyInfo("70.28.43.61:8080"));
      proxies[2].Should().Be(new ProxyInfo("179.43.81.135:80"));
      proxies[3].Should().Be(new ProxyInfo("186.15.233.218:45999"));
      proxies[4].Should().Be(new ProxyInfo("185.54.101.122:8080"));
      proxies[5].Should().Be(new ProxyInfo("24.37.245.42:46795"));
    }
  }
}