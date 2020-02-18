using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoRates.DeserializingJSON
{

    public class CurrenciesPrices
    {
        public Dictionary<string, ValueUSD> Currencies { get; set; }
    }

    public class ValueUSD
    {
        public double USD { get; set; }
    }

}
