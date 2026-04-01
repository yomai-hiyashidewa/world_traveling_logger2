using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using WorldTravelLogger.Models;
using WorldTravelLogger.Models.Context;
using WorldTravelLogger.Models.Enumeration;
using WorldTravelLogger.Models.List;
using WorldTravelLogger.ViewModels.Base;

namespace WorldTravelLogger.ViewModels
{
    public class RouteRegionMiniViewModel : ViewModelBase
    {
        MainModel model_;
        ControlModel control_;
        TransportationList transportationList_;




        public RouteRegionMiniViewModel(MainModel model)
        {
            model_ = model;
            model.CalcCompleted_ += Model_CalcCompleted_;
            control_ = model.GetControlModel();
            transportationList_ = model.GetTransportationList();
        }


        private void Model_CalcCompleted_(object? sender, EventArgs e)
        {
            UpdateAll();
        }

        public TransportationModel[] Routes
        {
            get
            {
                var list = new List<TransportationModel>();
                foreach (var model in transportationList_.GetRoute(control_.CurrentCountryType))
                {
                    if (model.IsDeparture(control_.CurrentCountryType))
                    {
                        var endModel = model.EndClone();
                        list.Add(endModel);
                    }
                    else
                    {
                        list.Add(model);
                    }

                }
                return list.ToArray();
            }
        }
        private void UpdateAll()
        {
            this.RaisePropertyChanged("Routes");
        }

    }

}
