using Newtonsoft.Json;

namespace CryptoTracker.Data.DTOs.CoinMarketCap
{
    public class CMCAllCoinResponse
    {
        public virtual string Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string Symbol { get; set; }

        public virtual long Rank { get; set; }

        [JsonProperty(PropertyName = "price_usd")]
        public virtual decimal? USDPrice { get; set; }

        [JsonProperty(PropertyName = "price_btc")]
        public virtual decimal? BTCPrice { get; set; }

        [JsonProperty(PropertyName = "24h_volume_usd")]
        public virtual decimal? Volume24HUSD { get; set; }

        [JsonProperty(PropertyName = "market_cap_usd")]
        public virtual decimal? MarketCapUSD { get; set; }

        [JsonProperty(PropertyName = "available_supply")]
        public virtual decimal? AvailableSupply { get; set; }

        [JsonProperty(PropertyName = "total_supply")]
        public virtual decimal? TotalSupply { get; set; }

        [JsonProperty(PropertyName = "percent_change_1h")]
        public virtual decimal? Change1H { get; set; }

        [JsonProperty(PropertyName = "percent_change_24h")]
        public virtual decimal? Change24H { get; set; }

        [JsonProperty(PropertyName = "percent_change_7d")]
        public virtual decimal? Change7D { get; set; }

        [JsonProperty(PropertyName = "last_updated")]
        public long LastUpdate { get; set; }
    }
}
