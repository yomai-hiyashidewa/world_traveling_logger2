using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldTravelLogger.Models.Enumeration;
using WorldTravelLogger.Models.List;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WorldTravelLogger.Models.Interface
{
    public interface IContext
    {
        public DateTime Date { get; }      // 日付
        public CountryType Country { get; }   // 国
        public string? Region { get; }        // 地域
        public double Price { get; }             // 値段
        public CurrencyType Currency { get; } // 通貨
        public string? Memo { get; }          // メモ

        public double JPYPrice { get; }
        public double EURPrice { get; }

        public double USDPrice { get; }


        public string DateString { get; }

        public string JPYPriceString { get; }

        public void ConvertPrice(ExchangeRater rater);
    }
}
