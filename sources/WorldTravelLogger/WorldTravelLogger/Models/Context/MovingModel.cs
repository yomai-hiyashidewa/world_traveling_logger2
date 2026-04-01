using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldTravelLogger.Models.Context
{
    public class MovingModel
    {
        private double distance_;
        private int time_;

        public MovingModel()
        {
           
        }

        public void Set(double distance, int time)
        {
            distance_ = distance;
            time_ = time;
        }


        public string Distance
        {
            get
            {
                return string.Format("{0:#,0}", distance_) + "km";
            }
        }

        public string Time
        {
            get
            {
                int hours = time_ / 60;
                int min = time_ - (60 * hours);
                int days = hours / 24;
                hours = hours - (24 * days);
                if(days > 0)
                {
                    return string.Format("{0}d {1}h {2}m", days, hours, min);
                }
                else if(hours > 0)
                {
                    return string.Format("{0}h {1}m", hours, min);
                }
                else
                {
                    return string.Format("{0}min", min);
                }
               

            }
        }
    }
}
