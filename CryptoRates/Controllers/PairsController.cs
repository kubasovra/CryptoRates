using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using CryptoRates.DeserializingJSON;
using CryptoRates.Data;

namespace CryptoRates.Controllers
{
    public class PairsController : Controller
    {
        CryptoContext _context;
        public PairsController(CryptoContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> All()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(@"https://min-api.cryptocompare.com/data/all/coinlist");
            string jsonString = await response.Content.ReadAsStringAsync();
            AllCurrencies allCurrencies = JsonSerializer.Deserialize<AllCurrencies>(jsonString);
            string regexPatternForCode = @"(?<=\()\w+(?=\))"; // Pattern to extract code from FullName: "Bitcoin (BTC)" => "BTC"
            foreach (KeyValuePair<string, DeserializingJSON.Coin> pair in allCurrencies.Data)
            {
                Currency currency = new Currency()
                {
                    Name = pair.Value.Name,
                    AlphaCode = Regex.Match(pair.Value.FullName, regexPatternForCode).Value,
                    Value_USD = 0,
                    WebPage = pair.Value.Url,
                    ImageURL = pair.Value.ImageUrl
                };
                _context.Currencies.Add(currency);
            }
            _context.SaveChanges();
            return View();
        }
    }
}