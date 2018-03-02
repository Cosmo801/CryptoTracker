using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoTracker.Data.DTOs.CoinMarketCap;
using CryptoTracker.Data.Request;
using CryptoTracker.Data.Helpers;
using System.Net.Http;
using Newtonsoft.Json;
using System.Linq.Dynamic;
using CryptoTracker.Data.Errors;
using CryptoTracker.Data.Models;

namespace CryptoTracker.Data.Services.CoinMarketCap
{
    public class CoinMarketCapService : ICoinMarketCapService
    {
        private HttpClient _client;

        public CoinMarketCapService()
        {
            _client = ClientHelper.GetClient(ClientHelper.CoinMarketCapBase);
        }

        public async Task<List<BasicCryptoModel>> GetAllCoins()
        {          

            try
            {
                //Create request
                HttpResponseMessage response = await _client.GetAsync("ticker/?limit=0").ConfigureAwait(false);
                var parsedCryptoList = new List<BasicCryptoModel>();

                //If api returns error throw this error up the stack to be handled
                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError) throw new CryptoServiceException("Coin Market Cap API is down");

                //Parse the response json into c# objects
                var contentJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                contentJson = contentJson.Replace("null", "0");
                var content = JsonConvert.DeserializeObject<List<CMCAllCoinResponse>>(contentJson);

                for (int i = 0; i < content.Count; i++)
                {
                    parsedCryptoList.Add(new BasicCryptoModel
                    {
                        Symbol = content[i].Symbol,
                        Name = content[i].Name,
                        BTCPrice = content[i].BTCPrice,
                        USDPrice = content[i].USDPrice,
                        MarketCap = content[i].MarketCapUSD,
                        CirculatingSupply = content[i].AvailableSupply,
                        TotalSupply = content[i].TotalSupply,
                        Change24h = content[i].Change24H,
                        Volume24h = content[i].Volume24HUSD
                    });
                }

                return parsedCryptoList;
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

        public async Task<BasicCryptoModel> GetCoin(string coinName)
        {
            
            try
            {
                HttpResponseMessage response = await _client.GetAsync($"ticker/{coinName}").ConfigureAwait(false);
                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError) throw new CryptoServiceException("Coin Market Cap API is down");

                var contentJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);


                var content = JsonConvert.DeserializeObject<List<CMCSingleCoinResponse>>(contentJson);
                var unparsedCrypto = content.First();


                return new BasicCryptoModel
                {
                    Symbol = unparsedCrypto.Symbol,
                    Name = unparsedCrypto.Name,
                    BTCPrice = unparsedCrypto.BTCPrice,
                    USDPrice = unparsedCrypto.USDPrice,
                    MarketCap = unparsedCrypto.MarketCapUSD,
                    CirculatingSupply = unparsedCrypto.AvailableSupply,
                    TotalSupply = unparsedCrypto.TotalSupply,
                    Change24h = unparsedCrypto.Change24H,
                    Volume24h = unparsedCrypto.Volume24HUSD
                };


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

        public async Task<List<BasicCryptoModel>> GetFilteredCoins(CryptoRequestService requestParameters)
        {
            try
            {
                IEnumerable<BasicCryptoModel> cryptoList = await GetAllCoins().ConfigureAwait(false);
                var filterStrings = requestParameters.GetFilters();

                foreach (var filter in filterStrings)
                {
                    cryptoList = cryptoList.Where(filter);
                }

                return cryptoList.ToList();

            }

            catch (Exception)
            {
                throw new CryptoServiceException("Download failed, service may be down or you do not have a stable internet connection");
            }



        }


    }

        
}
