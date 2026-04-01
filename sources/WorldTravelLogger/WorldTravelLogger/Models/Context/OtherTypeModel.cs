using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldTravelLogger.Models.Base;
using WorldTravelLogger.Models.Enumeration;

namespace WorldTravelLogger.Models.Context
{
    public class OtherTypeModel : BaseTypeModel
    {
        public OtherTypeModel(OtherType type) : base(type.ToString())
        {
        }
    }
}
