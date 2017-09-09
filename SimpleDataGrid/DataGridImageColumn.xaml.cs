using huypq.wpf.controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace SimpleDataGrid
{
    /// <summary>
    /// Interaction logic for DataGridImageColumn.xaml
    /// </summary>
    public partial class DataGridImageColumn : DataGridBoundColumn
    {
        public Binding ImageStreamBinding { get; set; }

        public DataGridImageColumn()
        {
            InitializeComponent();
        }

        protected override object PrepareCellForEdit(FrameworkElement editingElement, RoutedEventArgs editingEventArgs)
        {
            return null;
        }

        protected override void CancelCellEdit(FrameworkElement editingElement, object uneditedValue)
        {
            var ip = editingElement as ImagePicker;
            if (ip != null)
            {
                var exp = ip.GetBindingExpression(ImagePicker.FilePathProperty);
                exp.UpdateTarget();
            }
        }

        protected override bool CommitCellEdit(FrameworkElement editingElement)
        {
            var ip = editingElement as ImagePicker;
            if (ip != null)
            {
                var exp = ip.GetBindingExpression(ImagePicker.FilePathProperty);
                exp.UpdateSource();
                return !Validation.GetHasError(ip);
            }

            return true;
        }

        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            var element = new ImagePicker();
            element.IsEditable = false;

            //the binding order of FilePath and ImageStream is important because the dependent of its. Must set ImageStream binding before FilePath binding because if set FilePath fisrt, ImageStream create by FilePath will be overrided by ImageStream binding value
            element.SetBinding(ImagePicker.ImageStreamProperty, ImageStreamBinding);
            element.SetBinding(ImagePicker.FilePathProperty, Binding);

            return element;
        }

        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            var element = new ImagePicker();
            element.IsEditable = true;

            //the binding order of FilePath and ImageStream is important because the dependent of its. Must set ImageStream binding before FilePath binding because if set FilePath fisrt, ImageStream create by FilePath will be overrided by ImageStream binding value
            element.SetBinding(ImagePicker.ImageStreamProperty, ImageStreamBinding);
            element.SetBinding(ImagePicker.FilePathProperty, Binding);

            return element;
        }
    }
}
