using CryptoTracker.Data.Services.CoinMarketCap;
using CryptoTracker.Data.Services.CryptoCompare;
using CryptoTracker.Data.Services.Tracker;
using Unity;
using Unity.Lifetime;

namespace CryptoTracker.WPF.MVVM
{
    public static class ContainerHelper
    {
        /// <summary>
        /// Dependency injection
        /// </summary>


        private static IUnityContainer _container;

        static ContainerHelper()
        {
            _container = new UnityContainer();
            _container.RegisterType<ICoinMarketCapService, CoinMarketCapService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ICryptoCompareService, CryptoCompareService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ITrackerLoader, TrackerFileLoader>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ITrackerPriceService, TrackerPriceService>(new ContainerControlledLifetimeManager());

        }

        public static IUnityContainer Container
        {
            get { return _container; }
        }
    }
}
