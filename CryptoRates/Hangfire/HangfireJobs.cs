using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;
using CryptoRates.DeserializingJSON;
using System.Text.Json;
using CryptoRates.Data;

namespace CryptoRates.Hangfire
{
    public class HangfireJobs
    {
        private CryptoContext _context;
        public HangfireJobs(CryptoContext context)
        {
            _context = context;
        }
        public async Task GetAllCoins()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(@"https://min-api.cryptocompare.com/data/all/coinlist");
            string jsonString = await response.Content.ReadAsStringAsync();
            AllCurrencies allCurrencies = JsonSerializer.Deserialize<AllCurrencies>(jsonString);
            foreach (KeyValuePair<string, DeserializingJSON.Coin> pair in allCurrencies.Data)
            {
                Currency currency = new Currency()
                {
                    Name = pair.Value.Name,
                    Symbol = pair.Value.Symbol,
                    ValueUSD = 0,
                    WebPage = allCurrencies.BaseLinkUrl + pair.Value.Url,
                    ImageURL = allCurrencies.BaseImageUrl + pair.Value.ImageUrl
                };
                _context.Currencies.Add(currency);
            }
            _context.SaveChanges();
        }
    }
}
