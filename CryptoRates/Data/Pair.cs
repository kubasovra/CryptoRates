using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CryptoRates.Data
{
    public class Pair
    {
        public int PairId { get; set; }
        public IdentityUser User { get; set; }
        public Currency FirstCurrency { get; set; }
        public Currency SecondCurrency { get; set; }
        public double PriceFirstToSecond { get; set; }
        public double TargetPrice { get; set; }
        public double TargetPriceAbsoluteChange { get; set; }
        public double TargetPricePercentChange { get; set; }
        public bool IsNotifyOnPrice { get; set; }
        public bool IsNotifyOnAbsoluteChange { get; set; }
        public bool IsNotifyOnPercentChange { get; set; }
    }
}
