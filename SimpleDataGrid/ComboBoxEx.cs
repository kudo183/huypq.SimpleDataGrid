using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SimpleDataGrid
{
    public class ComboBoxEx : ComboBox
    {
        TextBox _editableTextBox;

        public bool IsUseDropDownClosedSelectedValueBinding { get; set; }

        public ComboBoxEx() : base()
        {
            ItemsPanel = new ItemsPanelTemplate();
            var stackPanelTemplate = new FrameworkElementFactory(typeof(VirtualizingStackPanel));
            ItemsPanel.VisualTree = stackPanelTemplate;

            IsEditable = true;
            IsTextSearchEnabled = true;
            StaysOpenOnEdit = true;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _editableTextBox = Template.FindName("PART_EditableTextBox", this) as TextBox;
        }

        protected override void OnDropDownClosed(EventArgs e)
        {
            base.OnDropDownClosed(e);

            UpdateSelectedValueBindingSource();
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            //fixbug unikey: because textbox auto select text cause the unikey replace wrong character, need more one Back key to remove selected text
            //if Back key and selected text != null, remove selected text
            if (e.Key == Key.Back && _editableTextBox != null)
            {
                if (string.IsNullOrEmpty(_editableTextBox.SelectedText) == false)
                {
                    _editableTextBox.Text = _editableTextBox.Text.Remove(_editableTextBox.SelectionStart,
                                                                         _editableTextBox.SelectionLength);
                    _editableTextBox.Select(_editableTextBox.Text.Length, 0);
                }
            }

            if (e.Key == Key.Enter || e.Key == Key.Escape)
            {
                SelectAllText();
            }
            else if (IsDropDownOpen == false && StaysOpenOnEdit == true)
            {
                IsDropDownOpen = true;
            }
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            
            if (IsEditable == true)
            {
                SelectAllText();
                if (IsDropDownOpen == false && StaysOpenOnEdit == true)
                {
                    IsDropDownOpen = true;
                }
            }
        }

        public IEnumerable ItemsSourceEx
        {
            get { return (IEnumerable)GetValue(ItemsSourceExProperty); }
            set { SetValue(ItemsSourceExProperty, value); }
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

        private void UpdateSelectedValueBindingSource()
        {
            if (IsUseDropDownClosedSelectedValueBinding == true)
            {
                var exp = GetBindingExpression(ComboBoxEx.SelectedValueProperty);
                exp.UpdateSource();
            }
        }

        private void SelectAllText()
        {
            if (_editableTextBox != null)
            {
                _editableTextBox.SelectAll();
            }
        }
    }
}