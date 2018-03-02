
using CryptoTracker.Data.Models.Tracker;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoTracker.Data.Services.Tracker
{
    public interface ITrackerLoaderService
    {

        /// <summary>
        /// Load cryptocurrencies to be tracked
        /// </summary>


        Task<List<SerializedCryptoModel>> LoadCrypto();
        Task AddCrypto(CryptoDataModel model);
        Task RemoveCrypto(CryptoDataModel model);
        Task SaveChanges();
        Task<bool> IsTrackable(CryptoDataModel model);

       
    }
}
