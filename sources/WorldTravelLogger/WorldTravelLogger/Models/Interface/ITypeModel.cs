using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldTravelLogger.Models.Interface
{
    public interface ITypeModel
    {
        public void Set(double cost);

        public string Type { get; }

        public int Count { get; }

        public double TotalCost { get; }

        public double MaxCost { get; }

        public double MinCost { get; }

        public string TotalCostString { get; }


        public string MaxCostString { get; }
        

        public string MinCostString { get; }
       

        public string AveCoastString { get; }
    }
}
