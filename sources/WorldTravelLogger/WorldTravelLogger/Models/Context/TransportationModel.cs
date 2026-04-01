using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WorldTravelLogger.Models.Base;
using WorldTravelLogger.Models.Enumeration;

namespace WorldTravelLogger.Models.Context
{
    public class TransportationModel : BaseContext
    {
        private Transportationtype transportationtype_; // 交通機関
        private PlaceType startPlace_;                  // 開始場所
        private DateTime endDate_;                      // 終了日時
        private CountryType endCountry_;                // 終了国
        private string? endRegion_;                     // 終了地域
        private PlaceType endPlace_;                    // 終了場所
        private double distance_;                       // 移動距離(km)
        private int time_;                              // 移動時間(min)

        public TransportationModel() :
            base()
        {
            transportationtype_ = Transportationtype.Train;
            startPlace_ = PlaceType.Station;                  // 開始場所
            endDate_ = DateTime.Now;                         // 終了日時
            endCountry_ = CountryType.JPN;                  // 終了国
            endRegion_ = null;                              // 終了地域
            endPlace_ = PlaceType.Station;                  // 終了場所
            distance_ = 0.0;                                // 移動距離(km)
            time_ = 0;                                      // 移動時間(min)
        }

        public TransportationModel(Transportationtype transportationtype,
            DateTime startDate,
            CountryType startCountry,
            string? startRegion,
            PlaceType startPlace,
            DateTime endDate,
            CountryType endCountry,
            string? endRegion,
            PlaceType endPlace,
            double distance,
            int time,
            double price,
            CurrencyType currency,
            string? memo) :
           base(startDate, startCountry, startRegion, price, currency, memo)
        {
            transportationtype_ = transportationtype;
            startPlace_ = startPlace;                  // 開始場所
            endDate_ = endDate;                         // 終了日時
            endCountry_ = endCountry;                  // 終了国
            endRegion_ = ConvertUpperStringOnlyTop(endRegion); // 終了地域
            endPlace_ = endPlace;                  // 終了場所
            distance_ = distance;                                // 移動距離(km)
            time_ = time;                                      // 移動時間(min)
            ResetType();
        }



        // 記述内容を更に分類する
        private void ResetType()
        {
            if (transportationtype_ == Transportationtype.Train)
            {
                if (distance_ <= 10)
                {
                    transportationtype_ = Transportationtype.LocalTrain;
                }
                else if (distance_ <= 100)
                {
                    transportationtype_ = Transportationtype.MiddleDistanceTrain;
                }
                else
                {
                    transportationtype_ = Transportationtype.LongDistanceTrain;
                }
            }
            else if (transportationtype_ == Transportationtype.Bus)
            {
                if (distance_ <= 10)
                {
                    transportationtype_ = Transportationtype.LocalBus;
                }
                else if (distance_ <= 100)
                {
                    transportationtype_ = Transportationtype.MiddleDistanceBus;
                }
                else
                {
                    transportationtype_ = Transportationtype.LongDistanceBus;
                }
            }
        }



        public Transportationtype Transportationtype { get { return transportationtype_; } } // 交通機関

        public DateTime StartDate
        {
            get
            {
                return base.Date;
            }
        }

        public CountryType StartCountry
        {
            get
            {
                return base.Country;
            }
        }

        public string? StartRegion
        {
            get
            {
                return base.Region;
            }
        }

        public PlaceType StartPlace
        {
            get
            {
                return startPlace_;
            }
        }                  // 開始場所

        public DateTime EndDate
        {
            get
            {
                return endDate_;
            }
        }

        // 終了日時

        public bool IsSameDate
        {
            get
            {
                return StartDate.Year == EndDate.Year &&
                    StartDate.Month == EndDate.Month &&
                    StartDate.Day == EndDate.Day;
            }
        }

        public string EndDateString
        {
            get
            {
                return base.GetDataString(endDate_);
            }
        }

        public CountryType EndCountry
        {
            get
            {
                return endCountry_;
            }
        }                // 終了国

        public string? EndRegion
        {
            get
            {
                return endRegion_;
            }
        }                     // 終了地域

        public PlaceType EndPlace
        {
            get
            {
                return endPlace_;

            }
        }                    // 終了場所

        public double Distance
        {
            get { return distance_; }
        }// 移動距離(km)

        public int Time
        {
            get { return time_; }
        }
        // 移動時間(min)

        public MovingModel GetMovingMoidel()
        {
            var moving = new MovingModel();
            moving.Set(distance_, time_);
            return moving;

        }

        public string DistanceAndTimeStr
        {
            get
            {
                var moving = GetMovingMoidel();
                return moving.Distance + "," + moving.Time;
            }
        }

        public bool SameCountry
        {
            get
            {
                return StartCountry == EndCountry;
            }

        }

        public bool AnotherCountry
        {
            get
            {
                return StartCountry != EndCountry;
            }

        }

        public bool AnotherDate
        {
            get
            {
                return StartDate != EndDate;
            }
        }

        public bool IsSameCountry(CountryType type)
        {
            return StartCountry == type ||
                EndCountry == type;
        }

        public bool SameRegion
        {
            get
            {
                return StartRegion == EndRegion;
            }
        }

        public bool SameDate
        {
            get
            {
                return StartDate == EndDate;
            }
        }

        public string GetRegion(CountryType type)
        {
            if (type == StartCountry)
            {
                return StartRegion;
            }
            else if (type == EndCountry)
            {
                return EndRegion;
            }
            else
            {
                return null;
            }
        }

        public bool IsSameStartRegion(string region)
        {
            return StartRegion == region;
        }

        public bool IsSameEndRegion(string region)
        {
            return EndRegion == region;
        }

        public bool IsSameRegion(string region)
        {
            return IsSameStartRegion(region) ||
                IsSameEndRegion(region);
        }

        public bool IsDeparture(CountryType type)
        {
            return !SameCountry && StartCountry == type;
        }

        // for no entry

        public bool IsNoEntry
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Memo) && Memo == "NO_ENTRY";
            }
        }

        // for route

        public bool IsRouteArrival
        {
            get
            {
                return AnotherCountry && startPlace_ != PlaceType.Dep;
            }
        }

        public bool IsRouteDeparture
        {
            get
            {
                return AnotherCountry && startPlace_ == PlaceType.Dep;
            }
        }

        public TransportationModel EndClone()
        {
            var cloneModel = (TransportationModel)this.MemberwiseClone();
            cloneModel.startPlace_ = PlaceType.Dep;
            return cloneModel;
        }


    }
}
