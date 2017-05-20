using System;
using System.Windows;
using System.Windows.Controls;

namespace SimpleDataGrid
{
    /// <summary>
    /// Interaction logic for DataGridForeignKeyColumn.xaml
    /// </summary>
    public partial class DataGridForeignKeyColumn : DataGridBoundColumn
    {
        public Type ReferenceType { get; set; }

        public DataGridForeignKeyColumn()
        {
            InitializeComponent();
        }

        protected override object PrepareCellForEdit(FrameworkElement editingElement, RoutedEventArgs editingEventArgs)
        {
            var grid = editingElement as Grid;
            var tb = grid.Children[0] as TextBox;
            return tb.Text;
        }

        protected override void CancelCellEdit(FrameworkElement editingElement, object uneditedValue)
        {
            var grid = editingElement as Grid;
            var tb = grid.Children[0] as TextBox;
            if (tb != null)
            {
                var exp = tb.GetBindingExpression(TextBox.TextProperty);
                exp.UpdateTarget();
            }
        }

        protected override bool CommitCellEdit(FrameworkElement editingElement)
        {
            var grid = editingElement as Grid;
            var tb = grid.Children[0] as TextBox;
            if (tb != null)
            {
                var exp = tb.GetBindingExpression(TextBox.TextProperty);
                exp.UpdateSource();
                return !Validation.GetHasError(tb);
            }

            return true;
        }

        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            var txt = new TextBlock { };
            txt.SetBinding(TextBlock.TextProperty, Binding);

            return txt;
        }

        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            var grid = new Grid()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(25) });

            var txt = new TextBox() { IsReadOnly = true };
            txt.SetBinding(TextBox.TextProperty, Binding);
            
            var btn = new TextBlock() { Text = "🔍", HorizontalAlignment = HorizontalAlignment.Center, Tag = txt };
            btn.MouseLeftButtonDown += Btn_Click;

            Grid.SetColumn(txt, 0);
            Grid.SetColumn(btn, 1);

            grid.Children.Add(txt);
            grid.Children.Add(btn);

            return grid;
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            var view = Activator.CreateInstance(ReferenceType) as UserControl;
            var w = new Window()
            {
                Content = view
            };
            w.ShowDialog();

            var vm = view.DataContext as ViewModel.IEditableGridViewModel;
            var txt = (sender as TextBlock).Tag as TextBox;

            if (vm.SelectedValue == null)
            {
                txt.Text = string.Empty;
            }
            else
            {
                txt.Text = vm.SelectedValue.ToString();
            }
        }
    }
}
