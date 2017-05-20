using System;
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

        protected override void CancelCellEdit(FrameworkElement editingElement, object uneditedValue)
        {
            var dp = editingElement as DatePicker;
            if (dp != null)
            {
                //clear DatePicker textbox text to make sure Text will sync with SelectedDate when call UpdateTarget of SelectedDateProperty,
                //if not clear, textbox may keep text un-sync with SelectedDate, when lost focus or Enter key press, Text will update back to SelectedDate
                dp.Text = "";

                var exp = dp.GetBindingExpression(DatePicker.SelectedDateProperty);
                exp.UpdateTarget();
            }
        }

        protected override bool CommitCellEdit(FrameworkElement editingElement)
        {
            var dp = editingElement as DatePicker;
            if (dp != null)
            {
                var exp = dp.GetBindingExpression(DatePicker.SelectedDateProperty);
                exp.UpdateSource();
                return !Validation.GetHasError(dp);
            }

            return true;
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

            txt.SetBinding(TextBlock.TextProperty, binding);

            return txt;
        }
    }
}
