using System;
using System.Collections.Generic;
using System.Linq;

namespace store_scrapper_2.DataTransmission.Web.Proxy
{
  public class ProxyListReader : IProxyListReader
  {
    private const string ProxyListUrl = "https://raw.githubusercontent.com/clarketm/proxy-list/master/proxy-list.txt";

    private readonly IUrlDownloader _urlDownloader;

    public ProxyListReader(IUrlDownloader urlDownloader)
    {
      _urlDownloader = urlDownloader;
    }

    public IEnumerable<ProxyInfo> Read()
    {
      return _urlDownloader
        .Download(ProxyListUrl)
        .Split(Environment.NewLine)
        .ToList()
        .Skip(4)
        .SkipLast(2)
        .Where(SameIncomingOutgoingIp)
        .Select(ExtractAddress)
        .Select(ProxyInfo.Parse)
        .ToArray();
    }

    private static bool SameIncomingOutgoingIp(string proxyDefinition) => !proxyDefinition.Contains("!");

    private static string ExtractAddress(string proxyDefinition) => proxyDefinition.Split(" ")[0];
  }
}