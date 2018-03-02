using CryptoTracker.Data.Models;
using System;

namespace CryptoTracker.WPF.Markets.Data
{
    public class EditTrackerEventArgs : EventArgs
    {
        public BasicCryptoModel SelectedCrypto { get; set; }
        public bool Add { get; set; }

        public EditTrackerEventArgs(BasicCryptoModel crypto, bool isAdd)
        {
            SelectedCrypto = crypto;
            Add = isAdd;
        }

    }
}
