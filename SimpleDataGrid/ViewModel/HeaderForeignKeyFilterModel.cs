using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SimpleDataGrid.ViewModel
{
    public class HeaderForeignKeyFilterModel : HeaderFilterBaseModel
    {
        public UserControl PopupView { get; set; }

        public string PopupViewSelectedIDPath { get; set; }

        public HeaderForeignKeyFilterModel(string name, string propertyName, Type propertyType, UserControl popupView)
            : base(name, "ForeignKeyFilter", propertyName, propertyType)
        {
            PopupViewSelectedIDPath = "SelectedValue";
            PopupView = popupView;
            PopupView.Padding = new Thickness(3);
            PopupView.Background = new SolidColorBrush(Color.FromRgb(45, 153, 115));
            PopupView.Width = 600;
            PopupView.Height = 400;
        }
    }
}
