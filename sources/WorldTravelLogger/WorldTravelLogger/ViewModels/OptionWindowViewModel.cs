using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using WorldTravelLogger.Models;
using WorldTravelLogger.Models.Enumeration;
using WorldTravelLogger.ViewModels.Base;

namespace WorldTravelLogger.ViewModels
{
    public class OptionWindowViewModel : ViewModelBase
    {
        private OptionModel model_;
        public OptionWindowViewModel(OptionModel model)
        {
            model_ = model;
        }

        private DelegateCommand loadCommmand_;
        public DelegateCommand LoadCommmand
        {
            get
            {
                return loadCommmand_
                 ?? (loadCommmand_ = new DelegateCommand(
                 () =>
                 {
                     model_.Load(CurrentListType);
                 }));
            }
        }

        public ListType[] ListTypes
        {
            get
            {

                var list = new List<ListType>();
                if (Path.Exists(model_.AccommodationPath))
                {
                    list.Add(ListType.AccommodationList);
                }
                if (Path.Exists(model_.TransportationPath))
                {
                    list.Add(ListType.TransportationList);
                }
                if (Path.Exists(model_.SightseeingPath))
                {
                    list.Add(ListType.SightseeingList);
                }
                if (Path.Exists(model_.ExchangeRatePath))
                {
                    list.Add(ListType.ExchangeRateList);
                }
                return list.ToArray();
            }
        }

        public ListType CurrentListType
        {
            get;
            set;
        }


        private string GetDirName(string path)
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                var dir1 = Directory.GetParent((path));
                var dir2 = Directory.GetParent(dir1.FullName);
                return Path.Combine(dir2.Name, dir1.Name, new DirectoryInfo(path).Name);
            }
            else
            {
                return null;
            }
        }

        public string? ImagePath
        {
            get
            {
                return GetDirName(model_.ImagePath);
            }
            set
            {
                model_.ImagePath = value;
                this.RaisePropertyChanged("ImagePath");
            }
        }

        public string? ListPath
        {
            get
            {
                return GetDirName(model_.ListPath);
            }
            set
            {
                model_.ListPath = value;
                this.RaisePropertyChanged("ListPath");
                this.RaisePropertyChanged("ListTypes");
                this.RaisePropertyChanged("CurrentListType");
            }
        }

    }
}
