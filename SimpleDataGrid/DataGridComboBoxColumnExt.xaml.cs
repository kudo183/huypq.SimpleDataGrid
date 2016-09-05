using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace SimpleDataGrid
{
    /// <summary>
    /// Interaction logic for DataGridComboBoxColumnExt.xaml
    /// </summary>
    public partial class DataGridComboBoxColumnExt : DataGridComboBoxColumn
    {
        private FrameworkElement _editingElement;
        private TextBox _editableTextBox;

        public event EventHandler HeaderAddButtonClick;

        public DataGridComboBoxColumnExt()
        {
            InitializeComponent();
        }

        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            _editableTextBox = null;

            _editingElement = base.GenerateEditingElement(cell, dataItem);
            CopyItemsSource(_editingElement);

            return _editingElement;
        }

        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            FrameworkElement element = base.GenerateElement(cell, dataItem);
            CopyItemsSource(element);
            return element;
        }

        private void CopyItemsSource(FrameworkElement element)
        {
            var binding = BindingOperations.GetBinding(this, ComboBox.ItemsSourceProperty);
            if (binding == null)
            {
                return;
            }

            BindingOperations.SetBinding(element, ComboBox.ItemsSourceProperty, binding);
        }

        void headerAddButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = e.OriginalSource as Button;
            if (btn == null || btn.Name != "btnAdd")
            {
                return;
            }

            if (HeaderAddButtonClick == null)
            {
                return;
            }

            HeaderAddButtonClick(sender, e);
        }

        void comboBox_KeyDown(object sender, KeyEventArgs e)
        {
            //fixbug unikey: because textbox auto select text cause the unikey replace wrong character, need more one Back key to remove selected text
            //if Back key and selected text != null, remove selected text
            if (e.Key == Key.Back)
            {
                if (_editableTextBox == null)
                {
                    var combo = _editingElement as ComboBox;
                    _editableTextBox = combo.Template.FindName("PART_EditableTextBox", combo) as TextBox;
                }

                if (string.IsNullOrEmpty(_editableTextBox.SelectedText) == false)
                {
                    _editableTextBox.Text = _editableTextBox.Text.Remove(_editableTextBox.SelectionStart,
                                                                         _editableTextBox.SelectionLength);
                    _editableTextBox.Select(_editableTextBox.Text.Length, 0);
                }
            }
        }
    }
}
