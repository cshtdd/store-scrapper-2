using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using store_scrapper_2.Model;

namespace store_scrapper_2.Services
{
  public class MultipleZipCodeProcessor : IMultipleZipCodeProcessor
  {
    private readonly ISingleZipCodeProcessor _singleZipCodeProcessor;

    public MultipleZipCodeProcessor(ISingleZipCodeProcessor singleZipCodeProcessor) => _singleZipCodeProcessor = singleZipCodeProcessor;

    public async Task ProcessAsync(IEnumerable<ZipCode> zipCodes) => await Task.WhenAll(zipCodes.Select(_singleZipCodeProcessor.ProcessAsync));
  }
}