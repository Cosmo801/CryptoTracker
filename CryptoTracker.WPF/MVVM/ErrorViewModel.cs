using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTracker.WPF.MVVM
{
    public class ErrorViewModel : INotifyPropertyChanged
    {
        private string _viewModel;
        private string _errorMessage;

        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }

            set
            {
                _errorMessage = value;
                RaisePropertyChanged();
            }
        }
        public string ViewModel
        {
            get
            {
                return _viewModel;
            }

            set
            {
                _viewModel = value;
                RaisePropertyChanged();
            }
        }
           

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([CallerMemberName]string property = null)
        {
            if (PropertyChanged == null) return;

            PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
