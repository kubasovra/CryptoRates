using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CryptoRates.Data
{
    public class Currency
    {
        [Key]
        public int CurrencyId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Symbol { get; set; }

        [Required]
        public double ValueUSD { get; set; }

        [Url]
        [Required]
        public string WebPage { get; set; }

        [Url]
        [Required]
        public string ImageURL { get; set; }
    }
}
