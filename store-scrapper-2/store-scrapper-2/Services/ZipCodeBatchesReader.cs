using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using store_scrapper_2.Configuration;
using store_scrapper_2.Model;

namespace store_scrapper_2.Services
{
  public class ZipCodeBatchesReader : IZipCodeBatchesReader
  {
    private readonly IZipCodeDataService _zipCodeDataService;
    private readonly IConfigurationReader _configurationReader;

    public ZipCodeBatchesReader(IZipCodeDataService zipCodeDataService, IConfigurationReader configurationReader)
    {
      _zipCodeDataService = zipCodeDataService;
      _configurationReader = configurationReader;
    }

    public async Task<IEnumerable<IEnumerable<ZipCode>>> ReadAllAsync()
    {
      var batchSize = int.Parse(_configurationReader.Read(ConfigurationKeys.ZipCodesBatchSize));
      return (await _zipCodeDataService.AllAsync())
        .OrderBy(_ => _.UpdateTimeUtc)
        .Select(_ => _.ZipCode)
        .ToBatches(batchSize);
    }
  }
}