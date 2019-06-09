using System;
using System.Collections.Generic;
using System.Linq;

namespace store_scrapper_2.DataTransmission.Web.Proxy
{
  public class ProxyListReader : IProxyListReader
  {
    private const string ProxyListUrl = "https://raw.githubusercontent.com/clarketm/proxy-list/master/proxy-list.txt";
    private const string ProxyListStatusUrl = "https://raw.githubusercontent.com/clarketm/proxy-list/32211a1624f0038c80886617e8536b140f220865/proxy-list-status.txt";

    private readonly IUrlDownloader _urlDownloader;

    public ProxyListReader(IUrlDownloader urlDownloader)
    {
      _urlDownloader = urlDownloader;
    }

    public IEnumerable<ProxyInfo> Read()
    {
      var successfulIps = _urlDownloader
        .Download(ProxyListStatusUrl)
        .Split(Environment.NewLine)
        .SkipLast(6)
        .Where(IsSuccess)
        .Select(ExtractIp)
        .ToArray();

      return _urlDownloader
        .Download(ProxyListUrl)
        .Split(Environment.NewLine)
        .Skip(4)
        .SkipLast(2)
        .Where(SameIncomingOutgoingIp)
        .Select(ExtractAddress)
        .Select(ProxyInfo.Parse)
        .Where(p => successfulIps.Contains(p.IpAddress))
        .ToArray();
    }

    private static bool IsSuccess(string proxyStatus) => proxyStatus.Contains("success");
    private static bool SameIncomingOutgoingIp(string proxyDefinition) => !proxyDefinition.Contains("!");
    private static string ExtractAddress(string proxyDefinition) => proxyDefinition.Split(" ")[0];
    private static string ExtractIp(string proxyStatus) => proxyStatus.Split(":")[0];
  }
}