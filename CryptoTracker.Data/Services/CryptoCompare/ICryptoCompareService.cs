using CryptoTracker.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoTracker.Data.Services.CryptoCompare
{
    public interface ICryptoCompareService
    {

        /// <summary>
        /// Download data from CryptoCompareAPI for tracker service
        /// </summary>
       
        Task<List<string>> GetAvailableCrypto();
        Task<AdvancedCryptoModel> GetCrypto(string crypto);
        Task<BasicCryptoModel> GetBasicCrypto(string crypto);

    } 
}
