using CryptoTracker.Data.Errors;
using CryptoTracker.Data.Models;
using CryptoTracker.Data.Request;
using CryptoTracker.Data.Services.CoinMarketCap;
using CryptoTracker.WPF.Markets.Data;
using CryptoTracker.WPF.MVVM;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Unity;

namespace CryptoTracker.WPF.Markets
{
    public class CryptoListViewModel : CryptoViewModelBase
    {
        

        #region Initialization

        public CryptoListViewModel(ICoinMarketCapService coinMarketCapService)
        {
            _coinMarketCapService = coinMarketCapService;
            FilterCryptoViewModel = ContainerHelper.Container.Resolve<FilterCryptoViewModel>();
            FilterCryptoViewModel.FiltersApplied += FilterCryptoViewModel_FiltersApplied;

            InitializeCommands();
            LoadAsyncData();
        }

        public override void InitializeCommands()
        {
            GetCoinCommand = new RelayCommand(OnGetCoin, CanGetCoin);
            SearchCommand = new RelayCommand(OnSearch);
            ToggleFiltersCommand = new RelayCommand<string>(OnToggleFilters);
        }

        public override void LoadAsyncData()
        {
            
            AllCoinsTask = new TaskWatcher<List<BasicCryptoModel>>(_coinMarketCapService.GetAllCoins());
            AllCoinsTask.PropertyChanged += AllCoinsTask_PropertyChanged;
            

        }

        private async void AllCoinsTask_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "Result")
            {
                CoinList = new ObservableCollection<BasicCryptoModel>(AllCoinsTask.Result);
                FilteredCoinList = CoinList;


                await Task.Delay(120000);
                LoadAsyncData();
            }

            if(e.PropertyName == "IsFaulted")
            {
                RaiseErrorOccured(AllCoinsTask.ErrorMessage);

                await Task.Delay(120000);
                LoadAsyncData();
            }

            return;
        }

        private ICoinMarketCapService _coinMarketCapService;

        #endregion

        #region Functions


        //List
        public TaskWatcher<List<BasicCryptoModel>> AllCoinsTask { get; set; }
        public ObservableCollection<BasicCryptoModel> CoinList
        {
            get
            {
                return _allCoinsList;
            }

            set
            {
                _allCoinsList = value;
                RaisePropertyChanged();
            }
        }
        private ObservableCollection<BasicCryptoModel> _allCoinsList;

        public BasicCryptoModel SelectedCoin
        {
            get
            {
                return _selectedCoin;
            }
            set
            {
                _selectedCoin = value;
                RaisePropertyChanged();
                GetCoinCommand.RaiseCanExecuteChanged();

            }
        }
        private BasicCryptoModel _selectedCoin;

        //Get Coin

        private bool CanGetCoin()
        {
            if (SelectedCoin == null) return false;

            return true;
        }
        private void OnGetCoin()
        {
            if (GetCoin == null) return;
            GetCoin(this, new GetCoinEventArgs(SelectedCoin.Symbol));
        }

        public RelayCommand GetCoinCommand { get; private set; }

        public event GetCoinDelegate GetCoin;
        public delegate void GetCoinDelegate(object sender, GetCoinEventArgs args);


        //Filter and Searching

        private async void FilterCryptoViewModel_FiltersApplied(object sender, ApplyFiltersEventArgs args)
        {
            await FilterCoins(args.Request);
        }

        public async Task FilterCoins(CryptoRequestService requestParameters)
        {
            try
            {
                FilteredCoinList = new ObservableCollection<BasicCryptoModel>(await _coinMarketCapService.GetFilteredCoins(requestParameters));
                FilterMenuVisibility = false;
            }
            catch (CryptoServiceException ex)
            {
                RaiseErrorOccured(ex.Message);
            }


        }

        private void OnToggleFilters(string toggle)
        {
            FilterMenuVisibility = bool.Parse(toggle);
        }

        private void OnSearch()
        {
            var tempSearch = SearchString;

            if (tempSearch == null) return;

            FilteredCoinList = new ObservableCollection<BasicCryptoModel>(CoinList.Where(c => c.Symbol.ToLowerInvariant().Contains(tempSearch) ||
            c.Name.ToLowerInvariant().Contains(tempSearch))
                               .ToList());




        }


        public RelayCommand SearchCommand { get; private set; }
        public RelayCommand<string> ToggleFiltersCommand { get; private set; }

        public string SearchString
        {
            get
            {
                return _searchString;
            }

            set
            {
                _searchString = value;
                RaisePropertyChanged();

                OnSearch();
            }
        }
        private string _searchString;

        public ObservableCollection<BasicCryptoModel> FilteredCoinList
        {
            get
            {
                return _filteredCoinList;
            }

            set
            {
                _filteredCoinList = value;
                RaisePropertyChanged();
            }
        }
        private ObservableCollection<BasicCryptoModel> _filteredCoinList;    

        public FilterCryptoViewModel FilterCryptoViewModel
        {
            get
            {
                return _filterCryptoViewModel;
            }
            set
            {
                _filterCryptoViewModel = value;
                RaisePropertyChanged();
            }
        }
        private FilterCryptoViewModel _filterCryptoViewModel;

        public bool FilterMenuVisibility
        {
            get
            {
                return _filterMenuVisibility;
            }
            set
            {
                _filterMenuVisibility = value;
                RaisePropertyChanged();
            }
        }
        private bool _filterMenuVisibility = false;


        #endregion

    }
}
