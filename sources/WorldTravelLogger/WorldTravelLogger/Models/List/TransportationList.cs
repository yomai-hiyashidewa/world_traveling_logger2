using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WorldTravelLogger.Models.Base;
using WorldTravelLogger.Models.Context;
using WorldTravelLogger.Models.Enumeration;
using WorldTravelLogger.Models.Interface;

namespace WorldTravelLogger.Models.List
{
    public class TransportationList : BaseContextList
    {
        public Transportationtype CurrentTransportationType { get; set; }

        public TransportationList()
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
                    // type
                    case 0:
                        var sttype = base.ConvertTransportationType(str);
                        flag = sttype == null;
                        break;
                    // start
                    // date
                    case 1:
                        var sdate = base.ConvertDate(str);
                        flag = sdate == null;
                        break;
                    // Country
                    case 2:
                        var countury = base.ConvertCountry(str);
                        flag = countury == null;
                        break;
                    // place
                    case 3:
                        flag = string.IsNullOrWhiteSpace(str);
                        break;
                    // place type
                    case 4:
                        var sptype = base.ConvertPlaceType(str);
                        flag = sptype == null;
                        break;
                    // start
                    // date
                    case 5:
                        var edate = base.ConvertDate(str);
                        flag = edate == null;
                        break;
                    // Country
                    case 6:
                        var ecountry = base.ConvertCountry(str);
                        flag = ecountry == null;
                        break;
                    // place
                    case 7:
                        string.IsNullOrWhiteSpace(str);
                        break;
                    // place type
                    case 8:
                        var eptype = base.ConvertPlaceType(str);
                        flag = eptype == null;
                        break;
                    // distance
                    case 9:
                        var val = base.ConvertDouble(str);
                        flag = val == null;
                        break;
                    // time
                    case 10:
                        var time = base.ConvertInt(str);
                        flag = time == null;
                        break;
                    // price
                    case 11:
                        var place = base.ConvertDouble(str);
                        flag = place == null;
                        break;
                    // currency
                    case 12:
                        var currency = base.ConvertCurrency(str);
                        flag = currency == null;
                        break;
                    // memo
                    case 13:
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
            Transportationtype? ttype = null;
            DateTime? startDate = null; ;
            CountryType? startCountry = null;
            string? startPlace = null;
            PlaceType? startPlaceType = null;
            DateTime? endDate = null;
            CountryType? endCountry = null;
            string? endPlace = null;
            PlaceType? endPlaceType = null;
            double? distance = null;
            int? time = null;
            double? price = null;
            CurrencyType? currencyType = null;
            string? memo = null;
            for (var j = 0; j < row.Length; j++)
            {
                var str = row[j];


                switch (j)
                {
                    // type
                    case 0:
                        ttype = base.ConvertTransportationType(str);
                        break;
                    // start
                    // date
                    case 1:
                        startDate = base.ConvertDate(str);
                        break;
                    // Country
                    case 2:
                        startCountry = base.ConvertCountry(str);
                        break;
                    // place
                    case 3:
                        startPlace = str;
                        break;
                    // place type
                    case 4:
                        startPlaceType = base.ConvertPlaceType(str);
                        break;
                    // start
                    // date
                    case 5:
                        endDate = base.ConvertDate(str);
                        break;
                    // Country
                    case 6:
                        endCountry = base.ConvertCountry(str);
                        break;
                    // place
                    case 7:
                        endPlace = str;
                        break;
                    // place type
                    case 8:
                        endPlaceType = base.ConvertPlaceType(str);
                        break;
                    // distance
                    case 9:
                        distance = base.ConvertDouble(str);
                        break;
                    // time
                    case 10:
                        time = base.ConvertInt(str);
                        break;
                    // price
                    case 11:
                        price = base.ConvertDouble(str);
                        break;
                    // currency
                    case 12:
                        currencyType = base.ConvertCurrency(str);
                        break;
                    // memo
                    case 13:
                        memo = str;
                        break;
                }
            }
            if (ttype != null &&
                   startDate != null &&
                   startCountry != null &&
                   startPlace != null &&
                   startPlaceType != null &&
                   endDate != null &&
                   endCountry != null &&
                   endPlace != null &&
                   endPlaceType != null &&
                   distance != null &&
                   time != null &&
                   price != null &&
                   currencyType != null
                   )
            {
                var model = new TransportationModel((Transportationtype)ttype, (DateTime)startDate, (CountryType)startCountry, startPlace, (PlaceType)startPlaceType,
                    (DateTime)endDate, (CountryType)endCountry, endPlace, (PlaceType)endPlaceType,
                    (double)distance, (int)time, (double)price, (CurrencyType)currencyType, memo);
                list_.Add(model);
                base.SetCountry(model.StartCountry, model.StartRegion);
                base.SetCountry(model.EndCountry, model.EndRegion);
                base.SetCountryAndCurrency(model.Country, model.Currency);
                base.SetDate(model.StartDate);
                base.SetDate(model.EndDate);
            }
        }


        private void SetCurrentTransportationType()
        {
            if (!base.ContainType(CurrentTransportationType.ToString()))
            {
                var tModel = hSet_.FirstOrDefault();
                if (tModel != null)
                {
                    var type = base.ConvertTransportationType(tModel.Type);
                    if (type != null)
                    {
                        CurrentTransportationType = (Transportationtype)type;
                    }
                }
            }
        }

