using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace SimpleDataGrid
{
    public class ComboBoxEx : ComboBox
    {
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
                oldValue.ResetCompleted -= combo.ProcessResetCompleted;
            }

            var newValue = e.NewValue as INotifyCollectionChangedEx;
            if (newValue != null)
            {
                newValue.ResetCompleted += combo.ProcessResetCompleted;
            }
        }

        private object _oldSelectedItem;

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            if (e.RemovedItems.Count > 0)
            {
                _oldSelectedItem = e.RemovedItems[0];
            }

            base.OnSelectionChanged(e);
        }

        private void ProcessResetCompleted()
        {
            var source = ItemsSource as INotifyCollectionChangedEx;

            if (source == null || _oldSelectedItem == null)
            {
                return;
            }

            var exp = GetBindingExpression(ComboBox.SelectedValueProperty);

            var o = _oldSelectedItem.GetType().GetProperty(SelectedValuePath);
            var p = exp.ResolvedSource.GetType().GetProperty(exp.ResolvedSourcePropertyName);

            p.SetValue(exp.ResolvedSource, o.GetValue(_oldSelectedItem));

            exp.UpdateTarget();
        }
    }
}

