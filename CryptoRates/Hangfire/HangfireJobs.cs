using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;
using CryptoRates.DeserializingJSON;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using CryptoRates.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace CryptoRates.Hangfire
{
    public class HangfireJobs
    {
        private readonly CryptoContext _context;
        private readonly ILogger<HangfireJobs> _logger;
        private readonly ILogger<EmailSender> _emailLogger;
        private readonly IConfiguration _configuration;
        public HangfireJobs(CryptoContext context, ILogger<HangfireJobs> logger, ILogger<EmailSender> emailLogger, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _emailLogger = emailLogger;
            _configuration = configuration;
        }
        //Updates the coin list, adding those who are not there yet
        public async Task GetAllCoins()
        {
            string baseImageAndLinkUrl = "https://www.cryptocompare.com";
            string jsonString = await SendRequest(@"https://min-api.cryptocompare.com/data/top/mktcapfull?limit=100&tsym=USD");
            CurrenciesToplist topList = JsonSerializer.Deserialize<CurrenciesToplist>(jsonString);
            foreach (Datum datum in topList.Data)
            {
                Currency currency = new Currency()
                {
                    Name = datum.CoinInfo.FullName,
                    Symbol = datum.CoinInfo.Name,
                    ValueUSD = 0,
                    WebPage = baseImageAndLinkUrl + datum.CoinInfo.Url,
                    ImageURL = baseImageAndLinkUrl + datum.CoinInfo.ImageUrl
                };
                if (_context.Currencies.FirstOrDefault(c => c.Name == datum.CoinInfo.FullName) == null)
                {
                    _context.Currencies.Add(currency);
                }
            }

            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }

        public async Task UpdateCoinsPrices()
        {
            List<Pair> allPairs = _context.Pairs.Include(p => p.User).Include(p => p.FirstCurrency).Include(p => p.SecondCurrency).ToList();
            if (allPairs != null)
            {
                string baseUrl = @"https://min-api.cryptocompare.com/data/pricemulti";
                List<string> fsymsArray = new List<string>();
                string tsymsParam = @"&tsyms=USD";
                string apiKeyParam = @"&api_key=dc1e5dc77434fb29d0bed5f39defab436aed74a01b6491479842826a12433a55";
                int symbolAmount = ExtractSymbols(allPairs, fsymsArray);
                string fsymsParam = @"?fsyms=";
                string jsonString;
                //API can handle a request with a maximum of 300 currency symbols at a time, so
                //if you have more than 300 - it will make multiple requests
                for (int i = 0; i < symbolAmount; i++)
                {
                    fsymsParam += fsymsArray[i] + ',';
                    if (i == 300 || i == symbolAmount - 1)
                    {
                        //Removes a ',' from the very end of the param string
                        fsymsParam.Remove(fsymsParam.Length - 1);
                        string request = baseUrl + fsymsParam + tsymsParam + apiKeyParam;
                        jsonString = await SendRequest(request);
                        fsymsParam = @"?fsyms=";
                        Dictionary<string, CurrencyValueUSD> currenciesPrices = JsonSerializer.Deserialize<Dictionary<string, CurrencyValueUSD>>(jsonString);
                        foreach (KeyValuePair<string, DeserializingJSON.CurrencyValueUSD> pair in currenciesPrices)
                        {
                            Currency currencyUpdate = _context.Currencies.FirstOrDefault(c => c.Symbol == pair.Key);
                            if (currencyUpdate != null)
                            {
                                currencyUpdate.ValueUSD = pair.Value.USD;
                            }
                        }
                        UpdatePairsPrices(allPairs);
                    }
                }
                CheckPairsPrice(allPairs);
            }

            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }

        private async Task<string> SendRequest(string request)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(request);
            return await response.Content.ReadAsStringAsync();
        }

        private void CheckPairsPrice(List<Pair> pairs)
        {
            foreach (Pair pair in pairs)
            {
                if (pair.IsNotifyOnPrice)
                {
                    if ((pair.PreviousPriceFirstToSecond > pair.TargetPrice && pair.TargetPrice > pair.PriceFirstToSecond) || (pair.PreviousPriceFirstToSecond < pair.TargetPrice && pair.TargetPrice < pair.PriceFirstToSecond))
                    {
                        EmailSender emailSender = new EmailSender(_emailLogger, _configuration);
                        emailSender.SendEmail(
                            pair.User.Email,
                            string.Format("{0}/{1} crossed {2}", pair.FirstCurrency.Symbol, pair.SecondCurrency.Symbol, pair.TargetPrice),
                            string.Format("{0}/{1} price is now {2}, last known price was {3}", pair.FirstCurrency.Symbol, pair.SecondCurrency.Symbol, pair.PriceFirstToSecond, pair.PreviousPriceFirstToSecond)
                            );
                    }
                }
            }
        }
        private void UpdatePairsPrices(List<Pair> pairs)
        {
            foreach(Pair pair in pairs)
            {
                //If it is the first price record for this pair, that is there is no previous price, 
                //then the previous price will be set equal to the current price
                if (pair.PriceFirstToSecond == 0)
                {
                    pair.PreviousPriceFirstToSecond = pair.FirstCurrency.ValueUSD / pair.SecondCurrency.ValueUSD;
                }
                else
                {
                    pair.PreviousPriceFirstToSecond = pair.PriceFirstToSecond;
                }
                pair.PriceFirstToSecond = pair.FirstCurrency.ValueUSD / pair.SecondCurrency.ValueUSD;
            }
        }

        //Extracts all unique symbols from requested pairs
        private int ExtractSymbols(List<Pair> pairs, List<string> fsymsArray)
        {
            int symbolCounter = 0;
            foreach (Pair pair in pairs)
            {
                if (!fsymsArray.Contains(pair.FirstCurrency.Symbol))
                {
                    fsymsArray.Add(pair.FirstCurrency.Symbol);
                    symbolCounter++;
                }
                if (!fsymsArray.Contains(pair.SecondCurrency.Symbol))
                {
                    fsymsArray.Add(pair.SecondCurrency.Symbol);
                    symbolCounter++;
                }
            }
            return symbolCounter;
        }
    }
}
