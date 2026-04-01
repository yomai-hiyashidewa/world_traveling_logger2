using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldTravelLogger.Models;
using WorldTravelLogger.Models.Context;
using WorldTravelLogger.Models.Enumeration;
using WorldTravelLogger.Models.Interface;
using WorldTravelLogger.Models.List;
using WorldTravelLogger.ViewModels.Base;

namespace WorldTravelLogger.ViewModels
{
    public class SightseeingViewModel : BaseContextListViewModel
    {
        SightseeingList list_;



        public SightseeingViewModel(SightseeingList list, ControlModel control):
             base(control)
        {
            list_ = list;
            list_.ListChanged += List__ListChanged;
            
        }

        private void List__ListChanged(object? sender, EventArgs e)
        {
            UpdateAll();
        }




        private void UpdateAll()
        {
            this.RaisePropertyChanged("TypeSightseeings");
            this.RaisePropertyChanged("CurrentSightseeingTypes");
            this.RaisePropertyChanged("CurrentSightseeingType");
            this.RaisePropertyChanged("EnableCurrentSightseeingType");
            this.RaisePropertyChanged("Sightseeings");

        }



        public SightseeingTypeModel[] TypeSightseeings
        {
            get
            {
                return list_.TypeSightseeings;
            }
        }





        public SightseeigType CurrentSightseeingType
        {
            get
            {
                return list_.CurrentSightseeingType;
            }
            set
            {
                if (list_.CurrentSightseeingType != value)
                {
                    list_.CurrentSightseeingType = value;
                    this.RaisePropertyChanged("Sightseeings");
                }
            }
        }

        public bool EnableCurrentSightseeingType
        {
            get
            {
                return CurrentSightseeingTypes.Count() > 0;
            }
        }

        public SightseeigType[] CurrentSightseeingTypes
        {
            get
            {
                return list_.CurrentSightseeingTypes;
            }
        }

       


        public SightseeingModel[] Sightseeings
        {
            get
            {
                return list_.GetCalcs(control_.IsCountryRegion).OfType<SightseeingModel>().
                   Where(m => m.SightseeigType == list_.CurrentSightseeingType).ToArray();
            }
        }


    }
}