        protected override bool CheckFormat(object[] arrays)
        {
            var length = arrays.Length;
            for (var i = 2; i < length; i++)
            {
                CheckFormats(i, (string[])arrays[i]);

            }
            return base.IsError;
        }



        protected override void Set(object[] arrays)
        {
            var length = arrays.Length;
            for (var i = 2; i < length; i++)
            {
                SetContext((string[])arrays[i]);

            }
        }

        protected override IEnumerable<IContext> GetContexts(ControlModel control)
        {
            return list_.OfType<TransportationModel>().
                Where(m => control.CheckControl(m.Transportationtype, m.StartDate, m.EndDate, m.StartCountry, m.EndCountry));
        }

        protected override IEnumerable<IContext> GetRegionContexts(ControlModel control)
        {
            return calcList_.OfType<TransportationModel>().
                Where(m => m.StartRegion == control.CurrentRegion || m.EndRegion == control.CurrentRegion);
        }


        public override IEnumerable<CountryType> GetCalcCounties()
        {
            var sets = new HashSet<CountryType>();
            foreach (var model in calcList_.OfType<TransportationModel>())
            {
                if (!sets.Contains(model.StartCountry))
                {
                    sets.Add(model.StartCountry);
                }
                if (!sets.Contains(model.EndCountry))
                {
                    sets.Add(model.EndCountry);
                }
            }
            foreach (var c in sets)
            {
                yield return c;
            }
        }

        public override DateTime? GetStartCalcDate(bool isRegion)
        {
            var enumerator = GetCalcs(isRegion).OfType<TransportationModel>();

            if (enumerator.Count() > 0)
            {
                return enumerator.Min(m => m.StartDate);
            }
            else
            {
                return null;
            }
        }

        public override DateTime? GetEndCalcDate(bool isRegion)
        {
            var enumerator = GetCalcs(isRegion).OfType<TransportationModel>();
            if (enumerator.Count() > 0)
            {
                return enumerator.Max(m => m.EndDate);
            }
            else
            {
                return null;
            }
        }

        public override HashSet<DateTime> GetCalcDates(bool isRegion, HashSet<DateTime> dates)
        {
            var hSet = new HashSet<DateTime>();
            var enumerator = GetCalcs(isRegion).OfType<TransportationModel>();
            foreach (var model in enumerator.Where(m => !dates.Contains(m.StartDate) || !dates.Contains(m.EndDate)))
            {
                if (!hSet.Contains(model.Date))
                {
                    if (!hSet.Contains(model.StartDate))
                    {
                        hSet.Add(model.StartDate);
                    }
                    if (!hSet.Contains(model.EndDate))
                    {
                        hSet.Add(model.EndDate);
                    }
                }
            }
            return hSet;
        }

        protected override void CalcTypeModels(IEnumerable<IContext> list)
        {
            var dic = new Dictionary<Transportationtype, TransportationTypeModel>();
            foreach (var model in list)
            {
                var tModel = (TransportationModel)model;
                TransportationTypeModel ttModel;
                if (dic.TryGetValue(tModel.Transportationtype, out ttModel))
                {
                    ttModel.Set(model.JPYPrice);
                    ttModel.SetParameter(tModel.Distance, tModel.Time);
                }
                else
                {
                    ttModel = new TransportationTypeModel(tModel.Transportationtype);
                    ttModel.Set(model.JPYPrice);
                    ttModel.SetParameter(tModel.Distance, tModel.Time);
                    dic.Add(tModel.Transportationtype, ttModel);
                }
            }
            base.Clear();
            foreach (var pair in dic)
            {
                base.SetModel(pair.Value);
            }
            
            SetCurrentTransportationType();
        }

        public TransportationTypeModel[] TypeTransportations
        {
            get
            {
                return hSet_.OfType<TransportationTypeModel>().ToArray();
            }
        }

        public Transportationtype[] CurrentTransportationTypes
        {
            get
            {
                var list = new List<Transportationtype>();
                foreach (var model in hSet_)
                {
                    var type = base.ConvertTransportationType(model.Type);
                    if (type != null)
                    {
                        list.Add((Transportationtype)type);
                    }
                }
                return list.ToArray();
            }
        }



        public double GetTotalDistance()
        {
            double sum = 0;
            foreach (var model in hSet_.OfType<TransportationTypeModel>())
            {
                sum += model.GetTotalDistance();
            }
            return sum;
        }

        public int GetTotalTime()
        {
            int sum = 0;
            foreach (var model in hSet_.OfType<TransportationTypeModel>())
            {
                sum += model.GetTotalTime();
            }
            return sum;
        }

        public IEnumerable<TransportationModel> GetRoute(CountryType type)
        {
            return calcList_.OfType<TransportationModel>().Where(m => m.IsSameCountry(type));
        }

         

        public IEnumerable<TransportationModel> GetArrivals(CountryType type)
        {
            return calcList_.OfType<TransportationModel>().Where(m => m.EndCountry == type && !m.SameCountry);
        }

        public IEnumerable<TransportationModel> GetDepartures(CountryType type)
        {
            return calcList_.OfType<TransportationModel>().Where(m => m.StartCountry == type && !m.SameCountry);
        }

        public HashSet<CountryType> GetNoEntryCountries()
        {
            var shSet = new HashSet<CountryType>();
            shSet.Add(CountryType.UNK);
            foreach(var model in list_.OfType<TransportationModel>().Where(m => m.IsNoEntry))
            {
                if (!shSet.Contains(model.EndCountry))
                {
                    shSet.Add(model.EndCountry);
                }
            }
            return shSet;
        }


        
    }
}
