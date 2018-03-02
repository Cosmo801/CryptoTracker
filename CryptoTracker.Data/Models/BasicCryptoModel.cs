namespace CryptoTracker.Data.Models

{
    public class BasicCryptoModel
    {
        /// <summary>
        /// Datagrid databinding element
        /// </summary>

        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal? MarketCap { get; set; }
        public decimal? BTCPrice { get; set; }
        public decimal? USDPrice { get; set; }
        public decimal? Volume24h { get; set; }
        public decimal? CirculatingSupply { get; set; }
        public decimal? TotalSupply { get; set; }
        public decimal? Change24h { get; set; }



    }
}
