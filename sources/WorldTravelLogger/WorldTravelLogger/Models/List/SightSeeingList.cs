using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.CompilerServices;
using WorldTravelLogger.Models.Base;
using WorldTravelLogger.Models.Interface;
using WorldTravelLogger.Models.Enumeration;
using WorldTravelLogger.Models.Context;

namespace WorldTravelLogger.Models.List
{
    public class SightseeingList : BaseContextList
    {

        public SightseeigType CurrentSightseeingType { get; set; }

        public SightseeingList()
        {
            
        }


        private void CheckFormats(int index, string[] row)
        {
            for (var j = 0; j < row.Length; j++)
            {
                var str = row[j];
                bool flag = false;
                switch (j)
                {
                    // context
                    case 0:
                        flag = string.IsNullOrWhiteSpace(str);
                        break;
                    // type
                    case 1:
                        var sType = base.ConvertSightseeingType(str);
                        flag = sType == null;
                        break;
                    // date
                    case 2:
                        var date = base.ConvertDate(str);
                        flag = date == null;
                        break;
                    // country
                    case 3:
                        var c = base.ConvertCountry(str);
                        flag = c == null;
                        break;
                    // region
                    case 4:
                        flag = string.IsNullOrWhiteSpace(str);
                        break;
                    // price
                    case 5:
                        var price = base.ConvertDouble(str);
                        flag = price == null;
                        break;
                    // currency
                    case 6:
                        var cType = base.ConvertCurrency(str);
                        flag = cType == null;
                        break;
                    // memo
                    case 7:
                       // none
                        break;


                }
                if (flag)
                {
                    base.SetErrorList(index, j, str);
                }
            }
        }

        private void SetContext(string[] row)
        {
            string? context = null;
            SightseeigType? sType = null;
            DateTime? date = null; ;
            CountryType? country = null;
            string? region = null;
            double? price = null;
            CurrencyType? currencyType = null;
            string? memo = null;
            for (var j = 0; j < row.Length; j++)
            {
                var str = row[j];
                
                switch (j)
                {
                    // context
                    case 0:
                        context = str;
                        break;
                    // type
                    case 1:
                        sType = base.ConvertSightseeingType(str);
                        break;
                    // date
                    case 2:
                        date = base.ConvertDate(str);
                        break;
                    // country
                    case 3:
                        country = base.ConvertCountry(str);
                        break;
                    // region
                    case 4:
                        region = str;
                        break;
                        // price
                    case 5:
                        price = base.ConvertDouble(str);
                        break;
                    // currency
                    case 6:
                        currencyType = base.ConvertCurrency(str);
                        break;
                    // memo
                    case 7:
                        memo = str;
                        break;
                  
                 
                }
            }
            if (context != null &&
                   sType != null &&
                   date != null &&
                   country != null &&
                   region != null &&
                   price != null &&
                   currencyType != null
                   )
            {
                var model = new SightseeingModel(context, (SightseeigType)sType, (DateTime)date, (CountryType)country,
                    region, (double)price, (CurrencyType)currencyType, memo);
                base.SetContext(model);
            }
        }

       

        private void SetCurrentSightSeingType()
        {
            if (!base.ContainType(CurrentSightseeingType.ToString()))
            {
                var tModel = hSet_.FirstOrDefault();
                if (tModel != null)
                {
                    var type = base.ConvertSightseeingType(tModel.Type);
                    if (type != null)
                    {
                        CurrentSightseeingType = (SightseeigType)type;
                    }
                }
            }
        }




        protected override bool CheckFormat(object[] arrays)
        {
            var length = arrays.Length;
            for (var i = 1; i < length; i++)
            {
                CheckFormats(i, (string[])arrays[i]);

            }
            return base.IsError;
        }

        protected override void Set(object[] arrays)
        {
            var length = arrays.Length;
            for (var i = 1; i < length; i++)
            {
                SetContext((string[])arrays[i]);

            }
        }

        protected override void CalcTypeModels(IEnumerable<IContext> list)
        {
            var dic = new Dictionary<SightseeigType, SightseeingTypeModel>();
            foreach (var model in list)
            {
                var sModel = (SightseeingModel)model;
                SightseeingTypeModel tModel;
                if (dic.TryGetValue(sModel.SightseeigType, out tModel))
                {
                    tModel.Set(model.JPYPrice);
                }
                else
                {
                    tModel = new SightseeingTypeModel(sModel.SightseeigType);
                    tModel.Set(model.JPYPrice);
                    dic.Add(sModel.SightseeigType, tModel);
                }
            }
            base.Clear();
            foreach (var pair in dic)
            {
                base.SetModel(pair.Value);
            }
            SetCurrentSightSeingType();
        }


        protected override IEnumerable<IContext> GetContexts(ControlModel control)
        {
            return list_.OfType<SightseeingModel>().
                Where(m => control.CheckControl(m.SightseeigType, m.Date, m.Country));
        }

        public SightseeingTypeModel[] TypeSightseeings
        {
            get
            {
                return hSet_.OfType<SightseeingTypeModel>().ToArray();
            }
        }


        public SightseeigType[] CurrentSightseeingTypes
        {
            get
            {
                var list = new List<SightseeigType>();
                foreach (var model in hSet_)
                {
                    var type = base.ConvertSightseeingType(model.Type);
                    if (type != null)
                    {
                        list.Add((SightseeigType)type);
                    }
                }
                return list.ToArray();
            }
        }





        // others
        private bool CheckOthers(SightseeigType type)
        {
            var oType = base.ConvertOtherType(type.ToString());
            return oType != null;
        }

        public List<IContext> ExportOthers()
        {
            var list = new List<IContext>();
            foreach(var model in list_.OfType<SightseeingModel>().
                Where(m => CheckOthers(m.SightseeigType)).ToArray())
            {
                list.Add(model);
                list_.Remove(model);
            }
            return list;
        }

        
       
    }
}
