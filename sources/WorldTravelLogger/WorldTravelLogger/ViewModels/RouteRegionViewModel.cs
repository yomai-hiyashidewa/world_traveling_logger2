using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldTravelLogger.Models;
using WorldTravelLogger.Models.Context;
using WorldTravelLogger.Models.Enumeration;
using WorldTravelLogger.Models.List;
using WorldTravelLogger.ViewModels.Base;

namespace WorldTravelLogger.ViewModels
{
    public class RouteRegionViewModel : ViewModelBase
    {
        ControlModel control_;
        MainModel model_;
        TransportationList transportationList_;

        CountryViewModel cVM_;

        public RouteRegionMiniViewModel MiniVM { get; private set; }

        public RouteRegionViewModel(MainModel model)
        {
            model_ = model;
            model.CalcCompleted_ += Model_CalcCompleted_;
            control_ = model.GetControlModel();
            transportationList_ = model.GetTransportationList();
            MiniVM = new RouteRegionMiniViewModel(model);
            SetCVM();
        }

        private void SetCVM()
        {
            cVM_ = new CountryViewModel(control_.CurrentCountryType, model_.ImageDir);
        }

        private void Model_CalcCompleted_(object? sender, EventArgs e)
        {
            SetCVM();
            UpdateAll();
        }

        private void Control__CountryChanged_(object? sender, EventArgs e)
        {
            SetCVM();
            UpdateAll();
        }

        private void UpdateAll()
        {
            this.RaisePropertyChanged("Type");
            this.RaisePropertyChanged("EnableImage");
            this.RaisePropertyChanged("CountryFlagPath");
            this.RaisePropertyChanged("StartDate");
            this.RaisePropertyChanged("EndDate");
        }

        public string StartDate
        {
            get
            {
                var date = transportationList_.GetRoute(control_.CurrentCountryType).FirstOrDefault();
                if(date != null)
                {
                    return date.EndDateString;
                }
                else
                {
                    return "";
                }
            }
        }

        public string EndDate
        {
            get
            {
                var date = transportationList_.GetRoute(control_.CurrentCountryType).LastOrDefault();
                if (date != null)
                {
                    return date.DateString;
                }
                else
                {
                    return "";
                }
            }
        }

        public CountryType Type
        {
            get
            {
                return cVM_.Type;
            }
        }

        public bool EnableImage
        {
            get
            {
                return cVM_.EnableImage;
            }
        }

        public string CountryFlagPath
        {
            get
            {
                return cVM_.ImagePath;
            }
        }
    }
}
