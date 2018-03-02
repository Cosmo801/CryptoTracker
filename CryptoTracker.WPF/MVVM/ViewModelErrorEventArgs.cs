using System;

namespace CryptoTracker.WPF.MVVM
{
    public class ViewModelErrorEventArgs : EventArgs
    {

        public string ErrorMessage { get; private set; }

        public ViewModelErrorEventArgs(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
