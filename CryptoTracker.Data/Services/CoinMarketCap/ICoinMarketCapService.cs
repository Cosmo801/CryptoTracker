using CryptoTracker.Data.Models;
using CryptoTracker.Data.Request;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoTracker.Data.Services.CoinMarketCap
{
    public interface ICoinMarketCapService
    {
        /// <summary>
        /// Download data from CoinMarketCap API
        /// </summary>

        Task<List<BasicCryptoModel>> GetAllCoins();
        Task<BasicCryptoModel> GetCoin(string coinName);

        Task<List<BasicCryptoModel>> GetFilteredCoins(CryptoRequestService requestParameters);
    }
}
