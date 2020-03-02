using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoRates.Data.DTO
{
    public class CurrencyDTO
    {
        public CurrencyDTO() { }
        public int CurrencyId { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public double ValueUSD { get; set; }
        public string WebPage { get; set; }
        public string ImageURL { get; set; }
    }
}
