namespace store_scrapper_2.DataTransmission
{
    public struct StoreInfoRequest
    {
        public string StoreNumber { get; }

        public StoreInfoRequest(string storeNumber) => StoreNumber = storeNumber;
    }
}