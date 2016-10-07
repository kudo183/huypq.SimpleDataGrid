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
                if (IsSkipSet(_itemSource, value) == true)
                {
                    return;
                }

                _itemSource = value;
                OnPropertyChanged("ItemSource");
            }
        }
        
        public override bool IsSkipSet(object oldValue, object newValue)
        {
            var source = _itemSource as INotifyCollectionChangedEx;
            if (source != null && source.IsResetting)
            {
                return true;
            }

            return base.IsSkipSet(oldValue, newValue);
        }

        public SimpleCommand AddCommand { get; set; }
    }
}
