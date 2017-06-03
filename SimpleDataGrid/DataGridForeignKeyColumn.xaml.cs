using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace SimpleDataGrid
{
    /// <summary>
    /// Interaction logic for DataGridForeignKeyColumn.xaml
    /// </summary>
    public partial class DataGridForeignKeyColumn : DataGridBoundColumn
    {
        public Binding DisplayTextBinding { get; set; }

        UserControl popupView;
        public UserControl PopupView
        {
            get { return popupView; }
            set
            {
                popupView = value;
                popupView.Padding = new Thickness(3);
                popupView.Background = new SolidColorBrush(Color.FromRgb(45, 153, 115));
                popupView.Width = 600;
                popupView.Height = 400;
            }
        }

        public string SelectedIDPath { get; set; } = "SelectedValue";

        public DataGridForeignKeyColumn()
        {
            InitializeComponent();
        }

        protected override object PrepareCellForEdit(FrameworkElement editingElement, RoutedEventArgs editingEventArgs)
        {
            return null;
        }

        protected override void CancelCellEdit(FrameworkElement editingElement, object uneditedValue)
        {
            var fp = editingElement as ForeignKeyPicker;
            if (fp != null)
            {
                var exp = fp.GetBindingExpression(ForeignKeyPicker.SelectedForeignKeyProperty);
                exp.UpdateTarget();
            }
        }

        protected override bool CommitCellEdit(FrameworkElement editingElement)
        {
            var fp = editingElement as ForeignKeyPicker;
            if (fp != null)
            {
                if (fp.IsPopupOpen)
                {
                    fp.SetSelectedForeignKey();
                }
                var exp = fp.GetBindingExpression(ForeignKeyPicker.SelectedForeignKeyProperty);
                exp.UpdateSource();
                return !Validation.GetHasError(fp);
            }

            return true;
        }

        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            var txt = new TextBlock { };
            txt.SetBinding(TextBlock.TextProperty, DisplayTextBinding);

            return txt;
        }

        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            var visualParent = VisualTreeHelper.GetParent(PopupView);
            if (visualParent != null)
            {
                var deco = visualParent as Decorator;
                if (deco != null)
                {
                    deco.Child = null;
                }
            }

            var logicalParent = PopupView.Parent;
            if (logicalParent != null)
            {
                var p = PopupView.Parent as Popup;
                if (p != null)
                {
                    p.Child = null;
                }
            }

            var element = new ForeignKeyPicker()
            {
                PopupView = PopupView,
                PopupViewSelectedIDPath = SelectedIDPath
            };

            element.SetBinding(ForeignKeyPicker.SelectedForeignKeyProperty, Binding);
            return element;
        }
    }
}
