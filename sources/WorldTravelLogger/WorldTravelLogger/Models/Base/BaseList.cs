using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldTravelLogger.Models.Context;
using WorldTravelLogger.Models.Csv;
using WorldTravelLogger.Models.Enumeration;
using WorldTravelLogger.Models.Interface;

namespace WorldTravelLogger.Models.Base
{
    public abstract  class BaseList
    {
       
        public List<FileErrorContext> ErrorList
        {
            get;
            private set;
        }

        public abstract bool IsLoaded { get; }
        
        protected BaseList()
        {
            ErrorList = new List<FileErrorContext>(); 
        }

        public FileErrorContext[] GetErrorArray()
        {
            return ErrorList.ToArray();
        }


        protected void SetErrorList(int i, int j,string context)
        {
            
            ErrorList.Add(new FileErrorContext(i,j,context));
        }

        public bool IsError
        {
            get { return ErrorList.Count != 0; }
        }

        protected DateTime? ConvertDate(string str)
        {
            DateTime date;
            if (DateTime.TryParse(str, out date))
            {
                return date; ;
            }
            else
            {
                return null;
            }
        }

        protected CurrencyType? ConvertCurrency(string str)
        {
            CurrencyType type;
            if (Enum.TryParse(str, out type))
            {
                return type;
            }
            else
            {
                return null;
            }
        }

        protected CountryType? ConvertCountry(string str)
        {
            CountryType type;
            if (Enum.TryParse(str, out type))
            {
                return type;
            }
            else
            {
                return null;
            }
        }

        protected AccommodationType? ConvertAccommodationType(string str)
        {
            AccommodationType type;
            if (Enum.TryParse(str, out type))
            {
                return type;
            }
            else
            {
                return null;
            }
        }

        protected Transportationtype? ConvertTransportationType(string str)
        {
            Transportationtype type;
            if(Enum.TryParse(str, out type))
            {
                return type;
            }
            else
            {
                return null;
            }
        }

        protected SightseeigType? ConvertSightseeingType(string str)
        {
            SightseeigType type;
            if(Enum.TryParse(str,out type))
            {
                return type;
            }
            else
            {
                return null;
            }
        }

        protected OtherType? ConvertOtherType(string typeStr)
        {
            OtherType type;
            if (Enum.TryParse(typeStr, out type))
            {
                return type;
            }
            else
            {
                return null;
            }
        }

        protected PlaceType? ConvertPlaceType(string str)
        {
            PlaceType type;
            if(Enum.TryParse(str, out type))
            {
                return type;
            }
            else
            {
                return null;
            }
        }

        protected double? ConvertDouble(string str)
        {
            double val;
            if (double.TryParse(str, out val))
            {
                return val;
            }
            else
            {
                return null;
            }
        }

        protected int? ConvertInt(string str)
        {
            int val;
            if(int.TryParse(str, out val))
            {
                return val;
            }
            else
            {
                return null;
            }
        }

        protected abstract bool CheckFormat(object[] arrays);

        protected abstract void Set(object[] arrays);

       

        public virtual void Init()
        {
            ErrorList.Clear();
        }

        public virtual ErrorTypes Load(string filePath,string checkFilename)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return ErrorTypes.None;
            }
            if (!File.Exists(filePath))
            {
                return ErrorTypes.FileNotFound;
            }

            var filename = Path.GetFileNameWithoutExtension(filePath);
            if (!string.Equals(filename, checkFilename))
            {
                return ErrorTypes.FileWrong;
            }
            var result = CSVReader.ReadCSV(filePath);
            if (result == null)
            {
                return ErrorTypes.FileNotOpen;
            }
            if (CheckFormat(result))
            {
                return ErrorTypes.FormatError;
            }

            if (!this.IsError)
            {
                this.Set(result);
            }
            return ErrorTypes.None;
        }

       


       

     

      


        



       
        

        
       

       

        

        
    }

    


   
}
