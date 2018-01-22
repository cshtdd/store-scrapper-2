using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using log4net;
using store_scrapper_2.Configuration;
using store_scrapper_2.Model;

namespace store_scrapper_2.Services
{
  public class AllZipCodesProcessor : IAllZipCodesProcessor
  {
    private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    
    [Obsolete]
    private readonly IZipCodeBatchesReader _zipCodeBatchesReader;
    [Obsolete]
    private readonly IMultipleZipCodeProcessor _multipleZipCodeProcessor;

    private readonly IZipCodeDataService _zipCodeDataService;
    private readonly ISingleZipCodeProcessor _singleZipCodeProcessor;
    private readonly IDelaySimulator _delaySimulator;
    private readonly IConfigurationReader _configurationReader;

    public AllZipCodesProcessor(IZipCodeBatchesReader zipCodeBatchesReader,
      IMultipleZipCodeProcessor multipleZipCodeProcessor,
      IZipCodeDataService zipCodeDataService,
      ISingleZipCodeProcessor singleZipCodeProcessor,
      IDelaySimulator delaySimulator,
      IConfigurationReader configurationReader)
    {
      _zipCodeBatchesReader = zipCodeBatchesReader;
      _multipleZipCodeProcessor = multipleZipCodeProcessor;
      _zipCodeDataService = zipCodeDataService;
      _singleZipCodeProcessor = singleZipCodeProcessor;
      _delaySimulator = delaySimulator;
      _configurationReader = configurationReader;
    }
    
    public async Task ProcessAsync()
    {
      do
      {
        await ProcessAllBatchesAsync();
      } while (_configurationReader.ReadBool(ConfigurationKeys.ZipCodesRunContinuosly));
    }

    [Obsolete]
    private async Task ProcessAllBatchesAsync()
    {
      var maxBatchesCountSetting = _configurationReader.ReadUInt(ConfigurationKeys.ZipCodesMaxBatchCount);

      var batches = await ReadBatchesArray(maxBatchesCountSetting);

      Logger.Info($"ProcessAsync Started; batchesCount={batches.Length}; maxBatchesCount={maxBatchesCountSetting};");

      var batchIndex = 0;

      foreach (var zipCodes in batches)
      {
        var zipCodesArray = zipCodes.ToArray();
        Logger.Info(
          $"Processing Batch; {nameof(batchIndex)}={batchIndex}; batchesCount={batches.Length}; batchSize={zipCodesArray.Length}");

        await _multipleZipCodeProcessor.ProcessAsync(zipCodesArray);
        await _delaySimulator.Delay();

        batchIndex++;
      }

      Logger.Info($"ProcessAsync Completed; batchesCount={batches.Length};");
    }

    [Obsolete]
    private async Task<IEnumerable<ZipCode>[]> ReadBatchesArray(uint maxBatchesCountSetting)
    {
      var batches = await ReadBatches(maxBatchesCountSetting);
      return batches.ToArray();
    }

    [Obsolete]
    private async Task<IEnumerable<IEnumerable<ZipCode>>> ReadBatches(uint maxBatchesCountSetting)
    {
      var allBatches = await _zipCodeBatchesReader.ReadAllAsync();

      if (maxBatchesCountSetting == 0)
      {
        return allBatches;
      }

      return allBatches.Take((int)maxBatchesCountSetting);
    }
  }
}