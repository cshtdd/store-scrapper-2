﻿using System.Reflection;
using System.Threading;
using log4net;
using store_scrapper_2.Configuration;
using store_scrapper_2.Logging;

namespace store_scrapper_2.Services
{
  public class DelaySimulator : IDelaySimulator
  {
    private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    
    private readonly IConfigurationReader _configurationReader;

    public DelaySimulator(IConfigurationReader configurationReader) => _configurationReader = configurationReader;

    public void Delay()
    {
      var delayMs = _configurationReader.ReadUInt(ConfigurationKeys.ZipCodesDelayMs);
      Logger.LogDebug("Sleeping...", nameof(delayMs), delayMs);
      Thread.Sleep((int)delayMs);
    }
  }
}