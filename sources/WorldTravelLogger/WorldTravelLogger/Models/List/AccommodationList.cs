using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Xsl;
using WorldTravelLogger.Models.Base;
using WorldTravelLogger.Models.Context;
using WorldTravelLogger.Models.Enumeration;
using WorldTravelLogger.Models.Interface;

namespace WorldTravelLogger.Models.List
{
    public class AccommodationList : BaseContextList
    {
        public AccommodationType CurrentAccommodationtype { get; set; }

        public AccommodationList()
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
                    // date
                    case 0:
                        var date = base.ConvertDate(str);
                        flag = date == null;
                        break;
                    // country
                    case 1:
                        var country = base.ConvertCountry(str);
                        flag = country == null;
                        break;
                    // region
                    case 2:
                        // none
                        break;
                    // Accommodation
                    case 3:
                        var Accommodation = base.ConvertAccommodationType(str);
                        flag = Accommodation == null;
                        break;
                    // price
                    case 4:
                        var price = base.ConvertDouble(str);
                        flag = price == null;
                        break;
                    /// currency
                    case 5:
                        var currency = base.ConvertCurrency(str);
                        flag = currency == null;
                        break;
                    // memo
                    case 6:
                        break;
                }
                if (flag)
                {
                    base.SetErrorList(index, j, str);
                }

            }
        }

        private IContext CreateModel(string[] row)
        {
            DateTime? date = null;
            CountryType? country = null;
            string region = null;
            AccommodationType? Accommodation = null;
            double? price = null;
            CurrencyType? currency = null;
            string memo = null;


            for (var i = 0; i < row.Length; i++)
            {
                var str = row[i];
                switch (i)
                {
                    // date
                    case 0:
                        date = base.ConvertDate(str);
                        break;
                    // country
                    case 1:
                        country = base.ConvertCountry(str);
                        break;
                    // region
                    case 2:
                        region = str;
                        break;
                    // Accommodation
                    case 3:
                        Accommodation = base.ConvertAccommodationType(str);
                        break;
                    // price
                    case 4:
                        price = base.ConvertDouble(str);
                        break;
                    /// currency
                    case 5:
                        currency = base.ConvertCurrency(str);
                        break;
                    // memo
                    case 6:
                        memo = str;
                        break;
                }
            }
            if (date != null && country != null && region != null && Accommodation != null &&
                price != null && memo != null)
            {
                return new AccommodationModel((DateTime)date, (CountryType)country, region,
                    (AccommodationType)Accommodation, (double)price, (CurrencyType)currency, memo);
            }
            else
            {
                return null;
            }
        }


        private void SetCurrentAccommodationType()
        {
            if (!base.ContainType(CurrentAccommodationtype.ToString()))
            {
                var tModel = hSet_.FirstOrDefault();
                if (tModel != null) {
                    var type = base.ConvertAccommodationType(tModel.Type);
                    if (type != null)
                    {
                        CurrentAccommodationtype = (AccommodationType)type;
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
            var list = new List<int[]>();
            for (var i = 1; i < length; i++)
            {
                var model = CreateModel((string[])arrays[i]);
                if (model != null)
                {
                    base.SetContext(model);
                }
            }
        }

        protected override void CalcTypeModels(IEnumerable<IContext> list)
        {
            var dic = new Dictionary<AccommodationType, AccommodationTypeModel>();
            foreach (var model in list)
            {
                var aModel = (AccommodationModel)model;
                AccommodationTypeModel tModel;
                if (dic.TryGetValue(aModel.Accommodation, out tModel))
                {
                    tModel.Set(model.JPYPrice);
                }
                else
                {
                    tModel = new AccommodationTypeModel(aModel.Accommodation);
                    tModel.Set(model.JPYPrice);
                    dic.Add(aModel.Accommodation, tModel);
                }
            }
            base.Clear();
            foreach (var pair in dic)
            {
                base.SetModel(pair.Value);
            }
            SetCurrentAccommodationType();
        }

        public AccommodationTypeModel[] TypeAccommodations
        {
            get
            {
                return hSet_.OfType<AccommodationTypeModel>().ToArray();
            }
        }

        public AccommodationType[] CurrentAccommodationTypes
        {
            get
            {
                var list = new List<AccommodationType>();
                foreach (var model in hSet_)
                {
                    var type = base.ConvertAccommodationType(model.Type);
                    if (type != null)
                    {
                        list.Add((AccommodationType)type);
                    }
                }
                return list.ToArray();
            }
        }










    }
}
