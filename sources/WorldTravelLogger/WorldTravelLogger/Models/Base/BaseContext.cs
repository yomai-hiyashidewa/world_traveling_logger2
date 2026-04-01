using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup.Localizer;
using WorldTravelLogger.Models.Enumeration;
using WorldTravelLogger.Models.Interface;
using WorldTravelLogger.Models.List;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WorldTravelLogger.Models.Base
{
    // 内容抽象クラス
    public abstract class BaseContext : IContext

    {
        private DateTime date_;         // 日付
        private CountryType country_;   // 国
        private string? region_;        // 地域
        private double price_;             // 値段
        private CurrencyType currency_; // 通貨
        private string? memo_;          // メモ

        public BaseContext()
        {
            date_       = DateTime.Now;
            country_    = CountryType.JPN;
            region_     = null;
            price_      = 0.0;
            currency_ = CurrencyType.JPY;
            memo_ = null;
        }

        public BaseContext(DateTime date, CountryType country, string? region, double price, CurrencyType currency, string? memo)
        {
            date_ = date;
            country_ = country;
            region_ = ConvertUpperStringOnlyTop(region);
            price_ = price;
            currency_ = currency;
            memo_ = memo;
            switch (currency_)
            {
                case CurrencyType.JPY:
                    JPYPrice = price;
                    break;
                case CurrencyType.EUR:
                    EURPrice = price;
                    break;
                case CurrencyType.USD:
                    USDPrice = price;
                    break;
                default:
                    break;
            }
        }

       


        public  DateTime Date { get { return date_; } }         // 日付
        public CountryType Country { get { return country_; } }   // 国
        public string? Region { get { return region_; } }        // 地域
        public double Price { get { return price_; } }             // 値段
        public CurrencyType Currency { get { return currency_; } } // 通貨
        public string? Memo { get { return memo_; } }          // メモ

        public double JPYPrice
        {
            get;
            private set;

        }

        public double EURPrice
        {
            get;
            private set;

        }

        public double USDPrice
        {
            get;
            private set;

        }

        protected string ConvertUpperStringOnlyTop(string? str)
        {
            if (!string.IsNullOrWhiteSpace(str))
            {
                return char.ToUpper(str[0]) + str.Substring(1);
            }
            else
            {
                return str;
            }
        }

        public void ConvertPrice(ExchangeRater rater)
        {
            if (Currency == CurrencyType.JPY)
            {
                var rate = rater.GetRate(CurrencyType.EUR, date_);
                if (rate != 0)
                {
                    EURPrice = Price / rate;
                }
                rate = rater.GetRate(CurrencyType.USD, date_);
                if (rate != 0)
                {
                    USDPrice = Price / rate;
                }

            }
            else
            {
                JPYPrice = Price * rater.GetRate(currency_, date_);
                if(Currency != CurrencyType.EUR)
                {
                    var rate = rater.GetRate(CurrencyType.EUR, date_);
                    if (rate != 0)
                    {
                        EURPrice = JPYPrice / rate;
                    }
                }
                if (Currency != CurrencyType.USD)
                {
                    var rate = rater.GetRate(CurrencyType.USD, date_);
                    if (rate != 0)
                    {
                        USDPrice = JPYPrice / rate;
                    }
                }

            }
        }

        public string GetDataString(DateTime date)
        {
            return date.ToString("yyyy/MM/dd");
        }

        protected void ConvertJPYPrice(double rate)
        {
            JPYPrice = Price * rate;
        }

        protected void ConvertEurPrice(double rate)
        {
            EURPrice = Price / rate;
        }

        protected void ConvertUSDPrice(double rate)
        {
            EURPrice = Price / rate;
        }


        public string DateString
        {
            get
            {
                return GetDataString(date_);
            }
        }

       

        public string JPYPriceString
        {
            get
            {
                return JPYPrice.ToString("C");
            }
        }

    }
}
