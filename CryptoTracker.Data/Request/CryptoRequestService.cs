using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CryptoTracker.Data.Request
{
    public class CryptoRequestService
    {
        /// <summary>
        /// Creates dynamic string queries for filtering BasicCryptoModels
        /// The c# objects are converted into strings to be used by the System.Linq.Dynamic library
        /// </summary>

        public CryptoRequestService()
        {
            _filterList = new List<string>();
        }

        public static List<string> ParseParameters(List<CryptoRequestParameters> parametersList)
        {
            var service = new CryptoRequestService();
            foreach (var requestParameter in parametersList)
            {
                service.AddFilter(requestParameter);
            }

            return service.GetFilters();
        }

        public static string ParseParameter(CryptoRequestParameters parameters)
        {
            var service = new CryptoRequestService();

            service.AddFilter(parameters);

            return service.GetFilters()
                          .First();
        }

        public void AddFilter(CryptoRequestParameters requestParameters)
        {


            string filterOperator;

            switch (requestParameters.Type)
            {
                case RequestFilterType.Maximum:
                    filterOperator = "<=";
                    break;
                case RequestFilterType.Minimum:
                    filterOperator = ">=";
                    break;
                default:
                    filterOperator = "<=";
                    break;

            }

            var builder = new StringBuilder();
            builder.Append(requestParameters.Property.ToString());
            builder.Append(filterOperator);
            builder.Append(requestParameters.Value.ToString());

            _filterList.Add(builder.ToString());


        }

        public void RemoveFilter(string filter)
        {
            var filterForDelete = _filterList.Single(f => f.Contains(filter));
            _filterList.Remove(filterForDelete);
        }

        public List<string> GetFilters()
        {
            if (_filterList == null) throw new Exception();

            return _filterList;
        }

        private List<string> _filterList;

 
       
















    }
}
