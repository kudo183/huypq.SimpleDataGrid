using SimpleDataGrid.Converter;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SimpleDataGrid
{
    /// <summary>
    /// Interaction logic for DataGridByteArrayColumn.xaml
    /// </summary>
    public partial class DataGridByteArrayColumn : DataGridTextColumn
    {
        static ByteArrayToBase64TextConverter _converter = new ByteArrayToBase64TextConverter();
        public DataGridByteArrayColumn()
        {
            InitializeComponent();
        }

        protected override void OnBindingChanged(BindingBase oldBinding, BindingBase newBinding)
        {
            base.OnBindingChanged(oldBinding, newBinding);
            (newBinding as System.Windows.Data.Binding).Converter = _converter;
        }

        public void SetStyleAsRightAlign()
        {
            var elementStyle = new Style(typeof(TextBlock));
            elementStyle.Setters.Add(new Setter(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Right));
            elementStyle.Setters.Add(new Setter(TextBlock.MarginProperty, new Thickness(0, 0, 3, 0)));
            ElementStyle = elementStyle;
            var editingElementStyle = new Style(typeof(TextBox));
            editingElementStyle.Setters.Add(new Setter(TextBox.HorizontalContentAlignmentProperty, HorizontalAlignment.Right));
            EditingElementStyle = editingElementStyle;
        }

        public void SetStyleAsRightAlignIntegerNumber()
        {
            Binding.StringFormat = "{0:N0}";
            SetStyleAsRightAlign();
        }
    }
}
