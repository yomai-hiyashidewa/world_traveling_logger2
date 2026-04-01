using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldTravelLogger.Models.Enumeration;
using WorldTravelLogger.Models.List;

namespace WorldTravelLogger.Models.Interface
{
    public interface IContextList
    {
        public bool IsLoaded { get; }

        public bool IsReady { get; }

        public Dictionary<CountryType, HashSet<string>> CountriesAndRegions { get; }

        public Dictionary<CountryType, HashSet<CurrencyType>> CountriesAndCurrencies { get; }

        public DateTime StartDate { get; }

        public DateTime EndDate { get; }

        public ErrorTypes Load(string filePath, string checkFilename);

        public void ConvertAnotherCurrency(ExchangeRater rater);

        public void Init();
        public void InitContexts();

        public void SetCoutriesAndCurrencies(Dictionary<CountryType, HashSet<CurrencyType>> dic);

        public IEnumerable<CountryType> GetCalcCounties();

        public DateTime? GetStartCalcDate(bool isRegion);

        public DateTime? GetEndCalcDate(bool isRegion);

        public HashSet<DateTime> GetCalcDates(bool isRegion, HashSet<DateTime> dates);

        public IContext[] GetArray();

        public void CalcModels(ControlModel control);

        public void CalcRegion(ControlModel control);

        public IEnumerable<IContext> GetCalcs(bool isRegion);

        public double GetTotalCost();

        public void SetCoutriesAndRegions(Dictionary<CountryType, HashSet<string>> dic);


    }
}
