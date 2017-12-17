using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using log4net;

namespace store_scrapper_2.Services
{
  public class AllZipCodesProcessor : IAllZipCodesProcessor
  {
    private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    
    private readonly IZipCodeBatchesReader _zipCodeBatchesReader;
    private readonly IMultipleZipCodeProcessor _multipleZipCodeProcessor;
    private readonly IBatchDelaySimulator _delaySimulator;

    public AllZipCodesProcessor(
      IZipCodeBatchesReader zipCodeBatchesReader,
      IMultipleZipCodeProcessor multipleZipCodeProcessor,
      IBatchDelaySimulator delaySimulator)
    {
      _zipCodeBatchesReader = zipCodeBatchesReader;
      _multipleZipCodeProcessor = multipleZipCodeProcessor;
      _delaySimulator = delaySimulator;
    }
    
    public async Task ProcessAsync()
    {
      var batches = (await _zipCodeBatchesReader.ReadAllAsync()).ToArray();

      Logger.Info($"ProcessAsync Started; batchesCount={batches.Length};");

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
  }
}