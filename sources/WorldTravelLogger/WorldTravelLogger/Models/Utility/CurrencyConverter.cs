using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using WorldTravelLogger.Models.Enumeration;

namespace WorldTravelLogger.Models.Utility
{
    static public class CurrencyConverter
    {
        private static string GetCurrencyStr(CurrencyType type)
        {
            string culture = "ja-JP";
            if (type == CurrencyType.USD)
            {
                culture = "en-US";
            }
            else if (type == CurrencyType.EUR)
            {
                culture = "fr-FR";
            }
            return culture;
        }

        public static string GetCurrencyStr(CurrencyType type, double price)
        {
            var cultureStr = CurrencyConverter.GetCurrencyStr(type);
            return price.ToString("C", CultureInfo.CreateSpecificCulture(cultureStr));
        }
    }
}
