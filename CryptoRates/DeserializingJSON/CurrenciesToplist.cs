using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoRates.DeserializingJSON
{

    public class CurrenciesToplist
    {
        public string Message { get; set; }
        public int Type { get; set; }
        public List<object> SponsoredData { get; set; }
        public List<Datum> Data { get; set; }
        public Ratelimit RateLimit { get; set; }
        public bool HasWarning { get; set; }
    }

    public class Ratelimit
    {
    }

    public class Datum
    {
        public Coininfo CoinInfo { get; set; }
        public RAW RAW { get; set; }
        public DISPLAY DISPLAY { get; set; }
    }

    public class Coininfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Internal { get; set; }
        public string ImageUrl { get; set; }
        public string Url { get; set; }
        public string Algorithm { get; set; }
        public string ProofType { get; set; }
        public Rating Rating { get; set; }
        public float NetHashesPerSecond { get; set; }
        public int BlockNumber { get; set; }
        public int BlockTime { get; set; }
        public float BlockReward { get; set; }
        public int Type { get; set; }
        public string DocumentType { get; set; }
    }

    public class Rating
    {
        public Weiss Weiss { get; set; }
    }

    public class Weiss
    {
        public string Rating { get; set; }
        public string TechnologyAdoptionRating { get; set; }
        public string MarketPerformanceRating { get; set; }
    }

    public class RAW
    {
        public USD USD { get; set; }
    }

    public class USD
    {
        public string TYPE { get; set; }
        public string MARKET { get; set; }
        public string FROMSYMBOL { get; set; }
        public string TOSYMBOL { get; set; }
        public string FLAGS { get; set; }
        public float PRICE { get; set; }
        public int LASTUPDATE { get; set; }
        public float MEDIAN { get; set; }
        public float LASTVOLUME { get; set; }
        public float LASTVOLUMETO { get; set; }
        public string LASTTRADEID { get; set; }
        public float VOLUMEDAY { get; set; }
        public float VOLUMEDAYTO { get; set; }
        public float VOLUME24HOUR { get; set; }
        public float VOLUME24HOURTO { get; set; }
        public float OPENDAY { get; set; }
        public float HIGHDAY { get; set; }
        public float LOWDAY { get; set; }
        public float OPEN24HOUR { get; set; }
        public float HIGH24HOUR { get; set; }
        public float LOW24HOUR { get; set; }
        public string LASTMARKET { get; set; }
        public float VOLUMEHOUR { get; set; }
        public float VOLUMEHOURTO { get; set; }
        public float OPENHOUR { get; set; }
        public float HIGHHOUR { get; set; }
        public float LOWHOUR { get; set; }
        public float TOPTIERVOLUME24HOUR { get; set; }
        public float TOPTIERVOLUME24HOURTO { get; set; }
        public float CHANGE24HOUR { get; set; }
        public float CHANGEPCT24HOUR { get; set; }
        public float CHANGEDAY { get; set; }
        public float CHANGEPCTDAY { get; set; }
        public float CHANGEHOUR { get; set; }
        public float CHANGEPCTHOUR { get; set; }
        public float SUPPLY { get; set; }
        public float MKTCAP { get; set; }
        public float TOTALVOLUME24H { get; set; }
        public float TOTALVOLUME24HTO { get; set; }
        public float TOTALTOPTIERVOLUME24H { get; set; }
        public float TOTALTOPTIERVOLUME24HTO { get; set; }
        public string IMAGEURL { get; set; }
        public string CONVERSIONTYPE { get; set; }
        public string CONVERSIONSYMBOL { get; set; }
    }

    public class DISPLAY
    {
        public USD1 USD { get; set; }
    }

    public class USD1
    {
        public string FROMSYMBOL { get; set; }
        public string TOSYMBOL { get; set; }
        public string MARKET { get; set; }
        public string PRICE { get; set; }
        public string LASTUPDATE { get; set; }
        public string LASTVOLUME { get; set; }
        public string LASTVOLUMETO { get; set; }
        public string LASTTRADEID { get; set; }
        public string VOLUMEDAY { get; set; }
        public string VOLUMEDAYTO { get; set; }
        public string VOLUME24HOUR { get; set; }
        public string VOLUME24HOURTO { get; set; }
        public string OPENDAY { get; set; }
        public string HIGHDAY { get; set; }
        public string LOWDAY { get; set; }
        public string OPEN24HOUR { get; set; }
        public string HIGH24HOUR { get; set; }
        public string LOW24HOUR { get; set; }
        public string LASTMARKET { get; set; }
        public string VOLUMEHOUR { get; set; }
        public string VOLUMEHOURTO { get; set; }
        public string OPENHOUR { get; set; }
        public string HIGHHOUR { get; set; }
        public string LOWHOUR { get; set; }
        public string TOPTIERVOLUME24HOUR { get; set; }
        public string TOPTIERVOLUME24HOURTO { get; set; }
        public string CHANGE24HOUR { get; set; }
        public string CHANGEPCT24HOUR { get; set; }
        public string CHANGEDAY { get; set; }
        public string CHANGEPCTDAY { get; set; }
        public string CHANGEHOUR { get; set; }
        public string CHANGEPCTHOUR { get; set; }
        public string SUPPLY { get; set; }
        public string MKTCAP { get; set; }
        public string TOTALVOLUME24H { get; set; }
        public string TOTALVOLUME24HTO { get; set; }
        public string TOTALTOPTIERVOLUME24H { get; set; }
        public string TOTALTOPTIERVOLUME24HTO { get; set; }
        public string IMAGEURL { get; set; }
        public string CONVERSIONTYPE { get; set; }
        public string CONVERSIONSYMBOL { get; set; }
    }

}
