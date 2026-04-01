using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using WorldTravelLogger.Models;
using WorldTravelLogger.Models.Context;
using WorldTravelLogger.Models.Enumeration;
using WorldTravelLogger.Models.Utility;
using WorldTravelLogger.ViewModels.Base;

namespace WorldTravelLogger.ViewModels
{
    public class SideViewModel : ViewModelBase
    {
        private MainModel? model_;
        private ControlModel control_;

        private CountryListViewModel clVM_;


        public SideViewModel(MainModel model)
        {
            model_ = model;
            control_ = model.GetControlModel();
            clVM_ = new CountryListViewModel();
            clVM_.CountryChanged += ClVM__CountryChanged;
            model_.CalcCompleted_ += Model__CalcCompleted_;
            model_.ImageListReady_ += Model__ImageListReady_;
        }

        private void Control__CountryChanged_(object? sender, EventArgs e)
        {
            this.RaisePropertyChanged("CountryImagePath");
        }

        private void ClVM__CountryChanged(object? sender, CountryChangedEventArgs e)
        {
            //control_.CurrentRouteCountryType = e.Type;
        }

        public CountryListViewModel GetCountryListViewModel()
        {
            return clVM_;
        }

        private void Model__CalcCompleted_(object? sender, EventArgs e)
        {
            UpdateView();
            SetCountries();
        }

        private void Model__ImageListReady_(object? sender, EventArgs e)
        {
            SetCountries();
        }

        private void SetCountries()
        {
            clVM_.SetCountries(model_.GetCountries(), model_.ImageDir);
        }


        private void UpdateView()
        {
            this.RaisePropertyChanged("IsWorld");
            this.RaisePropertyChanged("IsWorldView");
            this.RaisePropertyChanged("IsCountryMode");
            this.RaisePropertyChanged("IsWithJapan");
            this.RaisePropertyChanged("IsOnlyBorder");
            this.RaisePropertyChanged("IsRegion");
            this.RaisePropertyChanged("IsNotRegion");

            this.RaisePropertyChanged("CurrentCountry");
            this.RaisePropertyChanged("IsWithCrossBorder");
            this.RaisePropertyChanged("StartDate");
            this.RaisePropertyChanged("EndDate");

            this.RaisePropertyChanged("Movings");
            this.RaisePropertyChanged("Countries");
           
            this.RaisePropertyChanged("TotalCalcCountries");
            //this.RaisePropertyChanged("TotalCountries");

            this.RaisePropertyChanged("RegionsCount");
            this.RaisePropertyChanged("Regions");
            this.RaisePropertyChanged("ExchangeRates");
            this.RaisePropertyChanged("CountryFlagPath");
            this.RaisePropertyChanged("CountryImagePath");


        }

        public bool IsWorld
        {
            get
            {
                return !control_.IsWorldMode;

            }
            set
            {
                control_.IsWorldMode = !value;
            }
        }

        public bool IsWorldView
        {
            get
            {
                return control_.IsWorldMode;
            }
        }




        public bool IsCountryMode
        {
            get
            {
                return !control_.IsWorldMode;
            }
        }

        public bool IsWithJapan
        {
            get
            {
                return control_.IsWithJapan;
            }
            set
            {
                control_.IsWithJapan = value;
            }
        }

        public bool IsOnlyBorder
        {
            get
            {
                return control_.IsOnlyBorder;
            }
            set
            {
                control_.IsOnlyBorder = value;
            }
        }

        public bool IsRegion
        {
            get
            {
                return control_.IsRegion;
            }
            set
            {
                if (value)
                {
                    model_.SetCurrentRegion();
                }
                control_.IsRegion = value;
            }
        }

        public bool IsNotRegion
        {
            get
            {
                return !control_.IsRegion;
            }
        }


        public CountryType CurrentCountry
        {
            get
            {
                return control_.CurrentCountryType;

            }
            set
            {
                control_.CurrentCountryType = value;
            }
        }

        public string CurrentRegion
        {
            get
            {
                return control_.CurrentRegion;
            }
            set
            {
                control_.CurrentRegion = value;
            }
        }

        public bool IsWithCrossBorder
        {
            get
            {
                if (control_ == null)
                {
                    return false;
                }
                else
                {
                    return control_.IsWithCrossBorder;
                }
            }
            set
            {
                control_.IsWithCrossBorder = value;
            }

        }


        public CountryType[] Countries
        {
            get
            {
                var list = new List<CountryType>();
                list.AddRange(model_.GetCountries());
                return list.ToArray();
            }
        }

        public string[] Regions
        {
            get
            {
                if (model_ == null)
                {
                    return [];
                }
                else
                {
                    return model_.GetCurrentRegions();
                }
            }
        }

        public string RegionsCount
        {
            get
            {
                var count = 0;
                if (model_ == null)
                {
                    count = 0;
                }
                else
                {
                    if (control_.IsWorldMode)
                    {
                        count = model_.GetTotalRegionCount();
                    }
                    else
                    {
                        count = model_.GetCurrentRegionCount();
                    }
                }
                return count + " regions";
            }
        }

        public ExchangeRateModel[] ExchangeRates
        {
            get
            {
                if (model_ == null)
                {
                    return [];
                }
                else
                {
                    return model_.GetCurrentExchangeRates();
                }
            }
        }



        public string TotalCalcCountries
        {
            get
            {
                if (model_ == null)
                {
                    return "0";
                }
                else
                {
                    return model_.GetTotalSetCountries() + " countries"; ;
                }
            }
        }

        public string TotalCountries
        {
            get
            {
                if (model_ == null)
                {
                    return "0";
                }
                else
                {
                    return model_.GetCountries().Count() + " countries";
                }
            }
        }



        public MovingModel[] Movings
        {
            get
            {
                if (model_ == null)
                {
                    return [];
                }
                else
                {
                    var list = new List<MovingModel>();
                    list.Add(model_.GetMovingModel());
                    return list.ToArray();
                }
            }
        }

        public string CountryFlagPath
        {
            get
            {
                var cm = new CountryViewModel(control_.CurrentCountryType, model_.ImageDir);
                return cm.ImagePath;
            }
        }

        public string CountryImagePath
        {
            get
            {

                var imageDir = model_.ImageDir;
                if (Path.Exists(imageDir))
                {
                    if (control_.IsWorldMode)
                    {
                        return Path.Combine(imageDir, "Countries", FileNames.ZeroImageFile);
                    }
                    else
                    {
                        return Path.Combine(imageDir, "Countries", control_.CurrentCountryType.ToString(), FileNames.ZeroImageFile);
                    }
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
