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
    
    private readonly IZipCodeBatchesReader _zipCodeBatchesReader;
    private readonly IMultipleZipCodeProcessor _multipleZipCodeProcessor;
    private readonly IBatchDelaySimulator _delaySimulator;
    private readonly IConfigurationReader _configurationReader;

    public AllZipCodesProcessor(
      IZipCodeBatchesReader zipCodeBatchesReader,
      IMultipleZipCodeProcessor multipleZipCodeProcessor,
      IBatchDelaySimulator delaySimulator,
      IConfigurationReader configurationReader)
    {
      _zipCodeBatchesReader = zipCodeBatchesReader;
      _multipleZipCodeProcessor = multipleZipCodeProcessor;
      _delaySimulator = delaySimulator;
      _configurationReader = configurationReader;
    }
    
    public async Task ProcessAsync()
    {
      var maxBatchesCountSetting = _configurationReader.ReadInt(ConfigurationKeys.ZipCodesMaxBatchCount);

      var batches = await ReadBatchesArray(maxBatchesCountSetting);
        
      Logger.Info($"ProcessAsync Started; batchesCount={batches.Length}; maxBatchesCount={maxBatchesCountSetting};");

      var batchIndex = 0;
      
      foreach (var zipCodes in batches)
      {
        var zipCodesArray = zipCodes.ToArray();
        Logger.Info($"Processing Batch; {nameof(batchIndex)}={batchIndex}; batchesCount={batches.Length}; batchSize={zipCodesArray.Length}");       
        
        await _multipleZipCodeProcessor.ProcessAsync(zipCodesArray);
        await _delaySimulator.Delay();
        
        batchIndex++;
      }

      Logger.Info($"ProcessAsync Completed; batchesCount={batches.Length};");
    }

    private async Task<IEnumerable<ZipCode>[]> ReadBatchesArray(int maxBatchesCountSetting)
    {
      var batches = await ReadBatches(maxBatchesCountSetting);
      return batches.ToArray();
    }

    private async Task<IEnumerable<IEnumerable<ZipCode>>> ReadBatches(int maxBatchesCountSetting)
    {
      var allBatches = await _zipCodeBatchesReader.ReadAllAsync();

      if (maxBatchesCountSetting == 0)
      {
        return allBatches;
      }

      return allBatches.Take(maxBatchesCountSetting);
    }
  }
}