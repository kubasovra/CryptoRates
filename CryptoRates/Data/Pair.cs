using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CryptoRates.Models;

namespace CryptoRates.Data
{
    public class Pair
    {
        [Key]
        public int PairId { get; set; }
        public ApplicationUser User { get; set; }
        public Currency FirstCurrency { get; set; }
        public Currency SecondCurrency { get; set; }
        public double PriceFirstToSecond { get; set; } = 0;
        public double PreviousPriceFirstToSecond { get; set; } = 0;
        public double TargetPrice { get; set; } = 0;
        public double TargetPriceAbsoluteChange { get; set; } = 0;
        public double TargetPricePercentChange { get; set; } = 0;
        public bool IsNotifyOnPrice { get; set; } = false;
        public bool IsNotifyOnAbsoluteChange { get; set; } = false;
        public bool IsNotifyOnPercentChange { get; set; } = false;
    }
}
