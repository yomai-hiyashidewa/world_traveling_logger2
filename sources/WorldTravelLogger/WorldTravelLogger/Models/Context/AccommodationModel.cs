using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldTravelLogger.Models.Base;
using WorldTravelLogger.Models.Enumeration;
using WorldTravelLogger.Models.Interface;

namespace WorldTravelLogger.Models.Context
{
    public class AccommodationModel : BaseContext
    {
        private AccommodationType Accommodation_;     // 宿泊先

        public AccommodationModel() :
            base()
        {
            Accommodation_ = AccommodationType.Domitory;
        }

        public AccommodationModel(DateTime date,
            CountryType country,
            string? region,
            AccommodationType Accommodation,
            double price,
            CurrencyType currency, 
            string? memo) :
            base(date, country, region, price, currency, memo)
        {
            this.Accommodation_ = Accommodation;
        }

        public AccommodationType Accommodation { get { return Accommodation_; } }
    }
}
