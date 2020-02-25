using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoRates.Data.DTO
{
    public class CurrencyDTO
    {
        public CurrencyDTO(int id, string name, string symbol, double value, string webPage, string image)
        {
            CurrencyId = id;
            Name = name;
            Symbol = symbol;
            ValueUSD = value;
            WebPage = webPage;
            ImageURL = image;
        }
        public int CurrencyId { get; private set; }
        public string Name { get; private set; }
        public string Symbol { get; private set; }
        public double ValueUSD { get; private set; }
        public string WebPage { get; private set; }
        public string ImageURL { get; private set; }
    }
}
