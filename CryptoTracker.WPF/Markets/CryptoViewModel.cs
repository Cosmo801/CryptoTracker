using CryptoTracker.Data.Errors;
using CryptoTracker.Data.Models;
using CryptoTracker.Data.Services.CryptoCompare;
using CryptoTracker.WPF.MVVM;

namespace CryptoTracker.WPF.Markets
{
    public class CryptoViewModel : CryptoViewModelBase
    {

        #region Initialization

        public CryptoViewModel(ICryptoCompareService cryptoCompareService)
        {
            _cryptoCompareService = cryptoCompareService;

        }

        public override void InitializeCommands()
        {
            
        }

        public override void LoadAsyncData()
        {
            
           if (SelectedCoinString == null) return;
           GetCryptoTask = new TaskWatcher<AdvancedCryptoModel>(_cryptoCompareService.GetCrypto(SelectedCoinString));

           GetCryptoTask.PropertyChanged += GetCryptoCommand_PropertyChanged;

        }


        private void GetCryptoCommand_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Result") SelectedCrypto = GetCryptoTask.Result;
            if (e.PropertyName == "IsFaulted") RaiseErrorOccured(GetCryptoTask.ErrorMessage);

            return;

        }


        public TaskWatcher<AdvancedCryptoModel> GetCryptoTask { get; private set; }

        private ICryptoCompareService _cryptoCompareService;

        #endregion


        #region Functions
        //Display
        public string SelectedCoinString { get; set; }

        public AdvancedCryptoModel SelectedCrypto
        {
            get
            {
                return _selectedCrypto;
            }

            set
            {
                _selectedCrypto = value;
                RaisePropertyChanged();
            }
        }
        private AdvancedCryptoModel _selectedCrypto;


        #endregion




    }
}
