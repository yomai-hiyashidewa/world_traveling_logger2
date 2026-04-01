using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using WorldTravelLogger.Models;
using WorldTravelLogger.Models.Base;
using WorldTravelLogger.Models.Context;
using WorldTravelLogger.Models.Enumeration;
using WorldTravelLogger.Models.Interface;
using WorldTravelLogger.Models.List;
using WorldTravelLogger.ViewModels.Base;

namespace WorldTravelLogger.ViewModels
{
    public class AccommodationViewModel : BaseContextListViewModel
    {
        private AccommodationList list_;
        


        public AccommodationViewModel(AccommodationList list, ControlModel control): 
            base(control)
        {
            list_ = list;
            list_.ListChanged += List_ListChanged;
        }



        private void List_ListChanged(object? sender, EventArgs e)
        {
            this.UpdateAll();
        }

        private void UpdateAll()
        {
            this.RaisePropertyChanged("TypeAccommodations");
            this.RaisePropertyChanged("CurrentAccommodationTypes");
            this.RaisePropertyChanged("CurrentAccommodationType");
            this.RaisePropertyChanged("EnableCurrentAccommodationType");
            this.RaisePropertyChanged("TypeAccommodations");
            this.RaisePropertyChanged("Accommodations");


        }      

        public AccommodationTypeModel[] TypeAccommodations
        {
            get
            {
                return list_.TypeAccommodations;
            }
        }

        public AccommodationType CurrentAccommodationType
        {

            get { return list_.CurrentAccommodationtype; }
            set
            {
                if (list_.CurrentAccommodationtype != value)
                {
                    list_.CurrentAccommodationtype = value;
                    this.RaisePropertyChanged("Accommodations");
                }

            }

        }

        public bool EnableCurrentAccommodationType
        {
            get
            {
                return CurrentAccommodationTypes.Count() > 0;
            }
        }

        public AccommodationType[] CurrentAccommodationTypes
        {
            get
            {
                return list_.CurrentAccommodationTypes;
            }
        }

        public AccommodationModel[] Accommodations
        {
            get
            {
                return list_.GetCalcs(control_.IsCountryRegion).OfType<AccommodationModel>().
                    Where(m => m.Accommodation == CurrentAccommodationType).ToArray();


            }
        }


    }



}
