using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using WorldTravelLogger.Models;
using WorldTravelLogger.Models.Context;
using WorldTravelLogger.Models.Enumeration;
using WorldTravelLogger.Models.Interface;
using WorldTravelLogger.Models.List;
using WorldTravelLogger.ViewModels.Base;

namespace WorldTravelLogger.ViewModels
{
    public class OtherViewModel : BaseContextListViewModel
    {
        OtherList list_;

        

        public OtherViewModel(OtherList list,ControlModel control):
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
            this.RaisePropertyChanged("TypeOthers");
            this.RaisePropertyChanged("CurrentOtherTypes");
            this.RaisePropertyChanged("CurrentOtherType");
            this.RaisePropertyChanged("EnableCurrentOtherType");
            this.RaisePropertyChanged("Others");
           
        }

       
        private SightseeigType? ConvertType(string typeStr)
        {
            SightseeigType type;
            if (Enum.TryParse(typeStr, out type))
            {
                return type;
            }
            else
            {
                return null;
            }
        }

        


        public OtherTypeModel[] TypeOthers
        {
            get
            {
                return list_.TypeOthers;
            }
        }

       

        public OtherType CurrentOtherType
        {
            get
            {
                return list_.CurrentOtherType;
            }
            set
            {
                if (list_.CurrentOtherType != value)
                {
                    list_.CurrentOtherType = value;
                    this.RaisePropertyChanged("Others");
                }
            }
        }

        public bool EnableCurrentOtherType
        {
            get
            {
                return CurrentOtherTypes.Count() > 0;
            }
        }

        public OtherType[] CurrentOtherTypes
        {
            get
            {
                return list_.CurrentOtherTypes;
            }

        }

      

        public OtherModel[] Others
        {
            get
            {
                return list_.GetCalcs(control_.IsCountryRegion).OfType<OtherModel>().
                    Where(m => m.OtherType == list_.CurrentOtherType).ToArray();
            }
        }
    }
}
