using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoTracker.Data.Helpers;
using CryptoTracker.Data.Models;
using CryptoTracker.Data.Models.Tracker;
using CryptoTracker.Data.Request;
using CryptoTracker.Data.Services.CryptoCompare;
using CryptoTracker.Data.Services.Tracker.Data;

namespace CryptoTracker.Data.Services.Tracker
{
    public class TrackerPriceService : ITrackerPriceService
    {
        private ITrackerLoader _trackerLoader;
        private ICryptoCompareService _compareService;
        private ContinuousTaskFactory _taskFactory;
        private List<SerializedCryptoModel> _savedCrypto;
        private List<CryptoDataModel> _cryptoDataModels;

        public TrackerPriceService(ITrackerLoader trackerLoader, ICryptoCompareService compareService)
        {
            _trackerLoader = trackerLoader;
            _compareService = compareService;
            _taskFactory = new ContinuousTaskFactory();

            trackerLoader.SaveCryptoChanged += TrackerLoader_SaveCryptoChanged;
            _taskFactory.TaskCompleted += _taskFactory_TaskCompleted;      
        }

        private void _taskFactory_TaskCompleted(object arg1, EventArgs arg2)
        {
            OnTaskCompleted();
        }

        private async void TrackerLoader_SaveCryptoChanged(object arg1, EventArgs arg2)
        {
            if(_taskFactory.IsRunning) _taskFactory.EndTask();
            await LoadCrypto().ConfigureAwait(false);
            DownloadCryptoData();
            StartTracker();
            
        }

        public async Task LoadCrypto()
        {
            _savedCrypto = await _trackerLoader.LoadCrypto().ConfigureAwait(false);
        }
    
        public async Task<List<CryptoDataModel>> GetTrackedCrypto()
        {
            if(_cryptoDataModels == null)
            {
                await LoadCrypto();
                DownloadCryptoData();
                return _cryptoDataModels;
            }

            return _cryptoDataModels;
          
        }

        public void StartTracker(int delay = 30000)
        {
            _taskFactory.CreateTask(DownloadCryptoData, delay, CheckConditions);
        }

        public void DownloadCryptoData()
        {
            if (_savedCrypto == null) return;

            var downloadedCrypto = new List<CryptoDataModel>();

            var cryptoDownloadTaskList = new List<Task<BasicCryptoModel>>();

            for (int i = 0; i < _savedCrypto.Count; i++)
            {
                cryptoDownloadTaskList.Add(_compareService.GetBasicCrypto(_savedCrypto[i].Symbol));
            }

            //Wait all one by one

            while (cryptoDownloadTaskList.Count > 0)
            {
                var index = Task.WaitAny(cryptoDownloadTaskList.ToArray());
                downloadedCrypto.Add(new CryptoDataModel
                {
                    Data = cryptoDownloadTaskList[index].Result,
                    Conditions = CryptoModelConverter.GetUnserializedRequestParameters(_savedCrypto.Single(s => s.Symbol == cryptoDownloadTaskList[index].Result.Symbol))
                });
                cryptoDownloadTaskList.Remove(cryptoDownloadTaskList[index]);

            }

            _cryptoDataModels = downloadedCrypto;


        }

        public void CheckConditions()
        {
            if (_cryptoDataModels == null) return;
            //DO this method better
            foreach (var crypto in _cryptoDataModels)
            {

                foreach (var condition in crypto.Conditions)
                {
                    switch (condition.Type)
                    {
                        case RequestFilterType.Maximum:
                            if (condition.Property == RequestPropertyType.USDPrice)
                            {
                                if (crypto.Data.USDPrice <= condition.Value)
                                {
                                    OnConditionMet(condition, crypto);
                                }

                            }
                            if (condition.Property == RequestPropertyType.BTCPrice)
                            {
                                if (crypto.Data.BTCPrice <= condition.Value)
                                {
                                    OnConditionMet(condition, crypto);
                                }

                            }
                            if (condition.Property == RequestPropertyType.MarketCap)
                            {
                                if (crypto.Data.MarketCap <= condition.Value)
                                {
                                    OnConditionMet(condition, crypto);

                                }

                            }
                            if (condition.Property == RequestPropertyType.Volume24h)
                            {
                                if (crypto.Data.Volume24h <= condition.Value)
                                {
                                    OnConditionMet(condition, crypto);

                                }

                            }
                            if (condition.Property == RequestPropertyType.TotalSupply)
                            {
                                if (crypto.Data.TotalSupply <= condition.Value)
                                {
                                    OnConditionMet(condition, crypto);
                                }

                            }
                            if (condition.Property == RequestPropertyType.Change24H)
                            {
                                if (crypto.Data.Change24h <= condition.Value)
                                {
                                    OnConditionMet(condition, crypto);
                                }

                            }
                            if (condition.Property == RequestPropertyType.CirculatingSupply)
                            {
                                if (crypto.Data.CirculatingSupply <= condition.Value)
                                {
                                    OnConditionMet(condition, crypto);

                                }

                            }
                            break;

                        case RequestFilterType.Minimum:
                            if (condition.Property == RequestPropertyType.USDPrice)
                            {
                                if (crypto.Data.USDPrice >= condition.Value)
                                {
                                    OnConditionMet(condition, crypto);
                                }

                            }
                            if (condition.Property == RequestPropertyType.BTCPrice)
                            {
                                if (crypto.Data.BTCPrice >= condition.Value)
                                {
                                    OnConditionMet(condition, crypto);

                                }

                            }
                            if (condition.Property == RequestPropertyType.MarketCap)
                            {
                                if (crypto.Data.MarketCap >= condition.Value)
                                {
                                    OnConditionMet(condition, crypto);
                                }

                            }
                            if (condition.Property == RequestPropertyType.Volume24h)
                            {
                                if (crypto.Data.Volume24h >= condition.Value)
                                {
                                    OnConditionMet(condition, crypto);

                                }

                            }
                            if (condition.Property == RequestPropertyType.TotalSupply)
                            {
                                if (crypto.Data.TotalSupply >= condition.Value)
                                {
                                    OnConditionMet(condition, crypto);
                                }

                            }
                            if (condition.Property == RequestPropertyType.Change24H)
                            {
                                if (crypto.Data.Change24h >= condition.Value)
                                {
                                    OnConditionMet(condition, crypto);
                                }

                            }
                            if (condition.Property == RequestPropertyType.CirculatingSupply)
                            {
                                if (crypto.Data.CirculatingSupply >= condition.Value)
                                {
                                    OnConditionMet(condition, crypto);
                                }

                            }
                            break;


                    }
                }
            }

        }

        public void OnConditionMet(CryptoRequestParameters condition, CryptoDataModel model)
        {
            var service = new CryptoRequestService();
            service.AddFilter(condition);

            var conditionString = service.GetFilters().Single();


            if (ConditionMet == null) return;
            ConditionMet(this, new ConditionMetEventArgs(model.Data.Symbol, conditionString));
        }

        public void OnTaskCompleted()
        {
            TaskComplete(this, new EventArgs());
        }

        public event Action<object, ConditionMetEventArgs> ConditionMet;

        public event Action<object, EventArgs> TaskComplete;
    }
}
