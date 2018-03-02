using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using CryptoTracker.Data.Helpers;
using CryptoTracker.Data.Services.Tracker.Data;
using CryptoTracker.Data.Models.Tracker;
using CryptoTracker.Data.Services.CryptoCompare;

namespace CryptoTracker.Data.Services.Tracker
{
    public class TrackerFileLoaderService : ITrackerLoaderService
    {
        /// <summary>
        /// Load cryptocurrencies for tracking from a file
        /// </summary>


        public TrackerFileLoaderService()
        {
            _cryptoForUpdateList = new List<SerializedCryptoModel>();
        }

        public async Task<bool> IsTrackable(CryptoDataModel model)
        {    
            //Check if cryptocurrency price data is available
            var compareService = new CryptoCompareService();

            var availableSymbols = await compareService.GetAvailableCrypto();

            if (!availableSymbols.Contains(model.Data.Symbol)) return false;

            return true;
        }

        public async Task AddCrypto(CryptoDataModel model)
        {

            try
            {
                if (!await IsTrackable(model)) throw new Exception("Symbol unavailable for tracking service");

                var cryptoList = await LoadCrypto();
                var serializableModel = CryptoModelConverter.GetSerializedModel(model);

                if (!cryptoList.Contains(serializableModel))
                {
                    cryptoList.Add(serializableModel);
                }

                _cryptoForUpdateList = cryptoList;
                await SaveChanges();

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
                file = new FileStream("trackedCrypto.json", FileMode.Open);
                var streamReader = new StreamReader(file);

                var fileContent = await streamReader.ReadToEndAsync();
                serializedCrypto = JsonConvert.DeserializeObject<List<SerializedCryptoModel>>(fileContent);



                return serializedCrypto;





            }
            catch (FileNotFoundException)
            {
                File.Create("trackedCrypto.json");
                return serializedCrypto;

            }
            finally
            {
                file.Close();
            }
        }
    
        public async Task RemoveCrypto(CryptoDataModel model)
        {
            try
            {
                var cryptoList = await LoadCrypto();
                var serializableModel = CryptoModelConverter.GetSerializedModel(model);
                if (!cryptoList.Contains(serializableModel)) return;

                cryptoList.Remove(serializableModel);

                _cryptoForUpdateList = cryptoList;
                await SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task SaveChanges()
        {
            FileStream file = null;
            StreamWriter writer = null;

            try
            {
                var model = _cryptoForUpdateList;
                var cryptoJson = JsonConvert.SerializeObject(model);
                file = new FileStream("trackedCrypto.json", FileMode.Create);
                writer = new StreamWriter(file);

                await writer.WriteAsync(cryptoJson);

                OnSavedCryptoChanged();


            }
            catch (Exception)
            {

                throw;
            }

            finally
            {
                file.Close();
                writer.Close();
            }
        }

        public void OnSavedCryptoChanged()
        {
            _cryptoForUpdateList = new List<SerializedCryptoModel>();
            SavedCryptoChanged(this, new CryptoLoaderEventArgs());
        }

        public event Action<object, CryptoLoaderEventArgs> SavedCryptoChanged;
        private List<SerializedCryptoModel> _cryptoForUpdateList;
    }
}
