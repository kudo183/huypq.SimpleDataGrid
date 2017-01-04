using System.Windows;
using System.Windows.Controls;

namespace SimpleDataGrid
{
    /// <summary>
    /// Interaction logic for DataGridTextColumnExt.xaml
    /// </summary>
    public partial class DataGridTextColumnExt : DataGridTextColumn
    {
        public DataGridTextColumnExt()
        {
            InitializeComponent();
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
