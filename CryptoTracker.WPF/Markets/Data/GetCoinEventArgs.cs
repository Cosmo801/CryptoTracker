using System;

namespace CryptoTracker.WPF.Markets.Data
{
    public class GetCoinEventArgs : EventArgs
    {
        public string CoinName { get; set; }

        public GetCoinEventArgs(string coinName)
        {
            CoinName = coinName;
        }
    }
}
