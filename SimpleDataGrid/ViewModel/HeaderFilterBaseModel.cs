using System.ComponentModel;

namespace SimpleDataGrid.ViewModel
{
    public abstract class HeaderFilterBaseModel : INotifyPropertyChanged
    {
        public string FilterType { get; set; }

        protected HeaderFilterBaseModel(string name, string filterType)
        {
            _name = name;
            FilterType = filterType;
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value)
                {
                    return;
                }

                _name = value;
                OnPropertyChanged("Name");
            }
        }
        
        protected bool _isUsed = true;
        public bool IsUsed
        {
            get { return _isUsed; }
            set
            {
                if (_isUsed == value)
                {
                    return;
                }

                _isUsed = value;
                OnPropertyChanged("IsUsed");
            }
        }

        private object _filterValue;
        public object FilterValue
        {
            get { return _filterValue; }
            set
            {
                if (_filterValue == value)
                {
                    return;
                }

                _filterValue = value;
                OnPropertyChanged("FilterValue");
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
