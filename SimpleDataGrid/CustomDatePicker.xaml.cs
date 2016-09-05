using System;
using System.Windows.Controls;
using System.Windows.Data;

namespace SimpleDataGrid
{
    /// <summary>
    /// Interaction logic for CustomDatePicker.xaml
    /// </summary>
    public partial class CustomDatePicker : UserControl
    {
        private Binding _selectedDateBinding;
        public Binding SelectedDateBinding
        {
            get { return _selectedDateBinding; }
            set
            {
                if (_selectedDateBinding == value)
                {
                    return;
                }

                _selectedDateBinding = value;
                dp.SetBinding(DatePicker.SelectedDateProperty, _selectedDateBinding);
            }
        }

        private Binding _isEnabledBinding;
        public Binding IsEnabledBinding
        {
            get { return _isEnabledBinding; }
            set
            {
                if (_isEnabledBinding == value)
                {
                    return;
                }

                _isEnabledBinding = value;
                dp.SetBinding(IsEnabledProperty, _isEnabledBinding);
            }
        }

        public CustomDatePicker()
        {
            InitializeComponent();
        }

        private void btnPrev_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (dp.SelectedDate == null)
            {
                return;
            }

            dp.SelectedDate = dp.SelectedDate.Value.Subtract(new TimeSpan(1, 0, 0, 0));
        }

        private void btnNext_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (dp.SelectedDate == null)
            {
                return;
            }

            dp.SelectedDate = dp.SelectedDate.Value.AddDays(1);
        }
    }
}
