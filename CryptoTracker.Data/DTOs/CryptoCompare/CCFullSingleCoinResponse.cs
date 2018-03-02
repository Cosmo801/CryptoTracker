namespace CryptoTracker.Data.DTOs.CryptoCompare
{
    public class CCFullSingleCoinResponse
    {
       
        public string Type { get; set; }
        public string Market { get; set; }
        public string FromSymbol { get; set; }
        public string ToSymbol { get; set; }
        public string Flags { get; set; }
        public decimal Price { get; set; }
        public long LastUpdate { get; set; }
        public long LastTradeId { get; set; }
        public decimal LastVolume { get; set; }
        public decimal LastVolumeTo { get; set; }
        public decimal VolumeDay { get; set; }
        public decimal VolumeDayTo { get; set; }
        public decimal Volume24Hour { get; set; }
        public decimal Volume24HourTo { get; set; }
        public decimal OpenDay { get; set; }
        public decimal HighDay { get; set; }
        public decimal LowDay{ get; set; }
        public decimal Open24Hour { get; set; }
        public decimal High24Hour { get; set; }
        public decimal Low24Hour { get; set; }
        public string LastMarket { get; set; }
        public decimal Change24Hour { get; set; }
        public decimal ChangePCT24Hour { get; set; }
        public decimal ChangeDay { get; set; }
        public decimal ChangePCTday{ get; set; }
        public decimal Supply { get; set; }
        public decimal MKTCAP { get; set; }

    }
}
