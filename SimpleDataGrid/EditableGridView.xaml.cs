using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace SimpleDataGrid
{
    /// <summary>
    /// Interaction logic for EditableGridView.xaml
    /// </summary>
    public partial class EditableGridView : UserControl
    {
        public Visibility SaveButtonVisibility
        {
            get { return btnSave.Visibility; }
            set { btnSave.Visibility = value; }
        }

        public Visibility LoadButtonVisibility
        {
            get { return btnLoad.Visibility; }
            set { btnLoad.Visibility = value; }
        }

        public bool IsReadOnly
        {
            get { return dataGrid.IsReadOnly; }
            set { dataGrid.IsReadOnly = value; }
        }

        public ObservableCollection<DataGridColumn> Columns { get { return dataGrid.Columns; } }
        public UIElementCollection CustomMenuItems { get { return sp.Children; } }

        public EditableGridView()
        {
            InitializeComponent();
        }
    }
}
