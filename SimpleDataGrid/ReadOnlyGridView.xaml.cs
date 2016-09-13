using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SimpleDataGrid
{
    /// <summary>
    /// Interaction logic for ReadOnlyGridView.xaml
    /// </summary>
    public partial class ReadOnlyGridView : UserControl
    {
        public List<DataGridColumn> Columns { get; set; }
        public List<UIElement> CustomMenuItems { get; set; }

        public object SelectedItem { get { return dataGrid.SelectedItem; } }

        private bool _isFirstLoaded = true;

        public ReadOnlyGridView()
        {
            Columns = new List<DataGridColumn>();
            CustomMenuItems = new List<UIElement>();
            InitializeComponent();

            Loaded += EditableGridView_Loaded;

        }

        void EditableGridView_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeWhenFirstLoaded();
        }

        void InitializeWhenFirstLoaded()
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
