namespace store_scrapper_2.DataTransmission.Proxy
{
  public struct ProxyInfo
  {
    public string IpAddress { get; }
    public int Port { get; }
    
    public ProxyInfo(string ipAddress, int port)
    {
      IpAddress = ipAddress;
      Port = port;
    }

    public override string ToString() => $"{IpAddress}:{Port}";
  }
}