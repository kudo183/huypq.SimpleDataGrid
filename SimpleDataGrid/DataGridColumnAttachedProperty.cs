using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SimpleDataGrid
{
    public class DataGridColumnAttachedProperty
    {
        public static bool GetIsTabStop(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsTabStopProperty);
        }

        public static void SetIsTabStop(DependencyObject obj, bool value)
        {
            obj.SetValue(IsTabStopProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsTabStop.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsTabStopProperty =
            DependencyProperty.RegisterAttached("IsTabStop", typeof(bool), typeof(DataGridColumn), new PropertyMetadata(true));
    }
}
