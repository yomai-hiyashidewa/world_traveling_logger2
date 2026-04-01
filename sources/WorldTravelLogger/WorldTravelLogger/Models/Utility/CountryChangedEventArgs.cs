using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldTravelLogger.Models.Enumeration;

namespace WorldTravelLogger.Models.Utility
{
    public class CountryChangedEventArgs : EventArgs
    {
        public CountryType Type { get; private set; }

        public int Index { get; private set; }


        public CountryChangedEventArgs(CountryType type, int index)
        {
            Type = type;
            Index = index;
        }
    }
}
