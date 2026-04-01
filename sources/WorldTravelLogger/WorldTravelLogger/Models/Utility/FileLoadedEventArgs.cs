using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldTravelLogger.Models.Enumeration;

namespace WorldTravelLogger.Models.Utility
{
    public class FileLoadedEventArgs : EventArgs
    {
        public ListType Type { get; private set; }
        public ErrorTypes ErrorTypes { get; private set; }

        public FileLoadedEventArgs(ListType lType,ErrorTypes type)
        {
            Type = lType;
            ErrorTypes = type;
        }
    }
}
