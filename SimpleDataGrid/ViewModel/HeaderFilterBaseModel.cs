using System;
using System.ComponentModel;

namespace SimpleDataGrid.ViewModel
{
    public abstract class HeaderFilterBaseModel : INotifyPropertyChanged
    {
        public string FilterType { get; set; }

        public string PropertyName { get; set; }
        public Type PropertyType { get; set; }

        public Action ActionIsUsedChanged { get; set; }
        public Action ActionFilterValueChanged { get; set; }

        protected HeaderFilterBaseModel(string name, string filterType, string propertyName, Type propertyType)
        {
            _name = name;
            FilterType = filterType;
            PropertyName = propertyName;
            PropertyType = propertyType;
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (IsSkipSet(_name, value) == true)
                {
                    return;
                }

                _name = value;
                OnPropertyChanged("Name");
            }
        }

        protected bool _isUsed = false;
        public bool IsUsed
        {
            get { return _isUsed; }
            set
            {
                if (IsSkipSet(_isUsed, value) == true)
                {
                    return;
                }

                _isUsed = value;
                OnPropertyChanged("IsUsed");
                PropertyChangedAction("IsUsed");
            }
        }

        private object _filterValue;
        public object FilterValue
        {
            get { return _filterValue; }
            set
            {
                if (IsSkipSet(_filterValue, value) == true)
                {
                    return;
                }

                _filterValue = value;
                OnPropertyChanged("FilterValue");
                PropertyChangedAction("FilterValue");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void PropertyChangedAction(string propertyName)
        {
            if (propertyName == "IsUsed" && ActionIsUsedChanged != null)
            {
                ActionIsUsedChanged();
            }

            if (propertyName == "FilterValue" && ActionFilterValueChanged != null)
            {
                ActionFilterValueChanged();
            }
        }

        public virtual bool IsSkipSet(object oldValue, object newValue)
        {
            return object.Equals(oldValue, newValue);
        }
    }
}
