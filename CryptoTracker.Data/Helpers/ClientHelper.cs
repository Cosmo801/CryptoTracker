using System;
using System.Net.Http;

namespace CryptoTracker.Data.Helpers
{
    public class ClientHelper
    {
        public static HttpClient GetClient(string baseAddress)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };

            return client;
        }


        public const string CoinMarketCapBase = "https://api.coinmarketcap.com/v1/";
        public const string CryptoCompareBase = "https://min-api.cryptocompare.com/data/";

    }
}
