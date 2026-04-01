using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WorldTravelLogger.Models.Enumeration;
using WorldTravelLogger.Models.Interface;
using WorldTravelLogger.Models.List;

namespace WorldTravelLogger.Models.Base
{
    public abstract class BaseContextList : BaseList, IContextList
    {

        private DateTime? startDate_;
        private DateTime? endDate_;

        protected List<IContext> list_;
        protected List<IContext> calcList_;
        protected List<IContext> calcRegionList_;

        protected HashSet<ITypeModel> hSet_;

        public event EventHandler ListChanged;

       
       

        protected BaseContextList()
        {
            list_ = new List<IContext>();
            calcList_ = new List<IContext>();
            calcRegionList_ = new List<IContext>();
            hSet_ = new HashSet<ITypeModel>();
            CountriesAndRegions = new Dictionary<CountryType, HashSet<string>>();
            CountriesAndCurrencies = new Dictionary<CountryType, HashSet<CurrencyType>>();
            startDate_ = null;
            endDate_ = null;
        }

      

        protected void FireListChanged()
        {
            if (ListChanged != null)
            {
                ListChanged(this, EventArgs.Empty);
            }
        }

        protected void SetContext(IContext context)
        {
            list_.Add(context);
            SetCountry(context.Country, context.Region);
            SetCountryAndCurrency(context.Country, context.Currency);
            SetDate(context.Date);
        }

        protected void SetCountry(CountryType type, string region)
        {

            if (!CountriesAndRegions.ContainsKey(type))
            {
                var context = new HashSet<string>();
                CountriesAndRegions.Add(type, context);
            }
            if (!string.IsNullOrWhiteSpace(region) &&
                !CountriesAndRegions[type].Contains(region))
            {
                CountriesAndRegions[type].Add(region);
            }
        }

        protected void SetCountryAndCurrency(CountryType type, CurrencyType currencyType)
        {
            if (!CountriesAndCurrencies.ContainsKey(type))
            {
                var context = new HashSet<CurrencyType>();
                CountriesAndCurrencies.Add(type, context);
            }
            if (!CountriesAndCurrencies[type].Contains(currencyType))
            {
                CountriesAndCurrencies[type].Add(currencyType);
            }
        }

        protected void SetDate(DateTime date)
        {
            if (startDate_ == null || endDate_ == null)
            {
                startDate_ = date;
                endDate_ = date;
            }
            else if (startDate_ > date)
            {
                startDate_ = date;
            }
            else if (endDate_ < date)
            {
                endDate_ = date;
            }
        }

        protected virtual IEnumerable<IContext> GetContexts(ControlModel control)
        {
            return list_.Where(m => control.CheckControl(m.Date, m.Country));
        }

        protected virtual IEnumerable<IContext> GetRegionContexts(ControlModel control)
        {
            return calcList_.Where(m => m.Region == control.CurrentRegion);
        }

        protected abstract void CalcTypeModels(IEnumerable<IContext> list);

        protected void Clear()
        {
            hSet_.Clear();
        }

        protected void SetModel(ITypeModel model)
        {
            hSet_.Add(model);
        }

        protected bool ContainType(string type)
        {
            return hSet_.Count >= 0 && null != hSet_.FirstOrDefault(m => m.Type == type);
        }

        public override bool IsLoaded
        {
            get { return list_.Count > 0; }
        }

        public bool IsReady
        {
            get { return calcList_.Count > 0; }
        }


        public Dictionary<CountryType, HashSet<string>> CountriesAndRegions
        {
            get;
            protected set;
        }

        public Dictionary<CountryType, HashSet<CurrencyType>> CountriesAndCurrencies
        {
            get; protected set;
        }

        public DateTime StartDate
        {
            get
            {
                if (startDate_ == null)
                {
                    return DateTime.Now;
                }
                else
                {
                    return (DateTime)startDate_;
                }
            }
        }

