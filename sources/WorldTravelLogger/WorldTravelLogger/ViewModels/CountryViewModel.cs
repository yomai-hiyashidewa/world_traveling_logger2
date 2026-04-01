using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldTravelLogger.Models.Enumeration;

namespace WorldTravelLogger.ViewModels
{
    public class CountryViewModel
    {
        private CountryType countryType_;

        private string imageDir_;

        public CountryViewModel(CountryType countryType, string imageDir)
        {
            countryType_ = countryType;
            imageDir_ = imageDir;
        }

        public CountryType Type { get { return countryType_; } }

        public string ImagePath
        {
            get
            {
                if (EnableImage)
                {
                    var path = Path.Combine(imageDir_, "Flags", countryType_.ToString() + ".png");
                    if (File.Exists(path))
                    {
                        return path;
                    }
                }
                return null;
            }
        }

        public bool EnableImage
        {
            get
            {
                return !string.IsNullOrWhiteSpace(imageDir_) && Path.Exists(imageDir_);
            }
        }

    }
}
