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

    public static bool operator == (ProxyInfo p1, ProxyInfo p2) => p1.Equals(p2);
    public static bool operator !=(ProxyInfo p1, ProxyInfo p2) => !(p1 == p2);
  }
}