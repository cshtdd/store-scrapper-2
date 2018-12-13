using System;
using FluentAssertions;
using NSubstitute;
using store_scrapper_2.DataTransmission.Web.Proxy;
using Xunit;

namespace store_scrapper_2_Tests.DataTransmission.Web.Proxy
{
  public class ProxyRepositoryTest
  {
    private readonly IProxyListReader proxyListReader = Substitute.For<IProxyListReader>();
    private readonly ProxyRepository repository;

    public ProxyRepositoryTest()
    {
      repository = new ProxyRepository(proxyListReader); 
    }

    [Fact]
    public void CannotMarkGoodRequestsIfNoProxiesHaveBeenRead()
    {
      ((Action)(() =>
      {
          repository.MarkGoodRequest("127.0.0.1:80");
      })).Should().Throw<InvalidOperationException>();
    }
    
    [Fact]
    public void CannotMarkBadRequestsIfNoProxiesHaveBeenRead()
    {
      ((Action)(() =>
      {
          repository.MarkBadRequest("127.0.0.1:80");
      })).Should().Throw<InvalidOperationException>();
    }
  }
}