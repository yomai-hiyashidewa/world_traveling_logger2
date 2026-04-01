using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldTravelLogger.Models.Enumeration;
using WorldTravelLogger.Models.Utility;
using WorldTravelLogger.ViewModels.Base;

namespace WorldTravelLogger.ViewModels
{
    public  class CountryListViewModel : ViewModelBase
    {
        private List<CountryViewModel> list_;

      
        private CountryViewModel currentCountry_;

        public event EventHandler<CountryChangedEventArgs> CountryChanged;
        
        public void SetCountries(IEnumerable<CountryType> countries,string imagePath)
        {
            list_.Clear();
            SetList(countries, imagePath);
            this.RaisePropertyChanged("Countries");
            this.RaisePropertyChanged("CurrentCountry");
        }

        private void SetList(IEnumerable<CountryType> countries, string imagePath)
        {
            foreach (var c in countries)
            {
                var cm = new CountryViewModel(c, imagePath);
                list_.Add(cm);
            }
            currentCountry_ = list_.FirstOrDefault();
           
        }


        public CountryListViewModel()
        {
            list_ = new List<CountryViewModel>();
        }

        public CountryViewModel[] Countries
        {
            get
            {
                return list_.ToArray();
            }
        }

        public CountryViewModel CurrentCountry
        {
            get { return currentCountry_; }
            set
            {
                if(currentCountry_ != value)
                {
                    currentCountry_ = value;
                    if(CountryChanged != null && currentCountry_ != null)
                    {
                        CountryChanged(this, new CountryChangedEventArgs(currentCountry_.Type,list_.IndexOf(currentCountry_)));
                    }
                }
            }
        }
    }
}
