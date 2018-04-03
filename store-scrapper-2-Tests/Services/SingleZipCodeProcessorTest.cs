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
      _processor = new SingleZipCodeProcessor(_downloader, _persistor, _zipCodeDataService);
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
  }
}