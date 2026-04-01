using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using WorldTravelLogger.Models.Enumeration;
using WorldTravelLogger.Models.Utility;

namespace WorldTravelLogger.Models.Context
{
    public class CostModel
    {
        private CurrencyType currencytype_;
        private Dictionary<ContextListType, double> costs_;

        public CostModel(CurrencyType currencytype)
        {
            currencytype_ = currencytype;
            costs_ = new Dictionary<ContextListType, double>();
        }

        public void Set(ContextListType type, double cost)
        {
            costs_[type] = cost;
        }

      

        public string Total
        {
            get
            {
                return CurrencyConverter.GetCurrencyStr(currencytype_, 
                    costs_.Values.Sum());
                
            }
        }

        public string Accommodation
        {
            get
            {
                return CurrencyConverter.GetCurrencyStr(currencytype_,
                    costs_[ContextListType.AccommodationList]);
            }
        }

        public string Transportation
        {
            get
            {
                return CurrencyConverter.GetCurrencyStr(currencytype_,
                  costs_[ContextListType.TransportationList]);
            }
        }

        public string Sightseeing
        {
            get
            {
                return CurrencyConverter.GetCurrencyStr(currencytype_,
                  costs_[ContextListType.SightseeingList]);

            }
        }

        public string Other
        {
            get
            {
                return CurrencyConverter.GetCurrencyStr(currencytype_,
                  costs_[ContextListType.Other]);
            }
        }


    }
}
