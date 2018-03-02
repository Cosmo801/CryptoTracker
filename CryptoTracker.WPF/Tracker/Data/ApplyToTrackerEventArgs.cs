using CryptoTracker.Data.Models.Tracker;
using System;

namespace CryptoTracker.WPF.Tracker.Data
{
    public class ApplyToTrackerEventArgs : EventArgs
    {
        public CryptoDataModel Crypto { get; set; }
        public bool IsAdd { get; set; }

        public ApplyToTrackerEventArgs(CryptoDataModel crypto, bool isAdd)
        {
            Crypto = crypto;
            IsAdd = isAdd;
        }
    }
}
