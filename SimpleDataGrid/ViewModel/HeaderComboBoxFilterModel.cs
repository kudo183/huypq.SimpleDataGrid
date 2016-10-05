using System;

namespace SimpleDataGrid.ViewModel
{
    public class HeaderComboBoxFilterModel : HeaderFilterBaseModel
    {
        public const string ComboBoxFilter = "ComboBoxFilter";
        public const string TextFilter = "TextFilter";

        public HeaderComboBoxFilterModel(string name, string filterType, string propertyName, Type propertyType)
            : base(name, filterType, propertyName, propertyType)
        {
            
        }

        private object _itemSource;
        public object ItemSource
        {
            get { return _itemSource; }
            set
            {
                if (_itemSource == value)
                {
                    return;
                }

                _itemSource = value;
                OnPropertyChanged("ItemSource");
            }
        }

        public SimpleCommand AddCommand { get; set; }
    }
}
