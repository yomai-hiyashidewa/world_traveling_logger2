using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldTravelLogger.Models;
using WorldTravelLogger.Models.Context;
using WorldTravelLogger.ViewModels.Base;

namespace WorldTravelLogger.ViewModels
{
    public class UpperViewModel: ViewModelBase
    {
        private MainModel? model_;
        private ControlModel control_;
        public UpperViewModel()
        {
            // dummmy
        }

        public UpperViewModel(MainModel model)
        {
            model_ = model;
            control_ = model.GetControlModel();
            model_.CalcCompleted_ += Model__CalcCompleted_;
            
        }

        private void Model__CalcCompleted_(object? sender, EventArgs e)
        {
            UpdateView();
        }
       

        private void UpdateView()
        {
            this.RaisePropertyChanged("StartDate");
            this.RaisePropertyChanged("EndDate");

           
            this.RaisePropertyChanged("TotalDays");

            this.RaisePropertyChanged("Costs");
        }

        public DateTime StartDate
        {
            get
            {
                if (control_ == null)
                {
                    return DateTime.Now;
                }
                else
                {
                    return control_.StartDate;
                }
            }
            set
            {
                control_.StartDate = value;
            }
        }

        public DateTime EndDate
        {
            get
            {
                if (control_ == null)
                {
                    return DateTime.Now;
                }
                else
                {
                    return control_.EndDate;
                }
            }
            set
            {
                control_.EndDate = value;
            }
        }


       

        public string TotalDays
        {
            get
            {
                if (model_ == null)
                {
                    return "days";
                }
                else
                {
                    return model_.TotalCalcDays + " days";
                }
            }
        }


        //　2列目
        public CostModel[] Costs
        {
            get
            {
                if(model_ == null)
                {
                    return [];
                }
                else
                {
                    var cost = model_.GetCostModel();
                    var list = new List<CostModel>();
                    list.Add(cost);
                    return list.ToArray();
                }
            }
        }
    }
}
