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
        static ByteArrayToBase64TextConverter _base64Converter;
        static ByteArrayToHexTextConverter _hexConverter;

        IValueConverter _converter;

        public DataGridByteArrayColumn()
        {
            InitializeComponent();
        }

        protected override void OnBindingChanged(BindingBase oldBinding, BindingBase newBinding)
        {
            base.OnBindingChanged(oldBinding, newBinding);
            if (_converter == null)
            {
                if (_hexConverter == null)
                {
                    _hexConverter = new ByteArrayToHexTextConverter();
                }
                _converter = _hexConverter;
            }
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

        public void SetBase64Converter()
        {
            if (_base64Converter == null)
            {
                _base64Converter = new ByteArrayToBase64TextConverter();
            }

            _converter = _base64Converter;

            if (Binding != null)
            {
                (Binding as System.Windows.Data.Binding).Converter = _converter;
            }
        }
    }
}
