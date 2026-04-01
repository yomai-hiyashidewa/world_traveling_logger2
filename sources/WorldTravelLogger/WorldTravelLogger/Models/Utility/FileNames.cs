using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldTravelLogger.Models.Enumeration;

namespace WorldTravelLogger.Models.Utility
{
    public static class FileNames
    {
        public const string ImageDir = "Image";
        public const string ListDir = "List";
        public const string AccommodationFile = "accommodations";
        public const string TransportationFile = "transportations";
        public const string SightseeingFile = "sightseeing";
        public const string ExchangeRateFile = "exchange_rates";
        public const string ZeroImageFile = "zero.jpg";
        public const string SAVE_FILE_NAME = "WorldTravelLogger.csv";

        public static string GetFileName(ListType type)
        {
            switch (type)
            {
                case ListType.AccommodationList:
                    return AccommodationFile;
                case ListType.TransportationList:
                    return TransportationFile;
                case ListType.SightseeingList:
                    return SightseeingFile;
                case ListType.ExchangeRateList:
                    return ExchangeRateFile;

            }
            return null;
        }

    }
}
