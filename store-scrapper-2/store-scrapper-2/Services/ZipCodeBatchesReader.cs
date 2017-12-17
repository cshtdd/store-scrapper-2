using System.Collections.Generic;
using System.Threading.Tasks;
using store_scrapper_2.Model;

namespace store_scrapper_2.Services
{
  public class ZipCodeBatchesReader : IZipCodeBatchesReader
  {
    private readonly IZipCodeDataService _zipCodeDataService;

    public ZipCodeBatchesReader(IZipCodeDataService zipCodeDataService) => _zipCodeDataService = zipCodeDataService;

    public async Task<IEnumerable<IEnumerable<ZipCode>>> ReadAllAsync(int batchSize) => (await _zipCodeDataService.AllAsync()).ToBatches(batchSize);
  }
}