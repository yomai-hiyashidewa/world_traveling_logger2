using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using WorldTravelLogger.Models.Context;
using WorldTravelLogger.Models.Csv;
using WorldTravelLogger.Models.Enumeration;
using WorldTravelLogger.Models.Interface;
using WorldTravelLogger.Models.List;
using WorldTravelLogger.Models.Utility;

namespace WorldTravelLogger.Models
{
    public class MainModel
    {

       

        private ControlModel controllModel_;
        private ExchangeRater exchangeRater_;
        private OptionModel option_;

        private Dictionary<ContextListType, IContextList> listDic_;



        private Dictionary<CountryType, HashSet<string>> countriesAndRegions_;
        
        private Dictionary<CountryType, HashSet<CurrencyType>> countriesAndCurrencies_;
        private HashSet<CountryType> calcCountries_;
        private HashSet<CountryType> noEntryCounries_;

        public event EventHandler<FileLoadedEventArgs> FileLoaded_;
        public event EventHandler ImageListReady_;
        public event EventHandler CalcCompleted_;
        public event EventHandler CalcRouteCompleted_;



      

        private void InitSightseeing()
        {
            var sightseeings = (SightseeingList)listDic_[ContextListType.SightseeingList];
            var others = (OtherList)listDic_[ContextListType.Other];
            // otherも初期化
            others.Init();
            others.ImportOthers(sightseeings.ExportOthers());
            SetExchangeRate(ContextListType.SightseeingList);
            SetExchangeRate(ContextListType.Other);

            SetCountries();
            SetDate();
            CalcList(ContextListType.SightseeingList,false);
            CalcList(ContextListType.Other,true);
        }

        private void LoadImage()
        {
            if (!string.IsNullOrWhiteSpace(option_.ImagePath) && Path.Exists(option_.ImagePath))
            {
                if (ImageListReady_ != null)
                {
                    ImageListReady_.Invoke(this, EventArgs.Empty);
                }

            }
        }

        private void LoadExchange()
        {
            var filePath = option_.ExchangeRatePath;
            if (!string.IsNullOrWhiteSpace(filePath) && Path.Exists(filePath))
            {
                exchangeRater_.Init();
                var result = exchangeRater_.Load(option_.ExchangeRatePath, FileNames.ExchangeRateFile);
                if (result != ErrorTypes.None)
                {
                    
                }
                else
                {
                    SetExchangeRateAll();
                    CalcListAll();
                }
                FireFileLoaded(ListType.ExchangeRateList, result);
            }
        }

        private void SetExchangeRate(ContextListType type)
        {

            if (exchangeRater_ != null && exchangeRater_.IsLoaded)
            {
                var list = listDic_[type];
                if (list.IsLoaded)
                {
                    list.ConvertAnotherCurrency(exchangeRater_);
                }
            }
        }

        private void SetExchangeRateAll()
        {
            foreach (var type in listDic_.Keys)
            {
                SetExchangeRate(type);
            }
        }

        private ContextListType? ConvertListType(ListType type)
        {
            ContextListType? result = null;
            switch (type)
            {
                case ListType.AccommodationList:
                    result = ContextListType.AccommodationList;
                    break;
                case ListType.TransportationList:
                    result = ContextListType.TransportationList;
                    break;
                case ListType.SightseeingList:
                    result = ContextListType.SightseeingList;
                    break;
            }
            return result;
        }

        private void FireFileLoaded(ListType type, ErrorTypes error)
        {
            if (FileLoaded_ != null)
            {
                FileLoaded_.Invoke(this, new FileLoadedEventArgs(type, error));
            }
        }

        private void InitList(ContextListType type)
        {
            if (type == ContextListType.SightseeingList)
            {
                InitSightseeing();
            }
            else
            {
                SetExchangeRate(type);
                SetCountries();
                SetDate();
                CalcList(type,true);
            }
        }

        private void LoadList(ListType listType)
        {
            var filePath = option_.GetFilePath(listType);
            var flag = Path.Exists(filePath);
            if (!string.IsNullOrWhiteSpace(filePath) && Path.Exists(filePath))
            {
                var contextListType = ConvertListType(listType);
                if (contextListType != null)
                {
                    var list = listDic_[(ContextListType)contextListType];
                    list.Init();
                    var result = list.Load(filePath, FileNames.GetFileName(listType));
                    if (result != ErrorTypes.None)
                    {
                        
                    }
                    else
                    {
                        InitList((ContextListType)contextListType);
                    }
                    FireFileLoaded(listType, result);
                }
            }


        }

