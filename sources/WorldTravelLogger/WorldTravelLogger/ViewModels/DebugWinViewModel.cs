using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WorldTravelLogger.Models;
using WorldTravelLogger.Models.Enumeration;
using WorldTravelLogger.Models.List;
using WorldTravelLogger.Models.Utility;
using WorldTravelLogger.ViewModels.Base;

namespace WorldTravelLogger.ViewModels
{

    public class DebugWinViewModel : ViewModelBase
    {
        private MainModel model_;
        private AccommodationList accommodationList_;
        private TransportationList transportationList_;
        private SightseeingList sightseeingList_;
        private ExchangeRater exchangeRater_;

        public DebugWinViewModel(MainModel model)
        {
            model_ = model;
            accommodationList_ = model_.GetAccommodationList();
            transportationList_ = model_.GetTransportationList();
            sightseeingList_ = model_.GetSightseeingList();
            exchangeRater_ = model_.GetExchanger();
            model_.FileLoaded_ += Model__FileLoaded;
        }

        private void Model__FileLoaded(object? sender, FileLoadedEventArgs e)
        {
            switch (e.Type)
            {
                case ListType.AccommodationList:
                    this.RaisePropertyChanged("Accommodations");
                    break;
                case ListType.TransportationList:
                    this.RaisePropertyChanged("Transportations");
                    break;
                case ListType.SightseeingList:
                    this.RaisePropertyChanged("Sightseeings");
                    break;
                case ListType.ExchangeRateList:
                    this.RaisePropertyChanged("ExchangeRates");
                    break;
                default:
                    break;
            }
        }

        public void Delete()
        {
            model_.FileLoaded_ -= Model__FileLoaded;
        }

        public object[] Accommodations
        {
            get
            {
                if (accommodationList_.IsError)
                {
                    return accommodationList_.GetErrorArray();
                }
                else
                {
                    return accommodationList_.GetArray();
                }
            }
        }

        public object[] Transportations
        {
            get
            {
                if (transportationList_.IsError)
                {
                    return transportationList_.GetErrorArray();
                }
                else
                {
                    return transportationList_.GetArray();
                }
             
            }
        }

        public object[] Sightseeings
        {
            get
            {
                if (sightseeingList_.IsError)
                {
                    return sightseeingList_.GetErrorArray();
                }
                else
                {
                    return sightseeingList_.GetArray();
                }
            }
        }

        public object[] ExchangeRates
        {
            get
            {
                if (exchangeRater_.IsError)
                {
                    return exchangeRater_.GetErrorArray();
                }
                else
                {
                    return exchangeRater_.GetExchangeRates();
                }
            }
        }

        


    }
}
