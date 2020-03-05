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
using Hangfire.Server;
using Hangfire;

namespace CryptoRates.Hangfire
{
    public class HangfireJobs
    {
        private readonly string _apiKey;
        private readonly CryptoContext _context;
        private readonly ILogger<HangfireJobs> _logger;
        private readonly ILogger<EmailSender> _emailLogger;
        private readonly IConfiguration _configuration;

        //The historical data is minute wise, whereas current price updates every 10 seconds. This counter is used to sync 
        //them - historical updates every sixth getting current price
        private int HistoricalDataCounter = 0;

        public HangfireJobs(CryptoContext context, ILogger<HangfireJobs> logger, ILogger<EmailSender> emailLogger, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _emailLogger = emailLogger;
            _configuration = configuration;
            _apiKey = @"&api_key=" + configuration.GetSection("Cryptocompare.com")["ApiKey"];
        }
        //Updates the coin list, adding those who are not there yet
        public async Task GetAllCoins()
        {
            string baseImageAndLinkUrl = "https://www.cryptocompare.com";
            string jsonResponse = await SendRequest(@"https://min-api.cryptocompare.com/data/top/mktcapfull?limit=100&tsym=USD" + _apiKey);
            CurrenciesToplist topList = JsonSerializer.Deserialize<CurrenciesToplist>(jsonResponse);
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
            List<Pair> allPairs = await _context.Pairs.Include(p => p.User).Include(p => p.FirstCurrency).Include(p => p.SecondCurrency).ToListAsync();
            if (allPairs != null)
            {
                string baseUrl = @"https://min-api.cryptocompare.com/data/pricemulti";
                List<string> fsymsArray = new List<string>();
                string tsymsParam = @"&tsyms=USD";
                fsymsArray = ExtractSymbols(allPairs).ToList();
                string fsymsParam = @"?fsyms=";
                string jsonResponse;
                //API can handle a request with a maximum of 300 currency symbols at a time, so
                //if you have more than 300 - it will make multiple requests
                for (int i = 0; i < fsymsArray.Count; i += 300)
                {
                    fsymsParam += string.Join(',', fsymsArray.Skip(i).Take(300));
                    string request = baseUrl + fsymsParam + tsymsParam + _apiKey;
                    jsonResponse = await SendRequest(request);
                    fsymsParam = @"?fsyms=";
                    Dictionary<string, CurrencyValueUSD> currenciesPrices = JsonSerializer.Deserialize<Dictionary<string, CurrencyValueUSD>>(jsonResponse);
                    foreach (KeyValuePair<string, DeserializingJSON.CurrencyValueUSD> pair in currenciesPrices)
                    {
                        Currency currencyUpdate = _context.Currencies.FirstOrDefault(c => c.Symbol == pair.Key);
                        if (currencyUpdate != null)
                        {
                            currencyUpdate.ValueUSD = pair.Value.USD;
                        }
                    }
                    await UpdatePairsPrices(allPairs);
                }
                CheckPairsPrice(allPairs);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }

        [AutomaticRetry(Attempts = 10, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public async Task SendEmails(PerformContext hangfireContext)
        {
            List<Pair> allPairs = await _context.Pairs.Include(p => p.User).Include(p => p.FirstCurrency).Include(p => p.SecondCurrency).ToListAsync();

            foreach(Pair pair in allPairs.Where(p => p.State == PairStates.Pending))
            {
                EmailSender emailSender = new EmailSender(_emailLogger, _configuration);

                bool emailResult = false;
                try
                {
                    emailResult = await emailSender.SendEmail(
                        pair.User.Email,
                        string.Format("{0}/{1} crossed {2}", pair.FirstCurrency.Symbol, pair.SecondCurrency.Symbol, pair.TargetPrice),
                        string.Format("{0}/{1} price is now {2}, last known price was {3}", pair.FirstCurrency.Symbol, pair.SecondCurrency.Symbol, pair.PriceFirstToSecond, pair.PreviousPriceFirstToSecond)
                        );
                }
                catch
                {
                    throw;
                }
                finally
                {
                    //If it is the last attempt => mark pair as failed
                    if (hangfireContext.GetJobParameter<int>("RetryCount") == 10)
                    {
                        pair.State = PairStates.Failed;
                    }
                }
                pair.State = emailResult ? PairStates.Notified : PairStates.Failed;
                await _context.SaveChangesAsync();
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
                        _logger.LogInformation("Oink");
                        pair.State = PairStates.Pending;
                    }
                }
            }
        }

        private async Task UpdatePairsPrices(List<Pair> pairs)
        {
            foreach(Pair pair in pairs)
            {
                //If it is the first price record for this pair, that is there is no previous price, 
                //then the previous price will be set equal to the current price
                if (pair.HistoricalData == null || pair.HistoricalData.Count == 0)
                {
                    pair.HistoricalData = await GetPastDayPriceData(pair.FirstCurrency, pair.SecondCurrency);
                }

                if (pair.PriceFirstToSecond == 0)
                {
                    pair.PreviousPriceFirstToSecond = pair.FirstCurrency.ValueUSD / pair.SecondCurrency.ValueUSD;
                }
                else
                {
                    pair.PreviousPriceFirstToSecond = pair.PriceFirstToSecond;
                }

                pair.PriceFirstToSecond = pair.FirstCurrency.ValueUSD / pair.SecondCurrency.ValueUSD;

                if (HistoricalDataCounter == 6)
                {
                    pair.HistoricalData.RemoveAt(0);
                    pair.HistoricalData.Add(pair.PriceFirstToSecond.ToString());
                    HistoricalDataCounter = 0;
                }
                else
                {
                    HistoricalDataCounter++;
                }
            }
        }

        private async Task<List<string>> GetPastDayPriceData(Currency firstCurrency, Currency secondCurrency)
        {
            string request = string.Format(@"https://min-api.cryptocompare.com/data/v2/histominute?fsym={0}&tsym={1}&limit=1440" + _apiKey, firstCurrency.Symbol, secondCurrency.Symbol);
            string jsonResponse = await SendRequest(request);
            PairHistoricalData historicalData = JsonSerializer.Deserialize<PairHistoricalData>(jsonResponse);

            List<string> result = new List<string>();
            foreach(PriceData priceData in historicalData.Data.Data)
            {
                result.Add(priceData.close.ToString());
            }

            return result;
        }

        //Extracts all unique symbols from requested pairs
        private HashSet<string> ExtractSymbols(List<Pair> pairs)
        {
            HashSet<string> symbols = new HashSet<string>();
            foreach (Pair pair in pairs)
            {
                symbols.Add(pair.FirstCurrency.Symbol);
                symbols.Add(pair.SecondCurrency.Symbol);
            }
            return symbols;
        }
    }
}
