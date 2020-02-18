using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CryptoRates.Models
{
    public class CurrencyViewModel
    {
        [Required]
        [RegularExpression(@"^[\w''-'\s]{1,40}$", ErrorMessage = "Characters are not allowed")]
        public string Name { get; set; }

        [Required]
        [MaxLength(10)]
        [RegularExpression(@"^[\w''-'\s]{1,40}$", ErrorMessage = "Characters are not allowed")]
        public string Symbol { get; set; }

        [Required]
        [RegularExpression(@"^[\d'.']", ErrorMessage = "Must be numeric")]
        public double ValueUSD { get; set; }

        [Url(ErrorMessage = "URL is invalid")]
        public string WebPage { get; set; }

        [Url(ErrorMessage = "URL is invalid")]
        public string ImageURL { get; set; }

        public bool IsTracked { get; set; }
    }
}
