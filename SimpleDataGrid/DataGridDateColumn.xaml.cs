using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SimpleDataGrid
{
    /// <summary>
    /// Interaction logic for DataGridDateColumn.xaml
    /// </summary>
    public partial class DataGridDateColumn : DataGridBoundColumn
    {
        public DataGridDateColumn()
        {
            InitializeComponent();
        }

        protected override void CancelCellEdit(FrameworkElement editingElement, object uneditedValue)
        {
            var dp = editingElement as DatePicker;
            if (dp != null)
            {
                dp.SelectedDate = DateTime.Parse(uneditedValue.ToString());
            }
        }
        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            var dp = new DatePicker { };
            dp.SetBinding(DatePicker.SelectedDateProperty, Binding);

            return dp;
        }

        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            var txt = new TextBlock { };
            txt.SetBinding(TextBlock.TextProperty, Binding);

            return txt;
        }

        protected override object PrepareCellForEdit(FrameworkElement editingElement, RoutedEventArgs editingEventArgs)
        {
            var dp = editingElement as DatePicker;
            if (dp != null)
            {
                var dt = dp.SelectedDate;
                if (dt.HasValue)
                    return dt.Value;
            }
            return DateTime.Today;
        }

        public class DateTimeConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                var date = (DateTime)value;

                return date.ToString(parameter.ToString());
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                var strValue = value.ToString();
                DateTime resultDateTime;

                return DateTime.TryParse(strValue, out resultDateTime) ? resultDateTime : value;
            }
        }
    }
}
