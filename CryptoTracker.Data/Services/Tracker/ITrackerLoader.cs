using CryptoTracker.Data.Models.Tracker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTracker.Data.Services.Tracker
{
    public interface ITrackerLoader
    {
        Task<List<SerializedCryptoModel>> LoadCrypto();
        void AddCrypto(CryptoDataModel model);
        Task RemoveCrypto(CryptoDataModel model);
        Task<bool> SaveChanges();

        void RaiseCryptoChanged();
        event Action<object, EventArgs> SaveCryptoChanged;

    }
}
