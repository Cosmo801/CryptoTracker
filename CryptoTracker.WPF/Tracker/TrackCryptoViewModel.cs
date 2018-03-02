using CryptoTracker.Data.Errors;
using CryptoTracker.Data.Models;
using CryptoTracker.Data.Models.Tracker;
using CryptoTracker.Data.Services.Tracker;
using CryptoTracker.Data.Services.Tracker.Data;
using CryptoTracker.WPF.MVVM;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Unity;
using System.Threading.Tasks;
using CryptoTracker.Data.Request;

namespace CryptoTracker.WPF.Tracker
{
    public class TrackCryptoViewModel : CryptoViewModelBase
    {
        #region Initialization

        public TrackCryptoViewModel()
        {
            _trackerService = new DebugTrackerService();

            EditTrackerViewModel = ContainerHelper.Container.Resolve<AddToTrackerViewModel>();
            EditTrackerOpen = false;

            EditTrackerViewModel.AppliedToTracker += EditTrackerViewModel_AppliedToTracker;
            _trackerService.ConditionMet += _trackerService_ConditionMet;

            InitializeCommands();
            LoadData();

        }

        public override void InitializeCommands()
        {
            OpenEditTrackerCommand = new RelayCommand(OnAddCrypto);
            RemoveCryptoCommand = new RelayCommand(OnRemoveCrypto, CanRemoveCrypto);
            CloseEditTrackerCommand = new RelayCommand(OnCloseEditTracker);
        }

        public async override void LoadData()
        {
            try
            {
                LoadCryptoTask = new TaskWatcher<List<CryptoDataModel>>(_trackerService.LoadCryptoDataModels());

                LoadCryptoTask.PropertyChanged += LoadCryptoCompleted;
            }
            catch (CryptoServiceException ex)
            {
                RaiseErrorOccured(ex.Message);
                await Task.Delay(20000).ConfigureAwait(false);
                LoadData();
            }



        }

        private void LoadCryptoCompleted(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Result")
            {
                CryptoDataList = LoadCryptoTask.Result;

            }

            else
            {
                return;
            }
        }

        public TaskWatcher<List<CryptoDataModel>> LoadCryptoTask { get; private set; }
        private DebugTrackerService _trackerService;

        public AddToTrackerViewModel EditTrackerViewModel
        {
            get
            {
                return _editTrackerViewModel;
            }
            set
            {
                _editTrackerViewModel = value;
                RaisePropertyChanged();
            }
        }
        private AddToTrackerViewModel _editTrackerViewModel;

        public List<CryptoDataModel> CryptoDataList
        {
            get
            {
                return _cryptoDataList;
            }
            set
            {
                _cryptoDataList = value;
                ObservableCrypto = new ObservableCollection<BasicCryptoModel>(value.Select(c => c.Data));
                RaisePropertyChanged();
            }
        }
        private List<CryptoDataModel> _cryptoDataList;

        public ObservableCollection<BasicCryptoModel> ObservableCrypto
        {
            get
            {
                return _observableCrypto;
            }
            set
            {
                _observableCrypto = value;
                RemoveCryptoCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged();
            }
        }
        private ObservableCollection<BasicCryptoModel> _observableCrypto;

        public BasicCryptoModel SelectedObservableCrypto
        {
            get
            {
                return _selectedObservableCrypto;
            }
            set
            {
                _selectedObservableCrypto = value;
                RaisePropertyChanged();

                if (value == null) return;

                RemoveCryptoCommand.RaiseCanExecuteChanged();
                SelectedTracker = CryptoDataList.Where(c => c.Data.Symbol == value.Symbol)
                                                                                 .SelectMany(c => c.Conditions)
                                                                                 .ToList();
            }
        }
        private BasicCryptoModel _selectedObservableCrypto;


        #endregion


        #region Functions

        //Tracker

        private void _trackerService_ConditionMet(object arg1, ConditionMetEventArgs arg2)
        {
            if (TrackerViewModelConditionMet == null) return;
            TrackerViewModelConditionMet(this, arg2);
        }

        public event Action<object, ConditionMetEventArgs> TrackerViewModelConditionMet;

        //Edit Tracker

        private bool CanRemoveCrypto()
        {
            if (SelectedObservableCrypto == null) return false;
            return true;
        }

        private async void OnRemoveCrypto()
        {
            var cryptoForRemove = CryptoDataList.Single(c => c.Data.Symbol == SelectedObservableCrypto.Symbol);
            await _trackerService.RemoveCrypto(cryptoForRemove);
            await _trackerService.SaveChanges();
            ObservableSelectedTracker = null;

            LoadData();
        }

        public RelayCommand RemoveCryptoCommand { get; private set; }

        private async void EditTrackerViewModel_AppliedToTracker(object arg1, Data.ApplyToTrackerEventArgs arg2)
        {
            switch (arg2.IsAdd)
            {
                case true:
                    await _trackerService.AddCrypto(arg2.Crypto);
                    await _trackerService.SaveChanges();
                    break;
                case false:
                    //
                    break;



            }

            LoadData();

        }

        public ObservableCollection<string> ObservableSelectedTracker
        {
            get
            {
                return _observableSelectedTracker;
            }
            set
            {
                _observableSelectedTracker = value;
                RaisePropertyChanged();
            }
        }
        private ObservableCollection<string> _observableSelectedTracker;

        public List<CryptoRequestParameters> SelectedTracker
        {
            get
            {
                return _selectedTracker;
            }
            set
            {
                _selectedTracker = value;
                ObservableSelectedTracker = new ObservableCollection<string>(CryptoRequestService.ParseParameters(value));
            }
        }
        private List<CryptoRequestParameters> _selectedTracker;




        //Edit Tracker menu

        private async void OnAddCrypto()
        {
            EditTrackerOpen = true;
        }

        private void OnCloseEditTracker()
        {
            EditTrackerOpen = false;
        }

        public bool EditTrackerOpen
        {
            get
            {
                return _editTrackerOpen;
            }

            set
            {
                _editTrackerOpen = value;
                RaisePropertyChanged();
            }


        }
        private bool _editTrackerOpen;

        public RelayCommand OpenEditTrackerCommand { get; private set; }
        public RelayCommand CloseEditTrackerCommand { get; private set; }
        

        #endregion









       

       

      

        




      

      
        
       
        


       
        
       
       
        



      
        



        

       

        
    }
}
