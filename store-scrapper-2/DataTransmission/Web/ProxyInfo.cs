using System;

namespace store_scrapper_2.DataTransmission.Web
{
  public struct ProxyInfo
  {
    public string IpAddress { get; }
    public int Port { get; }

    public ProxyInfo(string fullAddress) : this(fullAddress.Split(":")[0], fullAddress.Split(":")[1]) {}
    public ProxyInfo(string ipAddress, string port) : this(ipAddress, int.Parse(port)) {}
    public ProxyInfo(string ipAddress, int port)
    {
      IpAddress = ipAddress;
      Port = port;
    }

    public static ProxyInfo Parse(string fullAddress) => new ProxyInfo(fullAddress);

    public override string ToString() => $"{IpAddress}:{Port}";

    public override bool Equals(object obj)
    {
      var that = (ProxyInfo)obj;
      return String.Equals(IpAddress, that.IpAddress) &&
        Port == that.Port;
    }

    public override int GetHashCode() => ToString().GetHashCode();

    public static bool operator == (ProxyInfo p1, ProxyInfo p2) => p1.Equals(p2);
    public static bool operator !=(ProxyInfo p1, ProxyInfo p2) => !(p1 == p2);
    
    public static implicit operator ProxyInfo(string fullAddress) => new ProxyInfo(fullAddress);
  }
}