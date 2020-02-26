using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoRates.Data.DTO
{
    public class PairDTO
    {
        public PairDTO() { }
        public PairDTO(int pairId, string userId)
        {
            PairId = pairId;
            UserId = userId;
        }
        public int PairId { get; private set; }
        public string UserId { get; private set; }
        public string FirstCurrencyName { get; set; }
        public string FirstCurrencySymbol { get; set; }
        public string FirstCurrencyImageUrl { get; set; }
        public string FirstCurrencyPageUrl { get; set; }
        public string SecondCurrencyName { get; set; }
        public string SecondCurrencySymbol { get; set; }
        public string SecondCurrencyImageUrl { get; set; }
        public string SecondCurrencyPageUrl { get; set; }
        public double PriceFirstToSecond { get; set; }
        public double PreviousPriceFirstToSecond { get; set; }
        public double TargetPrice { get; set; }
        public double TargetPriceAbsoluteChange { get; set; }
        public double TargetPricePercentChange { get; set; }
        public bool IsNotifyOnPrice { get; set; }
        public bool IsNotifyOnAbsoluteChange { get; set; }
        public bool IsNotifyOnPercentChange { get; set; }
    }
}
