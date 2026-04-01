using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldTravelLogger.Models.Base;
using WorldTravelLogger.Models.Context;
using WorldTravelLogger.Models.Enumeration;
using WorldTravelLogger.Models.Interface;

namespace WorldTravelLogger.Models.List
{
    public class OtherList : BaseContextList
    {

        public OtherType CurrentOtherType { get; set; }

        private void SetCurrentOtherType()
        {
            if (!base.ContainType(CurrentOtherType.ToString()))
            {
                var tModel = hSet_.FirstOrDefault();
                if (tModel != null)
                {
                    var type = base.ConvertOtherType(tModel.Type);
                    if (type != null)
                    {
                        CurrentOtherType = (OtherType)type;
                    }
                }
            }
        }


        protected override void CalcTypeModels(IEnumerable<IContext> list)
        {
            var dic = new Dictionary<OtherType, OtherTypeModel>();
            foreach (var model in list)
            {
                var oModel = (OtherModel)model;
                OtherTypeModel tModel;
                if (dic.TryGetValue(oModel.OtherType, out tModel))
                {
                    tModel.Set(model.JPYPrice);
                }
                else
                {
                    tModel = new OtherTypeModel(oModel.OtherType);
                    tModel.Set(model.JPYPrice);
                    dic.Add(oModel.OtherType, tModel);
                }
            }
            base.Clear();
            foreach (var pair in dic)
            {
                base.SetModel(pair.Value);
            }
            SetCurrentOtherType();
        }

        protected override IEnumerable<IContext> GetContexts(ControlModel control)
        {
            return list_.OfType<OtherModel>().
                Where(m => control.CheckControl(m.OtherType, m.Date, m.Country));
        }

        public OtherTypeModel[] TypeOthers
        {
            get
            {
                return hSet_.OfType<OtherTypeModel>().ToArray();
            }
        }


        public OtherType[] CurrentOtherTypes
        {
            get
            {
                var list = new List<OtherType>();
                foreach (var model in hSet_)
                {
                    var type = base.ConvertOtherType(model.Type);
                    if (type != null)
                    {
                        list.Add((OtherType)type);
                    }
                }
                return list.ToArray();
            }
        }

        public void ImportOthers(List<IContext> list)
        {
            foreach(var model in list.OfType<SightseeingModel>())
            {
                list_.Add(new OtherModel((OtherType)base.ConvertOtherType(model.SightseeigType.ToString()), model));
            }
        }





        protected override bool CheckFormat(object[] arrays)
        {
            return true;
        }

        protected override void Set(object[] arrays)
        {
            // none
        }
    }
}