        public MainModel()
        {
            option_ = new OptionModel();
            option_.ReqLoad += Option__ReqLoad;
            controllModel_ = new ControlModel();
            exchangeRater_ = new ExchangeRater();
            listDic_ = new Dictionary<ContextListType, IContextList>();
            listDic_.Add(ContextListType.AccommodationList, new AccommodationList());
            listDic_.Add(ContextListType.TransportationList, new TransportationList());
            listDic_.Add(ContextListType.SightseeingList, new SightseeingList());
            listDic_.Add(ContextListType.Other, new OtherList());

            countriesAndRegions_ = new Dictionary<CountryType, HashSet<string>>();
            countriesAndCurrencies_ = new Dictionary<CountryType, HashSet<CurrencyType>>();
            calcCountries_ = new HashSet<CountryType>();
            noEntryCounries_ = new HashSet<CountryType>();
            controllModel_.ControlChanged_ += ControllModel__ControlChanged_;
            controllModel_.RegionChanged_ += ControllModel__RegionChanged_;
        }

        private void Option__ReqLoad(object? sender, FileLoadedEventArgs e)
        {
            switch (e.Type)
            {
                case ListType.AccommodationList:
                case ListType.TransportationList:
                case ListType.SightseeingList:
                    LoadList(e.Type);
                    break;
                case ListType.ExchangeRateList:
                    LoadExchange();
                    break;
                case ListType.ImageList:
                    LoadImage();
                    break;
            }
        }

        private void ControllModel__RegionChanged_(object? sender, EventArgs e)
        {
            CalcRegionAll();
        }

        private void ControllModel__ControlChanged_(object? sender, EventArgs e)
        {
            CalcListAll();
        }

        public void Init()
        {
            exchangeRater_.Init();
            foreach (var list in listDic_.Values)
            {
                list.Init();
            }
            this.Load();
        }

        private void Load()
        {
            var data = CSVReader.ReadCSV(FileNames.SAVE_FILE_NAME);
            if (data != null)
            {
                option_.Load(data);
            }
            LoadList(ListType.AccommodationList);
            LoadList(ListType.TransportationList);
            LoadList(ListType.SightseeingList);
            LoadExchange();
        }



        public void Exit()
        {
            var data = option_.GetSaveData();
            CSVWriter.WriteCSV(FileNames.SAVE_FILE_NAME, data);
        }

        public OptionModel GetOptionModel()
        {
            return option_;
        }

        public string ImageDir
        {
            get
            {
                return option_.ImagePath;
            }
        }


        // control
    

        public void SetCurrentRegion()
        {
            if (countriesAndRegions_.Count > 0 && countriesAndRegions_.ContainsKey(controllModel_.CurrentCountryType))
            {
                var value = countriesAndRegions_[controllModel_.CurrentCountryType];
                if (value != null) ;
                controllModel_.CurrentRegion = value.FirstOrDefault();

            }

        }


        private void SetCountries()
        {
            countriesAndRegions_.Clear();
            countriesAndCurrencies_.Clear();
            foreach (var list in listDic_.Values)
            {
                list.SetCoutriesAndRegions(countriesAndRegions_);
                list.SetCoutriesAndCurrencies(countriesAndCurrencies_);
            }
            SetCurrentRegion();
        }

        private void CalcCountries()
        {
            calcCountries_.Clear();
            foreach (var list in listDic_.Values)
            {
                foreach (var c in list.GetCalcCounties().Where(c => !calcCountries_.Contains(c)))
                {
                    calcCountries_.Add(c);
                }
            }
            var tList = (TransportationList)listDic_[ContextListType.TransportationList];
            noEntryCounries_ = tList.GetNoEntryCountries();
        }

      

        private void SetDate()
        {
            foreach (var list in listDic_.Values)
            {
                controllModel_.SetStartSetDate(list.StartDate);
                controllModel_.SetEndSetDate(list.EndDate);
            }
            controllModel_.InitDate();
        }

        private void CalcDate()
        {

            foreach (var pair in listDic_)
            {
                var list = pair.Value;
                if (pair.Key == ContextListType.AccommodationList)
                {
                    controllModel_.ResetCalcDate(list.GetStartCalcDate(controllModel_.IsCountryRegion), list.GetEndCalcDate(controllModel_.IsCountryRegion));
                }
                else
                {
                    controllModel_.SetStartCalcDate(list.GetStartCalcDate(controllModel_.IsCountryRegion));
                    controllModel_.SetEndCalcDate(list.GetEndCalcDate(controllModel_.IsCountryRegion));
                }
            }
            controllModel_.InitDateFromCalc();
        }

        private void CalcList(ContextListType type, bool isLoad)
        {
            var list = listDic_[type];
            list.CalcModels(controllModel_);
            if (isLoad)
            {
                CalcApplication();
            }
        }


