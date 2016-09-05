namespace SimpleDataGrid.ViewModel
{
    public class HeaderComboBoxFilterModel : HeaderFilterBaseModel
    {
        public HeaderComboBoxFilterModel(string name)
            : base(name, "ComboBoxFilter")
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
    }
}
