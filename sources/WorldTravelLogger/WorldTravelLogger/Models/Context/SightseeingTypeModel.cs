using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldTravelLogger.Models.Base;
using WorldTravelLogger.Models.Enumeration;

namespace WorldTravelLogger.Models.Context
{
    public class SightseeingTypeModel : BaseTypeModel
    {
        public SightseeingTypeModel(SightseeigType type) : 
            base(type.ToString())
        {
        }
    }
}
