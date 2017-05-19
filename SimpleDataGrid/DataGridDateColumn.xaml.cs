using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

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
            
            dp.PreviewKeyDown += dp_PreviewKeyDown;

            return dp;
        }

        private void dp_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //because the DatePicker textbox has set e.Handled=true when Enter keydown event,
            //show need manully raise the Enter keydown event for DataGrid to commit current edit and move to next row
            if (e.Key == Key.Enter)
            {
                var evt = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromVisual(this.DataGridOwner), 0, e.Key)
                {
                    RoutedEvent = Keyboard.KeyDownEvent
                };
                
                //don't know why the DataGrid will get one more event from DataGridCell (2 event fired, can check by override DataGrid OnKeyDown method)
                this.DataGridOwner.RaiseEvent(evt);
            }
        }

        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            var txt = new TextBlock { };

            var b = (Binding as Binding);

            var binding = new Binding(b.Path.Path);
            binding.StringFormat = "{0:d}";
            binding.UpdateSourceTrigger = b.UpdateSourceTrigger;

            txt.SetBinding(TextBlock.TextProperty, binding);

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
