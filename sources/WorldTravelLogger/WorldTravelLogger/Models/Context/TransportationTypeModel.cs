using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using WorldTravelLogger.Models.Base;
using WorldTravelLogger.Models.Enumeration;

namespace WorldTravelLogger.Models.Context
{
    public class TransportationTypeModel : BaseTypeModel
    {

        private double totalDistance_;
        private int totalTime_;

        private double maxDistance_;

        private double minDistance_;

        private int maxTime_;

        private int minTime_;

        public string TotalDistance
        {
            get
            {
                return totalDistance_.ToString("F1") + " km";
            }
        }
        public string TotalTime
        {
            get
            {
                return totalTime_.ToString() + " min";
            }
        }

        public string MaxDistance
        {
            get
            {
                return maxDistance_.ToString("F1") + " km";
            }
        }

        public string MinDistance
        {
            get
            {
                return minDistance_.ToString("F1") + " km";
            }
        }

        public string MaxTime
        {
            get { return maxTime_.ToString() + " min"; }
        }

        public string MinTime
        {
            get { return minTime_.ToString() + " min"; }
        }

        public double GetTotalDistance()
        {
            return totalDistance_;
        }

        public int GetTotalTime()
        {
            return totalTime_;
        }

        public TransportationTypeModel(Transportationtype type) : 
            base(type.ToString())
        {
            totalDistance_ = 0.0;
            totalTime_ = 0;
            maxDistance_ = 0.0;
            minDistance_ = 0.0;
            maxTime_ = 0;
            minTime_ = 0;
        }

        public void SetParameter(double distance, int time)
        {
            totalDistance_ += distance;
            totalTime_ += time;
            if (maxDistance_ == 0 || maxDistance_ < distance)
            {
                maxDistance_ = distance;
            }
            if (minDistance_ == 0 || minDistance_ < MinCost)
            {
                minDistance_ = distance;
            }
            if (maxTime_ == 0 || maxTime_ < time)
            {
                maxTime_ = time;
            }
            if (minTime_ == 0 || minTime_ < time)
            {
                minTime_ = time;
            }
        }


        public string AveDistance
        {
            get
            {
                if(Count > 0)
                {
                    var ave = totalDistance_ / Count;
                    return ave.ToString("F1") + " km";
                }
                else
                {
                    return "0 km";
                }
            }
        }

        public string AveTime
        {
            get
            {
                if (Count > 0)
                {
                    var ave = totalTime_ / Count;
                    return ave.ToString("F1") + " min";
                }
                else
                {
                    return "0 min";
                }
            }
        }

    }
}
