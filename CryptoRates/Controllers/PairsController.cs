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
using CryptoRates.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CryptoRates.Controllers
{
    public class PairsController : Controller
    {
        CryptoContext _context;
        UserManager<IdentityUser> _userManager;
        public PairsController(CryptoContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //You need to log in to make it work (no matter how, either via Google or via local account)
        [HttpGet]
        public IActionResult MyPairs()
        {
            List<PairViewModel> pairs = new List<PairViewModel>();
            foreach (Pair pair in _context.Pairs.Where(p => _userManager.GetUserId(this.User) == p.User.Id).Include(c => c.FirstCurrency).Include(c => c.SecondCurrency).ToList())
            {
                pairs.Add(new PairViewModel(){ 
                    FirstCurrencyName = pair.FirstCurrency.Name,
                    FirstCurrencySymbol = pair.FirstCurrency.Symbol,
                    FirstCurrencyImageURL = pair.FirstCurrency.ImageURL,
                    SecondCurrencyName = pair.SecondCurrency.Name,
                    SecondCurrencySymbol = pair.SecondCurrency.Symbol,
                    SecondCurrencyImageURL = pair.SecondCurrency.ImageURL,
                    PriceFirstToSecond = pair.FirstCurrency.ValueUSD / pair.SecondCurrency.ValueUSD,
                    TargetPrice = pair.TargetPrice,
                    TargetPriceAbsoluteChange = pair.TargetPriceAbsoluteChange,
                    TargetPricePercentChange = pair.TargetPricePercentChange
                });
            }
            return View(pairs);
        }

        [HttpPost]
        public IActionResult AddPair(string firstCoin, string secondCoin, string targetPrice)
        {
            double price = double.Parse(targetPrice, System.Globalization.CultureInfo.InvariantCulture);
            IdentityUser identityUser = _context.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(this.User));
            Currency firstCurrency = _context.Currencies.FirstOrDefault(c => c.Name == firstCoin);
            Currency secondCurrency = _context.Currencies.FirstOrDefault(c => c.Name == secondCoin);
            if (firstCurrency == null || secondCurrency == null || identityUser == null)
            {
                return RedirectToAction("MyPairs");
            }

            Pair newPair = new Pair() {
                User = identityUser,
                FirstCurrency = firstCurrency,
                SecondCurrency = secondCurrency,
                TargetPrice = price,
                IsNotifyOnPrice = true
            };
            _context.Pairs.Add(newPair);
            _context.SaveChanges();

            return RedirectToAction("MyPairs");
        }
    }
}