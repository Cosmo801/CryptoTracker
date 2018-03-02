namespace CryptoTracker.Data.Models
{
    public class AdvancedCryptoModel : BasicCryptoModel
    {
        /// <summary>
        /// When a user wants to see full crypto details this model is used rather than the datagrid models
        /// </summary>

        public string ImageUrl { get; set; }
        public decimal? Open { get; set; }
        public decimal? Low { get; set; }
        public decimal? High { get; set; }

    }
}
