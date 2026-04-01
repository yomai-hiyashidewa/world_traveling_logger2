using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WorldTravelLogger.Views.Converters
{
    class EnumDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DisplayAttribute attr = field.GetCustomAttribute<DisplayAttribute>();
            if (attr != null)
            {
                return attr.Name;
            }
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
