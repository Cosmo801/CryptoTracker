using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoTracker.Data.Models;
using CryptoTracker.Data.Helpers;
using System.Net.Http;
using Newtonsoft.Json;
using CryptoTracker.Data.Errors;
using System.Linq;

namespace CryptoTracker.Data.Services.CryptoCompare
{
    public class CryptoCompareService : ICryptoCompareService
    {
        private HttpClient _client;

        public CryptoCompareService()
        {
            _client = ClientHelper.GetClient(ClientHelper.CryptoCompareBase);
        }

        public async Task<List<string>> GetAvailableCrypto()
        {
            HttpResponseMessage response;
            List<string> symbolCollection = new List<string>();

            try
            {

                //Make request
                var client = new HttpClient();
                client.BaseAddress = new Uri("https://www.cryptocompare.com/api/data/coinlist/");

                response = await client.GetAsync("").ConfigureAwait(false);

                //Parse json into c# objects
                var contentJson = await response.Content.ReadAsStringAsync();

                var transferDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(contentJson);
                var dataDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(transferDictionary["Data"].ToString());
                
                foreach(var entry in dataDictionary)
                {
                    symbolCollection.Add(entry.Key);
                }

                return symbolCollection;

            }
            catch (Exception)
            {
                return symbolCollection;
                throw new CryptoServiceException("Download failed, service may be down or you do not have a stable internet connection");
            }




        }
        public async Task<AdvancedCryptoModel> GetCrypto(string crypto)
        {
            HttpResponseMessage singleResponse;
            HttpResponseMessage listResponse;

            try
            {
                listResponse = await _client.GetAsync("all/coinlist").ConfigureAwait(false);
                var listContent = await listResponse.Content.ReadAsStringAsync().ConfigureAwait(false);



                var unparsedList = JsonConvert.DeserializeObject<Dictionary<string, object>>(listContent);
                var data = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string,string>>>(unparsedList["Data"].ToString());
                var image = "https://www.cryptocompare.com" + data[$"{crypto}"]["ImageUrl"];

                singleResponse = await _client.GetAsync($"pricemultifull?fsyms={crypto}&tsyms=BTC,USD").ConfigureAwait(false);

                var content = await singleResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                var unparsedCrypto = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, string>>>>>(content);

                var unparsedBitcoinData = unparsedCrypto["RAW"][$"{crypto}"]["BTC"];
                var unparsedUSDData = unparsedCrypto["RAW"][$"{crypto}"]["USD"];



                return new AdvancedCryptoModel
                {
                    Symbol = crypto,
                    Name = crypto,
                    BTCPrice = Convert.ToDecimal(unparsedBitcoinData["PRICE"]),
                    USDPrice = Convert.ToDecimal(unparsedUSDData["PRICE"]),
                    MarketCap = Convert.ToDecimal(unparsedUSDData["MKTCAP"]),
                    Change24h = Convert.ToDecimal(unparsedUSDData["CHANGEPCT24HOUR"]),
                    Volume24h = Convert.ToDecimal(unparsedUSDData["VOLUME24HOUR"]),
                    CirculatingSupply = Convert.ToDecimal(unparsedUSDData["SUPPLY"]),
                    TotalSupply = Convert.ToDecimal(unparsedUSDData["SUPPLY"]),
                    Open = Convert.ToDecimal(unparsedUSDData["OPEN24HOUR"]),
                    High = Convert.ToDecimal(unparsedUSDData["HIGH24HOUR"]),
                    Low = Convert.ToDecimal(unparsedUSDData["LOW24HOUR"]),
                    ImageUrl = image


                };



            }
            catch (Exception)
            {
                throw new CryptoServiceException("Download failed, service may be down or you do not have a stable internet connection");
            }
        }
        public async Task<BasicCryptoModel> GetBasicCrypto(string crypto)
        {
            HttpResponseMessage singleResponse;

            try
            {
                singleResponse = await _client.GetAsync($"pricemultifull?fsyms={crypto}&tsyms=BTC,USD").ConfigureAwait(false);

                var content = await singleResponse.Content.ReadAsStringAsync().ConfigureAwait(false);


                return ParseData(content, crypto);

              
                

              



            }
            catch (JsonSerializationException)
            {
                throw new CryptoServiceException("API returned error");
            }


            catch (Exception)
            {
                throw new CryptoServiceException("Download failed, service may be down or you do not have a stable internet connection");
            }
        }

        private BasicCryptoModel ParseData(string cryptoJson, string symbol)
        {
            var unparsedCrypto = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, string>>>>>(cryptoJson);

            var unparsedBitcoinData = unparsedCrypto["RAW"][$"{symbol}"]["BTC"];
            var unparsedUSDData = unparsedCrypto["RAW"][$"{symbol}"]["USD"];

            var btcPrice = ParsePrice(unparsedBitcoinData["PRICE"]);
            var usdPrice = ParsePrice(unparsedUSDData["PRICE"]);
            var marketCap = ParsePrice(unparsedUSDData["MKTCAP"]);
            var change24h = ParsePrice(unparsedUSDData["CHANGEPCT24HOUR"]);
            var volume24h = ParsePrice(unparsedUSDData["VOLUME24HOUR"]);
            var circulatingSupply = ParsePrice(unparsedUSDData["SUPPLY"]);
            var totalSupply = ParsePrice(unparsedUSDData["SUPPLY"]);




            return new BasicCryptoModel
            {
                Symbol = symbol,
                Name = symbol,
                BTCPrice = btcPrice,
                USDPrice = usdPrice,
                MarketCap = marketCap, 
                Change24h = change24h,
                Volume24h = volume24h,
                CirculatingSupply = circulatingSupply,
                TotalSupply = totalSupply


            };

        }

        private decimal ParsePrice(string price)
        {
            if (price.Contains("e-"))
            {
                var split = price.Split('e');
                var tensString = split[1].Replace("-", string.Empty);

                var number = Convert.ToDecimal(split[0]);
                var tens = Convert.ToInt32(tensString);

                var fullNumber = number * (10 * tens);
                return fullNumber;
                
            }

            return Convert.ToDecimal(price);
        

            
        }

       
    }
}
