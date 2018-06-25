# CryptoTracker

Sample WPF MVVM Application.

This application download cryptocurrency data and displays it to the users in datagrids. The user can choose to 'track' a cryptocurrency which is added to a file. When certain parameters that the users selects such as price > $100 the user is notified.

Points of interest

CryptoTracker.CryptoTracker.Data.Services - The tracker service and the data services that download data.

CryptoTracker.CryptoTracker.Data.Helper.ContinuousTaskFactory - Async support for a task. This is used in the application to continuously download the data for the tracked cryptocurrencies so the user can be notified when the parameters are reached.

CryptoTracker.CryptoTracker.WPF.MVVM - How MVVM and view navigation is implemented.

CryptoTracker.CryptoTracker.WPF.Markets - An example view model that uses the services to download cryptocurrency data 
