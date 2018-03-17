using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CryptoTracker.WPF.MVVM
{
    public abstract class CryptoViewModelBase : INotifyPropertyChanged
    {
        public abstract void LoadAsyncData();
        public abstract void InitializeCommands();


        public event Action<object, ViewModelErrorEventArgs> ErrorOccured;
        protected void RaiseErrorOccured(string message)
        {
            if (ErrorOccured == null) return;
            ErrorOccured(this, new ViewModelErrorEventArgs(message));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            if (PropertyChanged == null) return;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

          
        }
    }
}
