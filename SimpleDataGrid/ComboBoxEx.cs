using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace SimpleDataGrid
{
    public class ComboBoxEx : ComboBox
    {
        public ComboBoxEx() : base()
        {
            ItemsPanel = new ItemsPanelTemplate();
            var stackPanelTemplate = new FrameworkElementFactory(typeof(VirtualizingStackPanel));
            ItemsPanel.VisualTree = stackPanelTemplate;
        }

        public IEnumerable ItemsSourceEx
        {
            get { return (IEnumerable)GetValue(ItemsSourceExProperty); }
            set
            {
                SetValue(ItemsSourceExProperty, value);
                ItemsSource = value;
            }
        }

        // Using a DependencyProperty as the backing store for ItemsSourceEx.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceExProperty =
            DependencyProperty.Register(
                "ItemsSourceEx", typeof(IEnumerable), typeof(ComboBoxEx),
                new PropertyMetadata(null, new PropertyChangedCallback(OnItemsSourceExChanged)));

        private static void OnItemsSourceExChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.SetValue(ItemsSourceProperty, e.NewValue);
            var combo = d as ComboBoxEx;

            var oldValue = e.OldValue as INotifyCollectionChangedEx;
            if (oldValue != null)
            {
                oldValue.BeginReset -= combo.NewValue_BeginReset;
                oldValue.EndReset -= combo.NewValue_EndReset;
            }

            var newValue = e.NewValue as INotifyCollectionChangedEx;
            if (newValue != null)
            {
                newValue.BeginReset += combo.NewValue_BeginReset;
                newValue.EndReset += combo.NewValue_EndReset;
            }
        }

        private object _oldSelectedItem;

        private void NewValue_BeginReset()
        {
            _oldSelectedItem = SelectedValue;
        }

        private void NewValue_EndReset()
        {
            SelectedValue = _oldSelectedItem;
            Items.Refresh();
        }
    }
}