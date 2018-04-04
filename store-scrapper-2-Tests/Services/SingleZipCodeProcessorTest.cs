using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using store_scrapper_2;
using store_scrapper_2.Model;
using store_scrapper_2.DataTransmission;
using store_scrapper_2.Services;
using store_scrapper_2_Tests.Factory;
using Xunit;

namespace store_scrapper_2_Tests.Services
{
  public class SingleZipCodeProcessorTest
  {
    private readonly ZipCode _zipCode = new ZipCode("55555", 17, 45);

    private readonly IStoreInfoDownloader _downloader = Substitute.For<IStoreInfoDownloader>();
    private readonly IStoresPersistor _persistor = Substitute.For<IStoresPersistor>();
    private readonly IZipCodeDataService _zipCodeDataService = Substitute.For<IZipCodeDataService>();

    private readonly SingleZipCodeProcessor _processor;

    public SingleZipCodeProcessorTest()
    {
      _processor = new SingleZipCodeProcessor(_downloader, _persistor, _zipCodeDataService, new IgnorePaymentRequiredExceptions());
    }
    
    [Fact]
    public async Task DownloadsAndPersistsTheStoreData()
    {
      var stores = StoreNumberFactory
        .Create(10)
        .Select(StoreInfoFactory.Create)
        .ToArray();

      _downloader.DownloadAsync(Arg.Any<ZipCode>())
        .Returns(Task.FromResult<IEnumerable<StoreInfo>>(stores));
      

      await _processor.ProcessAsync(_zipCode);

      
      await _downloader
        .Received(1)
        .DownloadAsync(_zipCode);

      await _persistor
        .Received(1)
        .PersistAsync(Arg.Is<IEnumerable<StoreInfo>>(_ => _.SequenceEqual(stores)));

      await _zipCodeDataService
        .Received(1)
        .UpdateZipCodeAsync("55555");
    }

    [Fact]
    public async Task GracefullyHandlesPaymentRequiredWebExceptions()
    {
      _downloader.DownloadAsync(Arg.Any<ZipCode>())
        .Throws(new WebException("The remote server returned an error: (402) Payment Required."));

      
      await _processor.ProcessAsync(_zipCode);

      
     await _downloader
        .Received(1)
        .DownloadAsync(_zipCode);

      await _persistor
        .DidNotReceiveWithAnyArgs()
        .PersistAsync(Arg.Any<IEnumerable<StoreInfo>>());

      _zipCodeDataService
        .DidNotReceiveWithAnyArgs()
        .UpdateZipCodeAsync(Arg.Any<string>());
    }
    
    [Fact]
    public async Task BubblesUpPaymentRequiredWebExceptionsIfNeeded()
    {
      _downloader.DownloadAsync(Arg.Any<ZipCode>())
        .Throws(new WebException("The remote server returned an error: (402) Payment Required."));
     
      
      ((Func<Task>) (async () =>
      {
        await new SingleZipCodeProcessor(_downloader, _persistor, _zipCodeDataService, new NeverIgnoreExceptions())
          .ProcessAsync(_zipCode);
      })).Should().Throw<WebException>();

      
      await _downloader
        .Received(1)
        .DownloadAsync(_zipCode);

      await _persistor
        .DidNotReceiveWithAnyArgs()
        .PersistAsync(Arg.Any<IEnumerable<StoreInfo>>());

      _zipCodeDataService
        .DidNotReceiveWithAnyArgs()
        .UpdateZipCodeAsync(Arg.Any<string>());
    }

    [Fact]
    public async Task BubblesUpAnyOtherException()
    {
      _downloader.DownloadAsync(Arg.Any<ZipCode>())
        .Throws(new WebException("Remote host not found"));
      
      
      ((Func<Task>) (async () =>
      {
        await _processor.ProcessAsync(_zipCode);
      })).Should().Throw<WebException>();
      
      
      await _downloader
        .Received(1)
        .DownloadAsync(_zipCode);

      await _persistor
        .DidNotReceiveWithAnyArgs()
        .PersistAsync(Arg.Any<IEnumerable<StoreInfo>>());

      _zipCodeDataService
        .DidNotReceiveWithAnyArgs()
        .UpdateZipCodeAsync(Arg.Any<string>());
    }
  }
}