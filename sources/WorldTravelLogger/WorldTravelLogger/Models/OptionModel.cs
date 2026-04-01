using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using WorldTravelLogger.Models.Enumeration;
using WorldTravelLogger.Models.Utility;

namespace WorldTravelLogger.Models
{
    public class OptionModel
    {

        private string? listPath_;
        private string? imagePath_;

        public event EventHandler<FileLoadedEventArgs> ReqLoad;

       
        public OptionModel()
        {
            Assembly myAssembly = Assembly.GetEntryAssembly();
            var path = myAssembly.Location;
            var dir = Path.GetDirectoryName(path);
            listPath_ = Path.Combine(dir, FileNames.ListDir);
            imagePath_ = Path.Combine(dir, FileNames.ImageDir);
            
        }

        private void FireFileReqLoad(ListType type)
        {
            if (ReqLoad != null)
            {
                ReqLoad.Invoke(this,new FileLoadedEventArgs(type, ErrorTypes.None));
            }

        }

        private string GetListPath(ListType type)
        {
            return Path.Combine(listPath_, FileNames.GetFileName(type)+".csv");
        }




        public string? AccommodationPath
        {
            get { return GetListPath(ListType.AccommodationList); }
        }

        public string? TransportationPath
        {
            get { return GetListPath(ListType.TransportationList); }
        }

        public string? SightseeingPath
        {
            get { return GetListPath(ListType.SightseeingList); }
        }

        public string? ExchangeRatePath
        {
            get { return GetListPath(ListType.ExchangeRateList); }
        }

        

       

        public string? ImagePath
        {
            get
            {
                return imagePath_;
            }
            set
            {
                if(imagePath_ != value)
                {
                    imagePath_ = value;
                    FireFileReqLoad(ListType.ImageList);
                }
            }
        }

        public string? ListPath
        {
            get { return listPath_; }
            set
            {
                if(listPath_ != value)
                {
                    listPath_ = value;
                    FireFileReqLoad(ListType.AccommodationList);
                    FireFileReqLoad(ListType.TransportationList);
                    FireFileReqLoad(ListType.SightseeingList);
                    FireFileReqLoad(ListType.ExchangeRateList);
                }
            }
        }

        public void Load(ListType type)
        {
            FireFileReqLoad(type);
        }

       

        public object[] GetSaveData()
        {
            var list = new List<string[]>();

            for (var i = 0; i < 3; i++)
            {
                var row = new List<string>();
                switch (i)
                {
                    case 0:
                        row.Add("files");
                        row.Add("path");
                        break;
                    case 1:
                        row.Add(FileNames.ListDir);
                        row.Add(listPath_);
                        break;
                    case 2:
                        row.Add(FileNames.ImageDir);
                        row.Add(imagePath_);
                        break;
                }
                list.Add(row.ToArray());
            }
            return list.ToArray();
        }

        public bool Load(object[] arrays)
        {
            for (var i = 1; i < 3; i++)
            {
                string[] row = (string[])arrays[i];
                if (row.Length > 1)
                {
                    switch (i)
                    {
                        case 1:
                            listPath_ = row[1];
                            break;
                        case 2:
                            imagePath_ = row[1];
                            break;
                    }
                }
            }
            return true;
        }

        public string GetFilePath(ListType type)
        {
            string path = null;
            switch (type)
            {
                case ListType.AccommodationList:
                case ListType.TransportationList:
                case ListType.SightseeingList:
                case ListType.ExchangeRateList:
                    path = GetListPath(type);
                    break;
                case ListType.ImageList:
                    path = imagePath_;
                    break;
            }
            return path;
        }

        
    }
}
