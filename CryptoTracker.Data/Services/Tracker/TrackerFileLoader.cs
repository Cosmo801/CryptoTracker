using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CryptoTracker.Data.Helpers;
using CryptoTracker.Data.Models.Tracker;
using Newtonsoft.Json;

namespace CryptoTracker.Data.Services.Tracker
{
    public class TrackerFileLoader : ITrackerLoader
    {
       

        public void AddCrypto(CryptoDataModel model)
        {

            cryptoForAddList.Add(CryptoModelConverter.GetSerializedModel(model));

        }

        public async Task<List<SerializedCryptoModel>> LoadCrypto()
        {
            try
            {
                FileStream fileStream;
                string savedCryptoJson;

                using(fileStream = new FileStream("TrackedCrypto.json", FileMode.OpenOrCreate))
                {
                    if (fileStream.Length == 0) return new List<SerializedCryptoModel>();
                    var reader = new StreamReader(fileStream);
                    savedCryptoJson = await reader.ReadToEndAsync().ConfigureAwait(false);

                    reader.Close();
                    fileStream.Close();
                }

                return JsonConvert.DeserializeObject<List<SerializedCryptoModel>>(savedCryptoJson);

                
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task RemoveCrypto(CryptoDataModel model)
        {
            try
            {

                var savedCrypto = await LoadCrypto().ConfigureAwait(false);

                if (savedCrypto.Count() == 0) return;
                if (!savedCrypto.Any(c => c.Symbol == model.Data.Symbol)) return;

                cryptoForDeleteList.Add(CryptoModelConverter.GetSerializedModel(model));



                
               

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> SaveChanges()
        {
            FileStream fileStream;

            try
            {            

                List<SerializedCryptoModel> cryptoForSaving  = await LoadCrypto().ConfigureAwait(false);
                

                if(cryptoForAddList.Count != 0)
                {
                    foreach(var crypto in cryptoForAddList)
                    {
                        if (cryptoForSaving.Any(s => s.Symbol == crypto.Symbol)) continue;
                        cryptoForSaving.Add(crypto);
                    }
                }
                if(cryptoForDeleteList.Count != 0)
                {
                    foreach(var crypto in cryptoForDeleteList)
                    {
                        var cryptoForDelete = cryptoForSaving.FirstOrDefault(s => s.Symbol == crypto.Symbol);
                        if (cryptoForDelete != null) cryptoForSaving.Remove(cryptoForDelete);

                    }
                }


                using (fileStream = new FileStream("TrackedCrypto.json", FileMode.Truncate))
                {
                    var streamWriter = new StreamWriter(fileStream);
                    var cryptoForSavingJson = JsonConvert.SerializeObject(cryptoForSaving);
                    await streamWriter.WriteAsync(cryptoForSavingJson).ConfigureAwait(false);

                    streamWriter.Close();
                    fileStream.Close();
                }


                cryptoForAddList = new List<SerializedCryptoModel>();
                cryptoForDeleteList = new List<SerializedCryptoModel>();

               

                RaiseCryptoChanged();

                return true;

                
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void RaiseCryptoChanged()
        {
            SaveCryptoChanged(this, new EventArgs());
        }

        public event Action<object, EventArgs> SaveCryptoChanged;

        private List<SerializedCryptoModel> cryptoForAddList = new List<SerializedCryptoModel>();
        private List<SerializedCryptoModel> cryptoForDeleteList = new List<SerializedCryptoModel>();
    }
}
