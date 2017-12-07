namespace store_scrapper_2.Model
{
  public struct StoreNumber
  {
    public int Store { get; }
    public int Satellite { get; }
    
    public StoreNumber(int store, int satellite)
    {
      Store = store;
      Satellite = satellite;
    }

    public StoreNumber(string store, string satellite) : this(int.Parse(store), int.Parse(satellite)) { }

    public StoreNumber(string fullNumber) : this(fullNumber.Split('-')[0], fullNumber.Split('-')[1]) { }

    public override string ToString() => $"{Store}-{Satellite}";
  }
}