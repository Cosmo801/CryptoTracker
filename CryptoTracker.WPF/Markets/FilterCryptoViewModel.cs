using CryptoTracker.Data.Request;
using CryptoTracker.Data.Services.CoinMarketCap;
using CryptoTracker.WPF.Markets.Data;
using CryptoTracker.WPF.MVVM;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;

namespace CryptoTracker.WPF.Markets
{
    public class FilterCryptoViewModel : CryptoViewModelBase
    {

        #region Initialization

        public FilterCryptoViewModel(ICoinMarketCapService coinMarketCapService)  
        {
            _coinMarketCapService = coinMarketCapService;

            LoadData();
            InitializeCommands();
        }

        public override void LoadData()
        {
            _allCoinRequestService = new CryptoRequestService();

            CurrentRequestPropertyType = RequestPropertyType.USDPrice;
            CurrentRequestFilterType = RequestFilterType.Maximum;

        }

        public override void InitializeCommands()
        {
            AddFilterCommand = new RelayCommand(OnAddFilter, CanAddFilter);
            ApplyFiltersCommand = new RelayCommand(OnApplyFilters, CanApplyFilters);
            RemoveFilterCommand = new RelayCommand(OnRemoveFilter, CanRemoveFilter);
        }

        private ICoinMarketCapService _coinMarketCapService;
        private CryptoRequestService _allCoinRequestService;

        #endregion

        #region Filtering

        //Add
        private void OnAddFilter()
        {

            _allCoinRequestService.AddFilter(new CryptoRequestParameters
            {
                Property = CurrentRequestPropertyType,
                Type = CurrentRequestFilterType,
                Value = CurrentRequestValue
            });


            FilterList = new ObservableCollection<string>(_allCoinRequestService.GetFilters());
            CurrentFilter = new CryptoRequestParameters();
        }
        private bool CanAddFilter()
        {
            if (CurrentRequestValue == 0) return false;
            return true;
        }

        public RelayCommand AddFilterCommand { get; private set; }


        //Remove
        private bool CanRemoveFilter()
        {
            if (SelectedFilterString == null) return false;
            return true;
        }
        private void OnRemoveFilter()
        {
            _allCoinRequestService.RemoveFilter(SelectedFilterString);
            FilterList = new ObservableCollection<string>(_allCoinRequestService.GetFilters());
        }

        public RelayCommand RemoveFilterCommand { get; private set; }



        //Apply

        private bool CanApplyFilters()
        {
            if (_allCoinRequestService.GetFilters() != null) return true;
            return false;
        }
        private void OnApplyFilters()
        {
            if (FiltersApplied == null) return;
            FiltersApplied(this, new ApplyFiltersEventArgs(_allCoinRequestService));
        }


        public RelayCommand ApplyFiltersCommand { get; private set; }

        public event ApplyFiltersDelegate FiltersApplied;
        public delegate void ApplyFiltersDelegate(object sender, ApplyFiltersEventArgs args);


        public CryptoRequestParameters CurrentFilter
        {
            get
            {
                return _currentFilter;
            }
            set
            {
                _currentFilter = value;
                RaisePropertyChanged();
            }
        }
        private CryptoRequestParameters _currentFilter;

        public RequestPropertyType CurrentRequestPropertyType
        {
            get
            {
                return _currentRequestPropertyType;
            }
            set
            {
                _currentRequestPropertyType = value;
                RaisePropertyChanged();

            }
        }
        private RequestPropertyType _currentRequestPropertyType;

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

        public ObservableCollection<string> FilterList
        {
            get
            {
                return _filterList;
            }
            set
            {
                _filterList = value;
                RaisePropertyChanged();
            }
        }
        private ObservableCollection<string> _filterList;

        public string SelectedFilterString
        {
            get
            {
                return _selectedFilterString;
            }
            set
            {
                _selectedFilterString = value;
                RaisePropertyChanged();

                RemoveFilterCommand.RaiseCanExecuteChanged();
            }
        }
        private string _selectedFilterString;

        #endregion


























    }
}
