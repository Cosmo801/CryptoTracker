using System;

namespace CryptoTracker.WPF.Markets.Data
{
    public class FilterCoinEventArgs : EventArgs
    {
        public FilterCoinEventArgs()
        {

        }

        public FilterCoinEventArgs(string searchQuery)
        {
            SearchQuery = searchQuery;
        }



        public string SearchQuery { get; set; }
    }
}
