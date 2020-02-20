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
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Hangfire;
using CryptoRates.Hangfire;

namespace CryptoRates.Controllers
{
    [Authorize]
    public class PairsController : Controller
    {
        CryptoContext _context;
        UserManager<ApplicationUser> _userManager;
        IBackgroundJobClient _backgroundJob;
        ILogger<HangfireJobs> _hangfireLogger;
        public PairsController(CryptoContext context, UserManager<ApplicationUser> userManager, IBackgroundJobClient backgroundJob, ILogger<HangfireJobs> hangfireLogger)
        {
            _context = context;
            _userManager = userManager;
            _backgroundJob = backgroundJob;
            _hangfireLogger = hangfireLogger;
        }

        [HttpGet]
        public IActionResult MyPairs()
        {
            List<PairViewModel> pairs = new List<PairViewModel>();
            foreach (Pair pair in _context.Pairs.Where(p => _userManager.GetUserId(this.User) == p.User.Id).Include(c => c.FirstCurrency).Include(c => c.SecondCurrency).ToList())
            {
                pairs.Add(new PairViewModel(pair.PairId){ 
                    FirstCurrencyName = pair.FirstCurrency.Name,
                    FirstCurrencySymbol = pair.FirstCurrency.Symbol,
                    FirstCurrencyImageURL = pair.FirstCurrency.ImageURL,
                    SecondCurrencyName = pair.SecondCurrency.Name,
                    SecondCurrencySymbol = pair.SecondCurrency.Symbol,
                    SecondCurrencyImageURL = pair.SecondCurrency.ImageURL,
                    PriceFirstToSecond = pair.PriceFirstToSecond,
                    TargetPrice = pair.TargetPrice,
                    TargetPriceAbsoluteChange = pair.TargetPriceAbsoluteChange,
                    TargetPricePercentChange = pair.TargetPricePercentChange
                });
            }
            return View(pairs);
        }

        [HttpPost]
        public async  Task<IActionResult> AddPair(string firstCoin, string secondCoin, string targetPrice)
        {
            double price = double.Parse(targetPrice, System.Globalization.CultureInfo.InvariantCulture);
            Currency firstCurrency = _context.Currencies.FirstOrDefault(c => c.Name == firstCoin);
            Currency secondCurrency = _context.Currencies.FirstOrDefault(c => c.Name == secondCoin);

            if (firstCurrency == null || secondCurrency == null || targetPrice == null)
            {
                return RedirectToAction("MyPairs");
            }

            Pair newPair = new Pair() {
                User = await _userManager.GetUserAsync(this.User),
                FirstCurrency = firstCurrency,
                SecondCurrency = secondCurrency,
                TargetPrice = price,
                IsNotifyOnPrice = true
            };
            _context.Pairs.Add(newPair);
            _context.SaveChanges();

            HangfireJobs jobs = new HangfireJobs(_context, _hangfireLogger);
            await jobs.UpdateCoinsPrices();

            return RedirectToAction("MyPairs");
        }

        //Temporary
        [HttpPost]
        public IActionResult DeletePair(int pairId)
        {
            Pair pairToDelete = _context.Pairs.Find(pairId);
            _context.Pairs.Remove(pairToDelete);
            _context.SaveChanges();
            return RedirectToAction("MyPairs");
        }
    }
}