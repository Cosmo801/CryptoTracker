using CryptoTracker.Data.Request;
using System.Collections.Generic;

namespace CryptoTracker.Data.Models.Tracker
{
    public class CryptoDataModel
    {
        public BasicCryptoModel Data { get; set; }
        public List<CryptoRequestParameters> Conditions { get; set; }




    }
}
