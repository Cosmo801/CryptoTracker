namespace CryptoTracker.Data.Request
{
    public class CryptoRequestParameters
    {
        /// <summary>
        /// Used by filtering services to filter out cryptocurrencies
        /// </summary>

        public RequestPropertyType Property { get; set; }
        public RequestFilterType Type { get; set; }
        public decimal Value { get; set; }
    }

}
