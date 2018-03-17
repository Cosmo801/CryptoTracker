using CryptoTracker.Data.Models;
using CryptoTracker.Data.Models.Tracker;
using CryptoTracker.Data.Request;
using CryptoTracker.Data.Services.CryptoCompare;
using CryptoTracker.WPF.MVVM;
using CryptoTracker.WPF.Tracker.Data;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CryptoTracker.WPF.Tracker
{
    public class AddToTrackerViewModel : CryptoViewModelBase
    {
       

        #region Initialization

        public AddToTrackerViewModel(ICryptoCompareService compareService)
        {
            _compareService = compareService;


            InitializeCommands();
            LoadAsyncData();
        }

        public override void InitializeCommands()
        {
            AddCryptoToTrackerCommand = new RelayCommand(OnAddToTracker, CanAddToTracker);
            AddFilterCommand = new RelayCommand(OnAddFilter, CanAddFilter);
            RemoveFilterCommand = new RelayCommand(OnRemoveFilter, CanRemoveFilter);
        }

        public override void LoadAsyncData()
        {
           
           GetCryptoListTask = new TaskWatcher<List<string>>(_compareService.GetAvailableCrypto());
           GetCryptoListTask.PropertyChanged += CoinListLoaded;
            
            FilterDictionary = new Dictionary<string, CryptoRequestParameters>();
            FilterStrings = new ObservableCollection<string>();
        }

        private void CoinListLoaded(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Result") CryptoStringList = new ObservableCollection<string>(GetCryptoListTask.Result.OrderBy(s => s));

            if (e.PropertyName == "IsFaulted") RaiseErrorOccured(GetCryptoListTask.ErrorMessage);

            return; 
        }

        public TaskWatcher<List<string>> GetCryptoListTask { get; set; }

        private ICryptoCompareService _compareService;


        #endregion

        #region Functions

        //Add

        private bool CanAddFilter()
        {
            if (CurrentRequestValue == 0) return false;
            return true;
        }

        private void OnAddFilter()
        {
            var filter = new CryptoRequestParameters
            {
                Property = _currentRequestProperty,
                Type = CurrentRequestFilterType,
                Value = CurrentRequestValue
            };

            var filterString = CryptoRequestService.ParseParameter(filter);

            try
            {
                FilterDictionary.Add(filterString, filter);
                FilterStrings.Add(filterString);

                AddCryptoToTrackerCommand.RaiseCanExecuteChanged();
            }

            catch (ArgumentException)
            {

                return;
            }






        }

        public RelayCommand AddFilterCommand { get; private set; }


        public RequestPropertyType CurrentRequestProperty
        {
            get
            {
                return _currentRequestProperty;
            }

            set
            {
                _currentRequestProperty = value;
                RaisePropertyChanged();

            }
        }
        private RequestPropertyType _currentRequestProperty;

        public RequestFilterType CurrentRequestFilterType
        {
            get
            {
                return _currentRequestFilterType;
            }

            set
            {
                _currentRequestFilterType = value;
                RaisePropertyChanged();

            }
        }
        private RequestFilterType _currentRequestFilterType;

        public decimal CurrentRequestValue
        {
            get
            {
                return _currentRequestValue;

            }

            set
            {
                _currentRequestValue = value;
                RaisePropertyChanged();

                AddFilterCommand.RaiseCanExecuteChanged();

            }
        }
        private decimal _currentRequestValue;

        public string SelectedCryptoName
        {
            get
            {
                return _selectedCryptoName;
            }
            set
            {
                _selectedCryptoName = value;
                RaisePropertyChanged();
                AddCryptoToTrackerCommand.RaiseCanExecuteChanged();
            }
        }
        private string _selectedCryptoName;


        //Remove

        private bool CanRemoveFilter()
        {
            if (SelectedFilter == null) return false;
            return true;
        }

        private void OnRemoveFilter()
        {
            FilterDictionary.Remove(SelectedFilter);
            FilterStrings.Remove(SelectedFilter);

          
        }

        public RelayCommand AddCryptoToTrackerCommand { get; private set; }

        //Apply
        
        private bool CanAddToTracker()
        {
            if (SelectedCryptoName == null) return false;
            if (FilterDictionary.Count > 0) return true;
            return false;
        }

        private async void OnAddToTracker()
        {
            if (AppliedToTracker == null) return;


            var cryptoDataModel = new CryptoDataModel
            {
                Data = await _compareService.GetBasicCrypto(SelectedCryptoName),
                Conditions = FilterDictionary.Values.ToList()
            };

            AppliedToTracker(this, new ApplyToTrackerEventArgs(cryptoDataModel, true));

         

        }

        public RelayCommand RemoveFilterCommand { get; private set; }
        public event Action<object, ApplyToTrackerEventArgs> AppliedToTracker;
      

        public BasicCryptoModel SelectedCrypto
        {
            get
            {
                return _selectedCrypto;
            }
            set
            {
                _selectedCrypto = value;
                SelectedCryptoName = value.Symbol;
                RaisePropertyChanged();


            }
        }
        private BasicCryptoModel _selectedCrypto;

        public string SelectedFilter
        {
            get
            {
                return _selectedFilter;
            }
            set
            {
                _selectedFilter = value;
                RaisePropertyChanged();
                RemoveFilterCommand.RaiseCanExecuteChanged();
            }
        }
        private string _selectedFilter;

        public ObservableCollection<string> FilterStrings
        {
            get
            {
                return _filterStrings;
            }

            set
            {
                _filterStrings = value;
                RaisePropertyChanged();


            }
        }
        private ObservableCollection<string> _filterStrings;

        public Dictionary<string, CryptoRequestParameters> FilterDictionary
        {
            get
            {
                return _filterDictionary;
            }

            set
            {
                _filterDictionary = value;
            }
        }
        private Dictionary<string, CryptoRequestParameters> _filterDictionary;

        public ObservableCollection<string> CryptoStringList
        {
            get
            {
                return _cryptoStringList;
            }
            set
            {
                _cryptoStringList = value;
                RaisePropertyChanged();
            }
        }
        private ObservableCollection<string> _cryptoStringList;



        #endregion



    }
}
