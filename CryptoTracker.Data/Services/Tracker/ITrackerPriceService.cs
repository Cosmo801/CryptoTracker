using CryptoTracker.Data.Models.Tracker;
using CryptoTracker.Data.Request;
using CryptoTracker.Data.Services.Tracker.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTracker.Data.Services.Tracker
{
    public interface ITrackerPriceService
    {
        Task LoadCrypto();
        Task<List<CryptoDataModel>> GetTrackedCrypto();
        void DownloadCryptoData();
        void StartTracker(int delay = 30000);




        void CheckConditions();
        void OnConditionMet(CryptoRequestParameters condition, CryptoDataModel model);
        event Action<object, ConditionMetEventArgs> ConditionMet;
        event Action<object, EventArgs> TaskComplete;
    }
}
