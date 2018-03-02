using Newtonsoft.Json;

namespace CryptoTracker.Data.DTOs.CoinMarketCap
{
    public class CMCSingleCoinResponse
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Symbol { get; set; }

        public long Rank { get; set; }

        [JsonProperty(PropertyName = "price_usd")]
        public decimal USDPrice { get; set; }

        [JsonProperty(PropertyName = "price_btc")]
        public decimal BTCPrice { get; set; }

        [JsonProperty(PropertyName = "24h_volume_usd")]
        public decimal Volume24HUSD { get; set; }

        [JsonProperty(PropertyName = "market_cap_usd")]
        public decimal MarketCapUSD { get; set; }

        [JsonProperty(PropertyName = "available_supply")]
        public decimal? AvailableSupply { get; set; }

        [JsonProperty(PropertyName = "total_supply")]
        public decimal? TotalSupply { get; set; }

        [JsonProperty(PropertyName = "max_supply")]
        public decimal? MaxSupply { get; set; }

        [JsonProperty(PropertyName = "percent_change_1h")]
        public decimal Change1H { get; set; }

        [JsonProperty(PropertyName = "percent_change_24h")]
        public decimal Change24H { get; set; }

        [JsonProperty(PropertyName = "percent_change_7d")]
        public decimal Change7D { get; set; }

        [JsonProperty(PropertyName = "last_updated")]
        public long LastUpdate { get; set; }
    }
}
