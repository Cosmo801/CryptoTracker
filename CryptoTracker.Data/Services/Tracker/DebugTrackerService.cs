using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoTracker.Data.Models.Tracker;
using CryptoTracker.Data.Services.Tracker.Data;
using CryptoTracker.Data.Helpers;
using CryptoTracker.Data.Services.CryptoCompare;
using System.IO;
using Newtonsoft.Json;
using CryptoTracker.Data.Request;
using CryptoTracker.Data.Models;
using System.Linq.Dynamic;

namespace CryptoTracker.Data.Services.Tracker
{
    public class DebugTrackerService : ITrackerLoaderService, ITrackerService
    {
       /// <summary>
       /// Creates continuous tasks from cryptocurrencies stored in the tracker to download price data
       /// </summary>



        public DebugTrackerService()
        {
            _factory = new ContinuousTaskFactory();
            _cryptoForUpdateList = new List<SerializedCryptoModel>();
            _compareService = new CryptoCompareService();

            GetTrackedCryptoValue(30000);
        }



        #region loader

        public bool CanSave { get; set; } = false;

        public bool CryptoUpdated { get; set; } = false;

        public async Task<bool> IsTrackable(CryptoDataModel model)
        {
            try
            {
                var compareService = new CryptoCompareService();

                var availableSymbols = await compareService.GetAvailableCrypto();

                if (!availableSymbols.Contains(model.Data.Symbol)) return false;

                return true;
            }

            catch (Exception)
            {

                throw;
            }
            
        }
     
        public async Task AddCrypto(CryptoDataModel model)
        {
            try
            {
                if (!await IsTrackable(model)) throw new ArgumentException("Symbol unavailable for tracking service");

                var cryptoList = await LoadCrypto();
                var serializableModel = CryptoModelConverter.GetSerializedModel(model);

                if (cryptoList.Any(c => c.Symbol == model.Data.Symbol)) return;

                cryptoList.Add(serializableModel);
                _cryptoForUpdateList = cryptoList;
                CanSave = true;
                 
                
                
               

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<SerializedCryptoModel>> LoadCrypto()
        {
            FileStream file = null;
            List<SerializedCryptoModel> serializedCrypto = new List<SerializedCryptoModel>();


            try
            {

                string content;
                using(var reader = new StreamReader("trackedCrypto.json"))
                {
                    content = await reader.ReadToEndAsync();
                }

                if (string.IsNullOrEmpty(content)) return serializedCrypto;
                serializedCrypto = JsonConvert.DeserializeObject<List<SerializedCryptoModel>>(content);
               
                return serializedCrypto;

            }
            catch (FileNotFoundException)
            {
                File.Create("trackedCrypto.json");

            }
            catch (JsonSerializationException)
            {
                File.Create("trackedCrypto.json");
            }

            finally
            {
                if(file != null) file.Close();

            }
            return serializedCrypto;
        }    

        public async Task RemoveCrypto(CryptoDataModel model)
        {
            try
            {
                var cryptoList = await LoadCrypto();
                if (cryptoList.Count == 0) return;

                if (!cryptoList.Any(c => c.Symbol == model.Data.Symbol)) return;
                //var serializableModel = CryptoModelConverter.GetSerializedModel(model);
                //if (!cryptoList.Any(c => c.Symbol == model.Data.Symbol)) return;

                var cryptoForDelete = cryptoList.Single(c => c.Symbol == model.Data.Symbol);
                cryptoList.Remove(cryptoForDelete);

                _cryptoForUpdateList = cryptoList;
                CanSave = true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task SaveChanges()
        {

            
            try
            {
                if (!CanSave) return;

                var models = _cryptoForUpdateList;

                var cryptoJson = JsonConvert.SerializeObject(models);

                File.WriteAllText("trackedCrypto.json", cryptoJson);

               
               

                CanSave = false;
                _cryptoForUpdateList = new List<SerializedCryptoModel>();

                UpdateTracker();




            }
            catch (Exception ex)
            {


                throw;
            }

            finally
            {
                CanSave = false;
            }

           
        }

        private void UpdateTracker()
        {
            try
            {
                _factory.EndTask();
                GetTrackedCryptoValue(30000);
            }
            catch (Exception ex)
            {
                throw;
            }
           
        }

        #endregion


        public event Action<object, ConditionMetEventArgs> ConditionMet;

        public void CheckConditions(List<CryptoDataModel> models)
        {
            

           foreach(var crypto in models)
            {
                
                foreach(var condition in crypto.Conditions)
                {
                    switch (condition.Type)
                    {
                        case RequestFilterType.Maximum:
                            if(condition.Property == RequestPropertyType.USDPrice)
                            {
                                if(crypto.Data.USDPrice <= condition.Value)
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

        public async Task<List<CryptoDataModel>> LoadCryptoDataModels()
        {
            var cryptoDataModels = new List<CryptoDataModel>();

            try
            {
                var serializedCrypto = await LoadCrypto();

                var taskList = new List<Task<BasicCryptoModel>>();

                for (int i = 0; i < serializedCrypto.Count; i++)
                {
                    taskList.Add(_compareService.GetBasicCrypto(serializedCrypto[i].Symbol));
                }

                while (taskList.Count > 0)
                {
                    var index = Task.WaitAny(taskList.ToArray());
                    cryptoDataModels.Add(new CryptoDataModel
                    {
                        Data = taskList[index].Result,
                        Conditions = CryptoModelConverter.GetUnserializedRequestParameters(serializedCrypto.Single(s => s.Symbol == taskList[index].Result.Symbol))
                    });
                    taskList.Remove(taskList[index]);

                }


                _cryptoDataModels = cryptoDataModels;
                return cryptoDataModels;

            }
            catch (Exception)
            {
                _cryptoDataModels = cryptoDataModels;
                return cryptoDataModels;

            }
        }

        public async Task GetTrackedCryptoValue(int delay)
        {
            await Task.Factory.StartNew(() =>
            {
                Action callback = (() =>
                {
                    Task.Factory.StartNew(() =>
                    {
                        CheckConditions(_cryptoDataModels);
                    });
                });



                _factory.CreateTask(LoadCryptoDataModels, delay, callback);

            }).ConfigureAwait(false);


        }

        public void OnConditionMet(CryptoRequestParameters condition, CryptoDataModel model)
        {
            var service = new CryptoRequestService();
            service.AddFilter(condition);

            var conditionString = service.GetFilters().Single();


            if (ConditionMet == null) return;
            ConditionMet(this, new ConditionMetEventArgs(model.Data.Symbol, conditionString));
        }

        private ContinuousTaskFactory _factory;
        private CryptoCompareService _compareService;
        private List<SerializedCryptoModel> _cryptoForUpdateList;
        private List<CryptoDataModel> _cryptoDataModels;
    }
}
