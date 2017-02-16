using SimpleDataGrid.ViewModel;
using System;
using System.Windows.Data;

namespace SimpleDataGrid.Converter
{
    public class SortDirectionToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var sortDirection = (HeaderFilterBaseModel.SortDirection)value;
            switch (sortDirection)
            {
                case HeaderFilterBaseModel.SortDirection.Unsorted:
                    return null;
                case HeaderFilterBaseModel.SortDirection.Ascending:
                    return true;
                case HeaderFilterBaseModel.SortDirection.Descending:
                    return false;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var v = (bool?)value;
            switch (v)
            {
                case null:
                    return HeaderFilterBaseModel.SortDirection.Unsorted;
                case true:
                    return HeaderFilterBaseModel.SortDirection.Ascending;
                case false:
                    return HeaderFilterBaseModel.SortDirection.Descending;
            }
            return HeaderFilterBaseModel.SortDirection.Unsorted;
        }
    }

}
