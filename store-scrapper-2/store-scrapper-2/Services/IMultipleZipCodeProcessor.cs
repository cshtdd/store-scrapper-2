﻿using System.Collections.Generic;
using System.Threading.Tasks;
using store_scrapper_2.Model;

namespace store_scrapper_2.Services
{
  public interface IMultipleZipCodeProcessor
  {
    Task ProcessAsync(IEnumerable<ZipCode> zipCodes);    
  }
}