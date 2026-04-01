using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldTravelLogger.Models.Interface;

namespace WorldTravelLogger.Models.Base
{
    public abstract class BaseTypeModel: ITypeModel
    {
        public BaseTypeModel(string type)
        {
            Type = type;
            Count = 0;
            TotalCost = 0;
            MaxCost = 0;
            MinCost = 0;
        }

        public void Set(double cost)
        {
            Count++;
            TotalCost += cost;
            if(MaxCost == 0 || MaxCost < cost)
            {
                MaxCost = cost;
            }
            if(MinCost == 0 || cost < MinCost)
            {
                MinCost = cost;
            }
        }

        public string Type { get; protected set; }

        public int Count { get; protected set; }

        public double TotalCost { get; protected set; }

        public double MaxCost { get; protected set; }

        public double MinCost { get; protected set; }

        public string TotalCostString
        {
            get
            {
                return TotalCost.ToString("C");
            }
        }

        public string MaxCostString
        {
            get
            {
                return MaxCost.ToString("C");
            }
        }

        public string MinCostString
        {
            get
            {
                return MinCost.ToString("C");
            }
        }

        public string AveCoastString
        {
            get
            {
                double ave = 0;
                if (Count > 0)
                {
                    ave = TotalCost / Count;
                }
                return ave.ToString("C");
            }
        }


    }
}
