using SimpleDataGrid.ViewModel;
using System;
using System.Windows.Data;

namespace SimpleDataGrid.Converter
{
    public class ByteArrayToBase64TextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var byteArray = (byte[])value;
            return System.Convert.ToBase64String(byteArray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var base64String = (string)value;
            return System.Convert.FromBase64String(base64String);
        }
    }
}
