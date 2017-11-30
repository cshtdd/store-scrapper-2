using System.IO;
using System.Reflection;
using System.Xml;
using log4net;
using log4net.Config;

namespace store_scrapper_2.Configuration
{
  public static class Logging
  {
    public static void Initialize()
    {
      var log4NetConfig = new XmlDocument();
      log4NetConfig.Load(File.OpenRead("log4net.config"));

      var repo = LogManager.CreateRepository(
        Assembly.GetEntryAssembly(),
        typeof(log4net.Repository.Hierarchy.Hierarchy));

      XmlConfigurator.Configure(repo, log4NetConfig["log4net"]);
    }
  }
}