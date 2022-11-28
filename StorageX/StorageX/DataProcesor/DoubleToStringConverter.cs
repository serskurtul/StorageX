using System;
using System.Globalization;
using Xamarin.Forms;

namespace StorageX.DataProcesor
{
    public class DoubleToStringConverter : IValueConverter
    {
        public DoubleToStringConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToString(value, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToDouble(value, culture);
        }
    }
}

