using System;
using System.Collections.Generic;

namespace CryptoTracker.Data.Models.Tracker
{

    public class SerializedCryptoModel
    {
        public string Symbol { get; set; }
        public List<Dictionary<string, string>> Conditions { get; set; }



    }
}
