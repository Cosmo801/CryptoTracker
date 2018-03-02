
using CryptoTracker.Data.Models.Tracker;
using CryptoTracker.Data.Request;
using System;
using System.Collections.Generic;

namespace CryptoTracker.Data.Helpers
{
    public static class CryptoModelConverter
    {

        /// <summary>
        /// Convert UI elements into serializable elements
        /// </summary>


        public static SerializedCryptoModel GetSerializedModel(CryptoDataModel dataModel)
        {
            List<Dictionary<string, string>> conditions = new List<Dictionary<string, string>>();

            foreach(var condition in dataModel.Conditions)
            {
                conditions.Add(new Dictionary<string, string>
                {
                    {"Property", condition.Property.ToString() },
                    {"Type" , condition.Type.ToString() },
                    {"Value", condition.Value.ToString() }
                });
            }


            return new SerializedCryptoModel
            {
                Conditions = conditions,
                Symbol = dataModel.Data.Symbol
            };
        }

       
        public static List<CryptoRequestParameters> GetUnserializedRequestParameters(SerializedCryptoModel serializedCrypto)
        {
            var cryptoRequestParameters = new List<CryptoRequestParameters>();

            foreach(var unserializedParameters in serializedCrypto.Conditions)
            {

                cryptoRequestParameters.Add(new CryptoRequestParameters
                {
                    Property = (RequestPropertyType)Enum.Parse(typeof(RequestPropertyType), (unserializedParameters["Property"])),
                    Type = (RequestFilterType)Enum.Parse(typeof(RequestFilterType), unserializedParameters["Type"]),
                    Value = decimal.Parse(unserializedParameters["Value"])


                });
            }

            return cryptoRequestParameters;


        }

       
    }
}
