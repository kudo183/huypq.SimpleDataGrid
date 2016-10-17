using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SimpleDataGrid
{
    /// <summary>
    /// Interaction logic for EditableGridView.xaml
    /// </summary>
    public partial class EditableGridView : UserControl
    {
        public List<DataGridColumn> Columns { get; set; }
        public List<UIElement> CustomMenuItems { get; set; }

        private bool _isFirstLoaded = true;

        public EditableGridView()
        {
            Columns = new List<DataGridColumn>();
            CustomMenuItems = new List<UIElement>();
            InitializeComponent();

            Loaded += EditableGridView_Loaded;
        }

        void EditableGridView_Loaded(object sender, RoutedEventArgs e)
        {
            if (_isFirstLoaded == false)
                return;

            foreach (var dataGridColumn in Columns)
            {
                dataGrid.Columns.Add(dataGridColumn);
            }
            foreach (var uiElement in CustomMenuItems)
            {
                sp.Children.Add(uiElement);
            }

            _isFirstLoaded = false;
        }
    }
}
