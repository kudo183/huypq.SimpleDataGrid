using System;

namespace SimpleDataGrid.ViewModel
{
    public class HeaderComboBoxFilterModel : HeaderFilterBaseModel
    {
        public const string ComboBoxFilter = "ComboBoxFilter";
        public const string TextFilter = "TextFilter";

        public HeaderComboBoxFilterModel(string name,
            string filterType,
            string propertyName,
            Type propertyType,
            string displayMemberPath,
            string selectedValuePath)
            : base(name, filterType, propertyName, propertyType)
        {
            _displayMemberPath = displayMemberPath;
            _selectedValuePath = selectedValuePath;
        }

        private string _displayMemberPath;

        public string DisplayMemberPath
        {
            get { return _displayMemberPath; }
            set
            {
                if (IsSkipSet(_displayMemberPath, value) == true)
                {
                    return;
                }

                _displayMemberPath = value;
                OnPropertyChanged(nameof(DisplayMemberPath));
            }
        }

        private string _selectedValuePath;

        public string SelectedValuePath
        {
            get { return _selectedValuePath; }
            set
            {
                if (IsSkipSet(_selectedValuePath, value) == true)
                {
                    return;
                }

                _selectedValuePath = value;
                OnPropertyChanged(nameof(SelectedValuePath));
            }
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
                OnPropertyChanged(nameof(ItemSource));
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
