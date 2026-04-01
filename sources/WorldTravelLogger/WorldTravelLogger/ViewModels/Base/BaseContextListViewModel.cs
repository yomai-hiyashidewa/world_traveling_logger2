using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldTravelLogger.Models.Interface;
using WorldTravelLogger.Models;

namespace WorldTravelLogger.ViewModels.Base
{
    public abstract class BaseContextListViewModel : ViewModelBase
    {
        protected ControlModel control_;
       

        public BaseContextListViewModel(ControlModel control_)
        {
            this.control_ = control_;
        }


       


      

        

       


    }
}
