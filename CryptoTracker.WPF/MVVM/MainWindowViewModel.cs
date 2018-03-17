using CryptoTracker.WPF.Markets;
using GalaSoft.MvvmLight.Command;
using System;
using System.Threading.Tasks;
using Unity;
using CryptoTracker.WPF.Tracker;
using CryptoTracker.WPF.Settings;
using System.Media;
using System.Windows;

namespace CryptoTracker.WPF.MVVM
{
    public class MainWindowViewModel : CryptoViewModelBase
    {
        /// <summary>
        /// Centralized control of the views and view models of the application
        /// </summary>



        #region Initialization

        public MainWindowViewModel()
        {
            LoadAsyncData();
            InitializeCommands();

            _cryptoListViewModel.ErrorOccured += ViewModelErrorOccured;
            _filterCryptoViewModel.ErrorOccured += ViewModelErrorOccured;
            _cryptoViewModel.ErrorOccured += ViewModelErrorOccured;
            _trackCryptoViewModel.ErrorOccured += ViewModelErrorOccured;


            _cryptoListViewModel.GetCoin += OnGetCoin;
            _filterCryptoViewModel.FiltersApplied += OnFiltersApplied;
            _trackCryptoViewModel.TrackerViewModelConditionMet += OnTrackerViewModelConditionMet;


        }

        public override void LoadAsyncData()
        {
            _errorViewModel = ContainerHelper.Container.Resolve<ErrorViewModel>();
            _trackerPopupViewModel = ContainerHelper.Container.Resolve<TrackerPopupViewModel>();
            _mainSettingsViewModel = ContainerHelper.Container.Resolve<MainSettingsViewModel>();
            _cryptoViewModel = ContainerHelper.Container.Resolve<CryptoViewModel>();
            _cryptoListViewModel = ContainerHelper.Container.Resolve<CryptoListViewModel>();
            _filterCryptoViewModel = ContainerHelper.Container.Resolve<FilterCryptoViewModel>();
            _trackCryptoViewModel = ContainerHelper.Container.Resolve<TrackCryptoViewModel>();

            CurrentViewModel = _cryptoListViewModel;


        }
        public override void InitializeCommands()
        {
            NavigateCommand = new RelayCommand<string>(OnNavigate);
        }

        //Child Event Subscription

        private void ViewModelErrorOccured(object arg1, ViewModelErrorEventArgs arg2)
        {
            _errorViewModel.ErrorMessage = arg2.ErrorMessage;
            _errorViewModel.ViewModel = arg1.GetType().Name.Replace("ViewModel", "");

            CurrentViewModel = _errorViewModel;
        }

        private void OnTrackerViewModelConditionMet(object arg1, Data.Services.Tracker.Data.ConditionMetEventArgs arg2)
        {
            CreatePopupWindow(arg2.Crypto, arg2.Condition);
        }

        private async void CreatePopupWindow(string crypto, string condition)
        {
            await Task.Delay(10000);
            SystemSounds.Beep.Play();

            Application.Current.Dispatcher.Invoke((Action)delegate {

                var notificationWindow = new NotificationWindow(crypto, condition);
                notificationWindow.Show();

            });

        }

        private async void OnFiltersApplied(object sender, Markets.Data.ApplyFiltersEventArgs args)
        {
            await _cryptoListViewModel.FilterCoins(args.Request);
            OnNavigate("all");
        }

        private void OnGetCoin(object sender, Markets.Data.GetCoinEventArgs args)
        {
            _cryptoViewModel.SelectedCoinString = args.CoinName;
            _cryptoViewModel.LoadAsyncData();

            OnNavigate("single");
        }



        public object CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }

            set
            {
                _currentViewModel = value;
                RaisePropertyChanged();
            }
        }

        private CryptoListViewModel _cryptoListViewModel;
        private FilterCryptoViewModel _filterCryptoViewModel;
        private TrackCryptoViewModel _trackCryptoViewModel;
        private CryptoViewModel _cryptoViewModel;
        private MainSettingsViewModel _mainSettingsViewModel;
        private ErrorViewModel _errorViewModel;
        private TrackerPopupViewModel _trackerPopupViewModel;
        private object _currentViewModel;



        #endregion

        #region Functions

        //Navigation

        private void OnNavigate(string targetViewModel)
        {

            //Navigate according to buttons clicked
            switch (targetViewModel)
            {
                case "all":
                    CurrentViewModel = _cryptoListViewModel;
                    break;
                case "single":
                    CurrentViewModel = _cryptoViewModel;
                    break;
                case "filter":
                    CurrentViewModel = _filterCryptoViewModel;
                    break;
                case "track":
                    CurrentViewModel = _trackCryptoViewModel;
                    break;
                case "settings":
                    CurrentViewModel = _mainSettingsViewModel;
                    break;



            }

        }

        public RelayCommand<string> NavigateCommand { get; private set; }

        #endregion







       
    }
}
