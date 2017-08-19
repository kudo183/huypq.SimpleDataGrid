using huypq.wpf.Utils;
using SimpleDataGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimpleDataGridTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TestViewModel ViewModel;
        public MainWindow()
        {
            InitializeComponent();

        }

        protected override void OnInitialized(EventArgs e)
        {
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(new DependencyObject()) == true)
            {
                return;
            }

            ViewModel = new TestViewModel();

            ViewModel.ShowDialogAction = (content, title) =>
            {
                var w = new Window()
                {
                    Title = title,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    Content = content,
                    Owner = Window.GetWindow(this)
                };
                w.ShowDialog();
            };

            ViewModel.LoadCommand = new SimpleCommand("LoadCommand", () =>
            {
                gridView.dataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                ViewModel.Load();
            });

            DataContext = ViewModel;

            gridView.MapHeaderFilterModelToColumnHeader(ViewModel);

            base.OnInitialized(e);
        }
    }
}