        public DateTime EndDate
        {
            get
            {
                if (endDate_ == null)
                {
                    return DateTime.Now;
                }
                else
                {
                    return (DateTime)endDate_;
                }
            }
        }




        public void ConvertAnotherCurrency(ExchangeRater rater)
        {
            foreach (var model in list_)
            {
                model.ConvertPrice(rater);
            }
        }

        public override void Init()
        {
            base.Init();
            list_.Clear();
        }

        public virtual void InitContexts()
        {
            CountriesAndRegions.Clear();
            CountriesAndCurrencies.Clear();
            startDate_ = null;
            endDate_ = null;
        }

        public void SetCoutriesAndRegions(Dictionary<CountryType, HashSet<string>> dic)
        {
            foreach (var context in CountriesAndRegions)
            {
                if (!dic.ContainsKey(context.Key))
                {
                    dic.Add(context.Key, context.Value);
                }
                else
                {
                    foreach (var region in context.Value)
                    {
                        if (!dic[context.Key].Contains(region))
                        {
                            dic[context.Key].Add(region);
                        }
                    }

                }
            }
        }

        public void SetCoutriesAndCurrencies(Dictionary<CountryType, HashSet<CurrencyType>> dic)
        {
            foreach (var context in CountriesAndCurrencies)
            {
                if (!dic.ContainsKey(context.Key))
                {
                    dic.Add(context.Key, context.Value);
                }
                else
                {
                    foreach (var currency in context.Value)
                    {
                        if (!dic[context.Key].Contains(currency))
                        {
                            dic[context.Key].Add(currency);
                        }
                    }

                }
            }
        }

        public virtual IEnumerable<CountryType> GetCalcCounties()
        {
            var sets = new HashSet<CountryType>();
            foreach (var model in calcList_)
            {
                if (!sets.Contains(model.Country))
                {
                    sets.Add(model.Country);
                }
            }
            foreach (var c in sets)
            {
                yield return c;
            }
        }

        public virtual DateTime? GetStartCalcDate(bool isRegion)
        {
            var enumerator = GetCalcs(isRegion);

            if (enumerator.Count() > 0)
            {
                return enumerator.Min(m => m.Date);
            }
            else
            {
                return null;
            }
        }

        public virtual DateTime? GetEndCalcDate(bool isRegion)
        {
            var enumerator = GetCalcs(isRegion);
            if (enumerator.Count() > 0)
            {
                return enumerator.Max(m => m.Date);
            }
            else
            {
                return null;
            }
        }

        public virtual HashSet<DateTime> GetCalcDates(bool isRegion, HashSet<DateTime> dates)
        {
            var hSet = new HashSet<DateTime>();
            var enumerator = GetCalcs(isRegion);
            foreach (var model in enumerator.Where(m => !dates.Contains(m.Date)))
            {
                if (!hSet.Contains(model.Date))
                {
                    hSet.Add(model.Date);
                }
            }
            return hSet;
        }

        

     

  

        public IContext[] GetArray()
        {
            return list_.ToArray();
        }

        public void CalcModels(ControlModel control)
        {
            calcList_.Clear();
            foreach (var model in GetContexts(control))
            {
                calcList_.Add(model);
            }
            CalcTypeModels(calcList_);
            FireListChanged();
        }

        public void CalcRegion(ControlModel control)
        {
            calcRegionList_.Clear();
            if (control.IsCountryRegion)
            {
                foreach (var model in GetRegionContexts(control))
                {
                    calcRegionList_.Add(model);
                }
                CalcTypeModels(calcRegionList_);
            }
            else
            {
                CalcTypeModels(calcList_);
            }
            FireListChanged();
        }

        public IEnumerable<IContext> GetCalcs(bool isRegion)
        {
            if (!isRegion)
            {
                return calcList_;
            }
            else
            {
                return calcRegionList_;
            }
        }

        

     
        public double GetTotalCost()
        {
            double sum = 0;
            foreach (var model in hSet_)
            {
                sum += model.TotalCost;
            }
            return sum;
        }

       



    }
}
