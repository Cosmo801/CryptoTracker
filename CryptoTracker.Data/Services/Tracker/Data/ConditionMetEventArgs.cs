using System;

namespace CryptoTracker.Data.Services.Tracker.Data
{
    /// <summary>
    /// A crypto in the tracker has fulfilled its condition and user must be notified
    /// </summary>

    public class ConditionMetEventArgs : EventArgs
    {
        public ConditionMetEventArgs(string crypto, string condition)
        {
            Crypto = crypto;
            Condition = condition;
        }

        public string Crypto { get; set; }
        public string Condition { get; set; }


    }
}
