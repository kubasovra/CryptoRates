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
    public enum PairStates
    {
        Working,
        Pending,
        Notified,
        Failed
    }
    public class Pair
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public AppUser User { get; set; }

        [Required]
        public Currency FirstCurrency { get; set; }

        [Required]
        public Currency SecondCurrency { get; set; }

        public PairStates State { get; set; } = PairStates.Working;

        public List<string> HistoricalData { get; set; }

        public double PriceFirstToSecond { get; set; } = 0;

        public double PreviousPriceFirstToSecond { get; set; } = 0;

        public double TargetPrice { get; set; } = 0;

        public bool IsNotifyOnPrice { get; set; } = false;
    }
}
