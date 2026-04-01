using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldTravelLogger.Models;
using WorldTravelLogger.Models.Enumeration;
using WorldTravelLogger.Models.Utility;
using WorldTravelLogger.ViewModels.Base;

namespace WorldTravelLogger.ViewModels
{
    internal class MainViewPanelVM : ViewModelBase
    {
        private MainModel model_;
        private ControlModel control_;
        private RouteViewModel routeVM_;

        private int tabIndex_;

        public event EventHandler<FileLoadedEventArgs> FileLoaded_;

        public MainViewPanelVM()
        {
            model_ = new MainModel();
            control_ = model_.GetControlModel();
            routeVM_ = new RouteViewModel(model_);
            model_.CalcCompleted_ += Model__CalcCompleted_;
            control_.ControlChanged_ += Control__ControlChanged_;
            control_.RegionChanged_ += Control__RegionChanged_;
            model_.FileLoaded_ += Model__FileLoaded_;
        }

        private void Model__CalcCompleted_(object? sender, EventArgs e)
        {
            this.RaisePropertyChanged("SetDate");
        }

        private void Control__RegionChanged_(object? sender, EventArgs e)
        {
            this.RaisePropertyChanged("IsCountry");
            CheckTabIndex();
        }

        private void Control__ControlChanged_(object? sender, EventArgs e)
        {
            this.RaisePropertyChanged("IsCountry");
            this.RaisePropertyChanged("IsWithAirplane");
            this.RaisePropertyChanged("IsWithInsurance");
            CheckTabIndex();
        }


       private void CheckTabIndex()
        {
            if (!IsCountry && tabIndex_ > 3)
            {
                tabIndex_ = 0;
                this.RaisePropertyChanged("TabIndex");
            }
        }
        

        private void Model__FileLoaded_(object? sender, FileLoadedEventArgs e)
        {
            if (FileLoaded_ != null)
            {
                FileLoaded_.Invoke(sender, e);
            }
        }

        public int TabIndex
        {
            get
            {
                return tabIndex_;
            }
            set
            {
                if(tabIndex_ != value)
                {
                    tabIndex_ = value;
                }
            }
        }


        public bool IsWithAirplane
        {
            get
            {
                return control_.IsWithAirplane;

            }
            set
            {
                control_.IsWithAirplane = value;
            }
        }





        public bool IsWithInsurance
        {
            get
            {
                    return control_.IsWithInsurance;
            }
            set
            {
                control_.IsWithInsurance = value;
            }
        }

        public bool IsCountry
        {
            get
            {
                if (control_.IsWorldMode)
                {
                    return false;
                }
                else
                {
                    return !control_.IsRegion;
                }
            }
        }

        public string SetDate
        {
            get
            {
                var date = DateTime.Now;
                if (control_ != null && control_.StartSetDate != null &&control_.EndSetDate != null)
                {
                    StringBuilder sb = new StringBuilder();
                   
                    date = (DateTime)control_.StartSetDate;
                    sb.Append(date.ToString("yyyy/MM/dd"));
                    sb.Append("-");
                   date = (DateTime)control_.EndSetDate;
                    sb.Append(date.ToString("yyyy/MM/dd"));
                    return sb.ToString();
                }
                return "";
            }
        }

      

        public string FileVer
        {
            get
            {
                FileVersionInfo ver = FileVersionInfo.GetVersionInfo(
    System.Reflection.Assembly.GetExecutingAssembly().Location);
                return ver.FileVersion;
            }
        }

        public void Init()
        {
            model_.Init();
        }

        public void Exit()
        {
            model_.Exit();
        }

        public OptionWindowViewModel GetOptionWindowViewModel()
        {
            return new OptionWindowViewModel(model_.GetOptionModel());
        }

        public DebugWinViewModel GetDebugWinViewModel()
        {
            return new DebugWinViewModel(model_);
        }

        public UpperViewModel GetUpperViewModel()
        {
            return new UpperViewModel(model_);
        }

        public SideViewModel GetSideViewModel()
        {
            return new SideViewModel(model_);
        }

        public RouteViewModel GetRouteViewModel()
        {
            return routeVM_;
        }

        public AccommodationViewModel GetAccommodationViewModel()
        {
            return new AccommodationViewModel(model_.GetAccommodationList(), model_.GetControlModel());
        }

        public TransportationViewModel GetTransporationViewModel()
        {
            return new TransportationViewModel(model_.GetTransportationList(), model_.GetControlModel());
        }


        public SightseeingViewModel GetSightseeingViewModel()
        {
            return new SightseeingViewModel(model_.GetSightseeingList(), model_.GetControlModel());
        }

        public OtherViewModel GetOtherViewModel()
        {
            return new OtherViewModel(model_.GetOtherList(), model_.GetControlModel());
        }


        public string GetFilename(ListType type)
        {
            switch (type)
            {
                case ListType.AccommodationList:
                    return FileNames.AccommodationFile;
                case ListType.TransportationList:
                    return FileNames.TransportationFile;
                case ListType.SightseeingList:
                    return FileNames.SightseeingFile;
                case ListType.ExchangeRateList:
                    return FileNames.ExchangeRateFile;
                default:
                    return null;
            }
        }
    }
}
