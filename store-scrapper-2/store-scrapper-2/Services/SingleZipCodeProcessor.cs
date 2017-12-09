﻿using System.Threading.Tasks;
using store_scrapper_2.DataTransmission;
using store_scrapper_2.Model;

namespace store_scrapper_2.Services
{
  public class SingleZipCodeProcessor
  {
    private readonly IStoreInfoDownloader _downloader;
    private readonly ISingleStorePersistor _singleStorePersistor;

    private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public SingleZipCodeProcessor(IStoreInfoDownloader downloader, ISingleStorePersistor singleStorePersistor)
    {
      _downloader = downloader;
      _singleStorePersistor = singleStorePersistor;
    }

    public async Task ProcessAsync(ZipCode zipCode)
    {     
      Logger.Info($"Processing; zipCode={zipCode};");

      Logger.Info("Downloading Stores;");
      var stores = await _downloader.DownloadAsync(zipCode);

      foreach (var store in stores)
      {
        await _singleStorePersistor.PersistAsync(store);
      }

      Logger.Info($"Processing; zipCode={zipCode}; Result=true;");
    }
  }
}