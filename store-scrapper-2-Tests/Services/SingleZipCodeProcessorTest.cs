using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

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
    private readonly IDeadlockDetector _deadlockDetector = Substitute.For<IDeadlockDetector>();

    private readonly SingleZipCodeProcessor _processor;

    public SingleZipCodeProcessorTest()
    {
      _processor = new SingleZipCodeProcessor(
        _downloader,
        _persistor,
        _zipCodeDataService,
        new IgnorePaymentRequiredExceptions(),
        _deadlockDetector);
    }
    
    [Fact]
    public void DownloadsAndPersistsTheStoreData()
    {
      var stores = StoreNumberFactory
        .Create(10)
        .Select(StoreInfoFactory.Create)
        .ToArray();

      _downloader.Download(Arg.Any<ZipCode>())
        .Returns(stores);
      

      _processor.Process(_zipCode);

      
      _downloader
        .Received(1)
        .Download(_zipCode);

      _persistor
        .Received(1)
        .Persist(Arg.Is<IEnumerable<StoreInfo>>(_ => _.SequenceEqual(stores)));

      _zipCodeDataService
        .Received(1)
        .UpdateZipCode("55555");
      
      _deadlockDetector
        .Received(1)
        .UpdateStatus();
    }

    [Fact]
    public void GracefullyHandlesPaymentRequiredWebExceptions()
    {
      _downloader.Download(Arg.Any<ZipCode>())
        .Throws(new WebException("The remote server returned an error: (402) Payment Required."));

      
      _processor.Process(_zipCode);

      
     _downloader
        .Received(1)
        .Download(_zipCode);

      _persistor
        .DidNotReceiveWithAnyArgs()
        .Persist(Arg.Any<IEnumerable<StoreInfo>>());

      _zipCodeDataService
        .DidNotReceiveWithAnyArgs()
        .UpdateZipCode(Arg.Any<string>());
    }
    
    [Fact]
    public void BubblesUpPaymentRequiredWebExceptionsIfNeeded()
    {
      _downloader.Download(Arg.Any<ZipCode>())
        .Throws(new WebException("The remote server returned an error: (402) Payment Required."));
     
      
      ((Action) (() =>
      {
        new SingleZipCodeProcessor(_downloader, _persistor, _zipCodeDataService, new NeverIgnoreExceptions(), _deadlockDetector)
          .Process(_zipCode);
      })).Should().Throw<WebException>();

      
      _downloader
        .Received(1)
        .Download(_zipCode);

      _persistor
        .DidNotReceiveWithAnyArgs()
        .Persist(Arg.Any<IEnumerable<StoreInfo>>());

      _zipCodeDataService
        .DidNotReceiveWithAnyArgs()
        .UpdateZipCode(Arg.Any<string>());
    }

    [Fact]
    public void BubblesUpAnyOtherException()
    {
      _downloader.Download(Arg.Any<ZipCode>())
        .Throws(new WebException("Remote host not found"));
      
      
      ((Action) (() =>
      {
        _processor.Process(_zipCode);
      })).Should().Throw<WebException>();
      
      
      _downloader
        .Received(1)
        .Download(_zipCode);

      _persistor
        .DidNotReceiveWithAnyArgs()
        .Persist(Arg.Any<IEnumerable<StoreInfo>>());

      _zipCodeDataService
        .DidNotReceiveWithAnyArgs()
        .UpdateZipCode(Arg.Any<string>());
    }
  }
}