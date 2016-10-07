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

        public DataGridComboBoxColumnExt()
        {
            InitializeComponent();
            SelectedValuePath = "Key";
            DisplayMemberPath = "Value";
        }

        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            var comboBox = new ComboBoxEx();

            ApplyStyle(true, false, comboBox);
            ApplyColumnProperties(comboBox);

            var binding = BindingOperations.GetBinding(this, ComboBox.ItemsSourceProperty);
            ApplyBinding(binding, comboBox, ComboBoxEx.ItemsSourceExProperty);

            _editableTextBox = null;
            _editingElement = comboBox;

            return comboBox;
        }

        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            var comboBox = new TextBlockComboBox();

            ApplyStyle(false, false, comboBox);
            ApplyColumnProperties(comboBox);

            var binding = BindingOperations.GetBinding(this, ComboBox.ItemsSourceProperty);
            ApplyBinding(binding, comboBox, ComboBoxEx.ItemsSourceExProperty);

            return comboBox;
        }

        #region code from DataGridComboBoxColumn.cs and DataGridHelper.cs
        internal class TextBlockComboBox : ComboBoxEx
        {
            static TextBlockComboBox()
            {
                DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBlockComboBox), new FrameworkPropertyMetadata(DataGridComboBoxColumn.TextBlockComboBoxStyleKey));
                KeyboardNavigation.IsTabStopProperty.OverrideMetadata(typeof(TextBlockComboBox), new FrameworkPropertyMetadata(false));
            }
        }
        /// <summary> 
        ///     Assigns the ElementStyle to the desired property on the given element.
        /// </summary> 
        private void ApplyStyle(bool isEditing, bool defaultToElementStyle, FrameworkElement element)
        {
            Style style = PickStyle(isEditing, defaultToElementStyle);
            if (style != null)
            {
                element.Style = style;
            }
        }

        private Style PickStyle(bool isEditing, bool defaultToElementStyle)
        {
            Style style = isEditing ? EditingElementStyle : ElementStyle;
            if (isEditing && defaultToElementStyle && (style == null))
            {
                style = ElementStyle;
            }

            return style;
        }

        private void ApplyColumnProperties(ComboBox comboBox)
        {
            ApplyBinding(SelectedItemBinding, comboBox, ComboBox.SelectedItemProperty);
            ApplyBinding(SelectedValueBinding, comboBox, ComboBox.SelectedValueProperty);
            ApplyBinding(TextBinding, comboBox, ComboBox.TextProperty);

            SyncColumnProperty(this, comboBox, ComboBox.SelectedValuePathProperty, SelectedValuePathProperty);
            SyncColumnProperty(this, comboBox, ComboBox.DisplayMemberPathProperty, DisplayMemberPathProperty);
            SyncColumnProperty(this, comboBox, ComboBox.ItemsSourceProperty, ItemsSourceProperty);
        }

        private static bool IsDefaultValue(DependencyObject d, DependencyProperty dp)
        {
            return DependencyPropertyHelper.GetValueSource(d, dp).BaseValueSource == BaseValueSource.Default;
        }

        private static void SyncColumnProperty(DependencyObject column, DependencyObject content, DependencyProperty contentProperty, DependencyProperty columnProperty)
        {
            if (IsDefaultValue(column, columnProperty))
            {
                content.ClearValue(contentProperty);
            }
            else
            {
                content.SetValue(contentProperty, column.GetValue(columnProperty));
            }
        }

        /// <summary> 
        ///     Assigns the Binding to the desired property on the target object. 
        /// </summary>
        private static void ApplyBinding(BindingBase binding, DependencyObject target, DependencyProperty property)
        {
            if (binding != null)
            {
                BindingOperations.SetBinding(target, property, binding);
            }
            else
            {
                BindingOperations.ClearBinding(target, property);
            }
        }
        #endregion

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
