using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldTravelLogger.Models.Base;
using WorldTravelLogger.Models.Enumeration;

namespace WorldTravelLogger.Models.Context
{
    public class SightseeingModel: BaseContext
    {
        private string? context_;                   // 内容
        private SightseeigType sightseeigType_;   // 観光種別

        public SightseeingModel() :
            base()
        {
            context_ = null;
            sightseeigType_ = SightseeigType.Visiting;

        }

        public SightseeingModel(string? context, 
            SightseeigType sightseeigType, 
            DateTime date, 
            CountryType country, 
            string? region, 
            double price, 
            CurrencyType currency, 
            string? memo):
            base(date,country, region, price, currency, memo)
        {
            context_ = context;
            sightseeigType_ = sightseeigType;
            ResetType();
        }

        private void ResetType()
        {
            if (string.IsNullOrWhiteSpace(context_))
            {
                return;
            }
            if (sightseeigType_ == SightseeigType.Visiting || sightseeigType_ == SightseeigType.Walking)
            {
                var upperC = context_.ToUpper();
                if (upperC.Contains("BEACH"))
                {
                    sightseeigType_ = SightseeigType.Beach;
                }
                else if (upperC.Contains("BAY") || upperC.Contains("CAVE") || upperC.Contains("VALLY") ||
                    upperC.Contains("LAKE"))
                {
                    sightseeigType_ = SightseeigType.Nature;
                }
                else if (upperC.Contains("MUSEUM"))
                {
                    sightseeigType_ = SightseeigType.Museum;
                }
                else if (upperC.Contains("CHURCH") || upperC.Contains("CATHEDRAL") || 
                    upperC.Contains("MOSK") || upperC.Contains("SHRINE"))
                {
                    sightseeigType_ = SightseeigType.Church;
                }
                else if (upperC.Contains("ZOO"))
                {
                    sightseeigType_ = SightseeigType.Zoo;
                }
                else if (upperC.Contains("HERITAGE"))
                {
                    sightseeigType_ = SightseeigType.Heritage;
                }
                else if (upperC.Contains("OVERVIEWING"))
                {
                    sightseeigType_ = SightseeigType.Overviewing;
                }
                else if (upperC.Contains("WATERFALL"))
                {
                    sightseeigType_ = SightseeigType.Waterfall;
                }
                else if (upperC.Contains("CASTLE") || upperC.Contains("FORTLESS") ||
                    upperC.Contains("PALACE"))
                {
                    sightseeigType_ = SightseeigType.Castle;
                }
                else if (upperC.Contains("PARK") || upperC.Contains("GARDEN"))
                {
                    sightseeigType_ = SightseeigType.Park;
                }
            }
        }

        public string? Context
        {
            get { return context_; }    
        }

        public SightseeigType SightseeigType
        {
            get { return sightseeigType_; }
        }


    }
}
