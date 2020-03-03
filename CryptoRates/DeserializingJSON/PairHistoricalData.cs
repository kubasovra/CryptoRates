using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoRates.DeserializingJSON
{

    public class PairHistoricalData
    {
        public string Response { get; set; }
        public string Message { get; set; }
        public bool HasWarning { get; set; }
        public int Type { get; set; }
        public Ratelimit RateLimit { get; set; }
        public HistoricalData Data { get; set; }
    }

    public class HistoricalData
    {
        public bool Aggregated { get; set; }
        public int TimeFrom { get; set; }
        public int TimeTo { get; set; }
        public PriceData[] Data { get; set; }
    }

    public class PriceData
    {
        public int time { get; set; }
        public float high { get; set; }
        public float low { get; set; }
        public float open { get; set; }
        public float volumefrom { get; set; }
        public float volumeto { get; set; }
        public float close { get; set; }
        public string conversionType { get; set; }
        public string conversionSymbol { get; set; }
    }

}
