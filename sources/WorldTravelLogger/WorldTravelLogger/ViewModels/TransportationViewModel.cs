using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using WorldTravelLogger.Models;
using WorldTravelLogger.Models.Context;
using WorldTravelLogger.Models.Enumeration;
using WorldTravelLogger.Models.Interface;
using WorldTravelLogger.Models.List;
using WorldTravelLogger.ViewModels.Base;

namespace WorldTravelLogger.ViewModels
{
    public class TransportationViewModel : BaseContextListViewModel
    {
        TransportationList list_;


        public TransportationViewModel(TransportationList list, ControlModel control) :
            base(control)
        {
            list_ = list;
            list.ListChanged += List_ListChanged;


        }

        private void List_ListChanged(object? sender, EventArgs e)
        {
            UpdateAll();
        }


        private void UpdateAll()
        {
            this.RaisePropertyChanged("TypeTransportations");
            this.RaisePropertyChanged("CurrentTransportationTypes");
            this.RaisePropertyChanged("CurrentTransportationType");
            this.RaisePropertyChanged("EnableCurrentTransportationType");
            this.RaisePropertyChanged("Transportations");
        }

        public TransportationTypeModel[] TypeTransportations
        {
            get
            {
                return list_.TypeTransportations;
            }
        }



        public Transportationtype CurrentTransportationType
        {
            get
            {
                return list_.CurrentTransportationType;
            }
            set
            {
                if (list_.CurrentTransportationType != value)
                {
                    list_.CurrentTransportationType = value;
                    this.RaisePropertyChanged("Transportations");
                }
            }
        }

        public bool EnableCurrentTransportationType
        {
            get
            {
                return CurrentTransportationTypes.Count() > 0;
            }
        }

        public Transportationtype[] CurrentTransportationTypes
        {
            get
            {
                return list_.CurrentTransportationTypes;
            }
        }

       



        public TransportationModel[] Transportations
        {
            get
            {
                return list_.GetCalcs(control_.IsCountryRegion).OfType<TransportationModel>().
                    Where(m => m.Transportationtype == list_.CurrentTransportationType).ToArray();
            }
        }






    }
}
