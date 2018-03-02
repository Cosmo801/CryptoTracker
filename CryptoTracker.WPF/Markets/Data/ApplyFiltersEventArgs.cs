using CryptoTracker.Data.Request;
using System;

namespace CryptoTracker.WPF.Markets.Data
{
    public class ApplyFiltersEventArgs : EventArgs
    {
        public CryptoRequestService Request { get; set; }

        public ApplyFiltersEventArgs(CryptoRequestService request)
        {
           Request = request;
        }
    }
}
