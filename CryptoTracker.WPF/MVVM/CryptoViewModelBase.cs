using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CryptoTracker.WPF.MVVM
{
    public abstract class CryptoViewModelBase : INotifyPropertyChanged
    {
        public abstract void LoadData();
        public abstract void InitializeCommands();

        private Dictionary<string, bool> _propertySet = new Dictionary<string, bool>(); 
        
        protected void PropertySet(string propertyName)
        {
            try
            {
                if (_propertySet.ContainsKey(propertyName)) return;
               _propertySet.Add(propertyName, true);
            }
            catch (ArgumentException)
            {
                throw;
            }
            
        }
        protected bool IsPropertySet(string propertyName)
        {
            bool value;
            return _propertySet.TryGetValue(propertyName, out value);
        }



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

            PropertySet(propertyName);
        }
    }
}
