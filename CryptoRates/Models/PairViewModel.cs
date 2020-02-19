using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoRates.Models
{
    public class PairViewModel
    {
        public PairViewModel(int pairId)
        {
            PairId = pairId;
        }
        public int PairId { get; private set; }
        public string FirstCurrencyName { get; set; }
        public string FirstCurrencySymbol { get; set; }
        public string FirstCurrencyImageURL { get; set; }
        public string SecondCurrencyName { get; set; }
        public string SecondCurrencySymbol { get; set; }
        public string SecondCurrencyImageURL { get; set; }
        public double PriceFirstToSecond { get; set; }
        public double TargetPrice { get; set; }
        public double TargetPriceAbsoluteChange { get; set; }
        public double TargetPricePercentChange { get; set; }
    }
}
