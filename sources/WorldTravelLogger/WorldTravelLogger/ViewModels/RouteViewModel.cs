using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldTravelLogger.Models;
using WorldTravelLogger.Models.Context;
using WorldTravelLogger.ViewModels.Base;
using WorldTravelLogger.Views.Parts;

namespace WorldTravelLogger.ViewModels
{
    public class RouteViewModel : ViewModelBase
    {
        private RouteCountryViewModel arrivalsViewModel_;
        private RouteCountryViewModel departuresViewModel_;
        private RouteRegionViewModel regionsViewModel_;

        public RouteViewModel(MainModel model)
        {
            arrivalsViewModel_ = new RouteCountryViewModel(true,model);
            departuresViewModel_ = new RouteCountryViewModel(false,model);
            regionsViewModel_ = new RouteRegionViewModel(model);
        }

       


        public RouteCountryViewModel GetRouteCountryViewModel(bool isArrive)
        {
            if (isArrive)
            {
                return arrivalsViewModel_;
            }
            else
            {
                return departuresViewModel_;
            }
        }

        public RouteRegionViewModel GetRegionsViewModel()
        {
            return regionsViewModel_;
        }

    }
}
