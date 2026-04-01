using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldTravelLogger.Models.Enumeration;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WorldTravelLogger.Models.Context
{
    public class ExchangeRateModel
    {
        private CurrencyType currency_;
        private double rate_;

        public ExchangeRateModel(CurrencyType currency_, double rate_)
        {
            this.currency_ = currency_;
            this.rate_ = rate_;
        }

        public CurrencyType Currency
        {
            get
            {
                return currency_;
            }
        }
        public string Rate
        {
            get
            {
                var checkRate = (int)rate_;
                if (checkRate > 0)
                {
                    return rate_.ToString("C");
                }
                else
                {
                    return rate_.ToString("#.##");
                }
            }
        }
    }
}
