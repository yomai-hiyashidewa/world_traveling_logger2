using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldTravelLogger.Models.Base;
using WorldTravelLogger.Models.Enumeration;

namespace WorldTravelLogger.Models.Context
{
    public class OtherModel : BaseContext
    {
        private string? context_;                   // 内容
        private OtherType otherType_;   // 種別


        public OtherModel() :
                base()
        {
            context_ = null;
            otherType_ = OtherType.Accident;

        }



        public OtherModel(OtherType type, SightseeingModel model) :
            base(model.Date,model.Country,model.Region,model.Price,model.Currency,model.Memo)
        {
            otherType_ = type;
            context_ = model.Context;
            
        }

        public string? Context
        {
            get { return context_; }
        }

        public OtherType OtherType
        {
            get { return otherType_; }
        }
    }
}