        private void CalcListAll()
        {
            foreach (var type in listDic_.Keys)
            {
                CalcList(type,false);
            }
            CalcApplication();
        }


        private void CalcApplication()
        {
            CalcCountries();
            CalcDate();
            if (CalcCompleted_ != null)
            {
                CalcCompleted_.Invoke(this, EventArgs.Empty);
            }
        }

        private void CalcRegionApplication()
        {
            CalcDate();
            if (CalcCompleted_ != null)
            {
                CalcCompleted_.Invoke(this, EventArgs.Empty);
            }
        }


        private void CalcRegionAll()
        {
            foreach (var pair in listDic_)
            {
                pair.Value.CalcRegion(controllModel_);
            }
            CalcRegionApplication();
        }





        public IEnumerable<CountryType> GetCountries()
        {
            foreach (var c in countriesAndRegions_)
            {
                yield return c.Key;
            }
        }

        public int GetTotalSetCountries()
        {

            return countriesAndRegions_.Keys.Count(c => !noEntryCounries_.Contains(c));
        }

        public string[] GetCurrentRegions()
        {
            if (countriesAndRegions_.Count > 0 &&
                countriesAndRegions_.ContainsKey(controllModel_.CurrentCountryType))
            {
                return countriesAndRegions_[controllModel_.CurrentCountryType].ToArray();
            }
            else
            {
                return null;
            }
        }

        public int GetCurrentRegionCount()
        {
            if (countriesAndRegions_.Count > 0 &&
                countriesAndRegions_.ContainsKey(controllModel_.CurrentCountryType))
            {
                return countriesAndRegions_[controllModel_.CurrentCountryType].Count;
            }
            else
            {
                return 0;
            }
        }

        public ExchangeRateModel[] GetCurrentExchangeRates()
        {
            if (countriesAndCurrencies_.Count > 0 &&
                countriesAndCurrencies_.ContainsKey(controllModel_.CurrentCountryType))
            {
                var list = new List<ExchangeRateModel>();
                foreach (var currency in countriesAndCurrencies_[controllModel_.CurrentCountryType].Where(c => c != CurrencyType.JPY))
                {
                    ExchangeRateModel model = null;
                    if (controllModel_.EnableCalcDate)
                    {
                        model = new ExchangeRateModel(currency, exchangeRater_.GetAverageRate(currency, (DateTime)controllModel_.StartCalcDate, (DateTime)controllModel_.EndCalcDate));
                    }
                    else
                    {
                        model = new ExchangeRateModel(currency, exchangeRater_.GetAverageRate(currency, controllModel_.StartDate, controllModel_.EndDate));
                    }
                    list.Add(model);
                }
                return list.ToArray();
            }
            else
            {
                return null;
            }
        }


        public int GetTotalRegionCount()
        {
            var sum = 0;
            foreach (var pair in countriesAndRegions_)
            {
                sum += pair.Value.Count;
            }
            return sum;

        }

        public int TotalCalcCountries
        {
            get
            {
                return calcCountries_.Count;
            }
        }

        public int TotalCalcDays
        {
            get
            {
                var hSet = new HashSet<DateTime>();
                foreach (var list in listDic_.Values)
                {
                    foreach (var date in list.GetCalcDates(controllModel_.IsCountryRegion, hSet))
                    {
                        hSet.Add(date);
                    }
                }
                return hSet.Count;
            }
        }

        public CostModel GetCostModel()
        {
            var cost = new CostModel(CurrencyType.JPY);
            foreach (var pair in listDic_)
            {
                cost.Set(pair.Key, pair.Value.GetTotalCost());
            }
            return cost;
        }

        public MovingModel GetMovingModel()
        {
            var model = new MovingModel();
            var list = (TransportationList)listDic_[ContextListType.TransportationList];
            model.Set(list.GetTotalDistance(), list.GetTotalTime());
            return model;
        }

        // control
        public ControlModel GetControlModel()
        {
            return controllModel_;
        }

        public AccommodationList GetAccommodationList()
        {

            return (AccommodationList)listDic_[ContextListType.AccommodationList];
        }


        public TransportationList GetTransportationList()
        {
            return (TransportationList)listDic_[ContextListType.TransportationList];
        }



        public SightseeingList GetSightseeingList()
        {
            return (SightseeingList)listDic_[ContextListType.SightseeingList];
        }

        public OtherList GetOtherList()
        {
            return (OtherList)listDic_[ContextListType.Other];
        }

        public ExchangeRater GetExchanger()
        {
            return exchangeRater_;
        }
    }




}
