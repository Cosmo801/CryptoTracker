using CryptoTracker.Data.Models.Tracker;
using CryptoTracker.Data.Request;
using CryptoTracker.Data.Services.Tracker.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoTracker.Data.Services.Tracker
{
    public interface ITrackerService
    {
        
        Task<List<CryptoDataModel>> LoadCryptoDataModels();
        Task GetTrackedCryptoValue(int delay);

        void CheckConditions(List<CryptoDataModel> model);
        void OnConditionMet(CryptoRequestParameters condition, CryptoDataModel model);
        event Action<object, ConditionMetEventArgs> ConditionMet;


    }
}
