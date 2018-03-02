using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CryptoTracker.WPF.Tracker
{
    public class TrackerPopupViewModel : INotifyPropertyChanged
    {
        private string _symbol;
        private string _condition;

        public string Symbol
        {
            get
            {
                return _symbol;
            }
            set
            {
                _symbol = value;
                RaisePropertyChanged();
            }
        }
        public string Condition
        {
            get
            {
                return _condition;
            }
            set
            {
                _condition = value;
                RaisePropertyChanged();
            }
        }


        public void LoadData(string symbol, string condition)
        {
            Symbol = symbol;
            Condition = condition;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([CallerMemberName]string property = null)
        {
            if (PropertyChanged == null) return;
            PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
