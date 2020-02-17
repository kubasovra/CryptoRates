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

        //Temporary, later this functional will be moved to Hangfire
        [HttpGet]
        public async Task<IActionResult> All()
        {
            return View();
        }
    }
}