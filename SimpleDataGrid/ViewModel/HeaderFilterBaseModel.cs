using System;
using System.ComponentModel;

namespace SimpleDataGrid.ViewModel
{
    public abstract class HeaderFilterBaseModel : INotifyPropertyChanged
    {
        public string FilterType { get; set; }

        public string PropertyName { get; set; }
        public string SortPropertyName { get; set; }
        public Type PropertyType { get; set; }
        public bool IsShowInUI { get; set; }

        public Action ActionIsUsedChanged { get; set; }
        public Action ActionFilterValueChanged { get; set; }
        public Action ActionIsSortedChanged { get; set; }

        public enum SortDirection
        {
            Unsorted,
            Ascending,
            Descending
        }

        protected HeaderFilterBaseModel(string name, string filterType, string propertyName, Type propertyType)
        {
            _name = name;
            FilterType = filterType;
            PropertyName = propertyName;
            PropertyType = propertyType;
            SortPropertyName = propertyName;
            IsShowInUI = true;
            ClearFilterValueCommand = new SimpleCommand(nameof(ClearFilterValueCommand), () =>
            {
                FilterValue = null;
            });
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
                OnPropertyChanged(nameof(Name));
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
                OnPropertyChanged(nameof(IsUsed));
                PropertyChangedAction(nameof(IsUsed));
            }
        }

        protected SortDirection _isSorted = SortDirection.Unsorted;
        public SortDirection IsSorted
        {
            get { return _isSorted; }
            set
            {
                if (IsSkipSet(_isSorted, value) == true)
                {
                    return;
                }

                _isSorted = value;
                OnPropertyChanged(nameof(IsSorted));
                PropertyChangedAction(nameof(IsSorted));
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
                OnPropertyChanged(nameof(FilterValue));
                PropertyChangedAction(nameof(FilterValue));
            }
        }

        public SimpleCommand ClearFilterValueCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void PropertyChangedAction(string propertyName)
        {
            if (_disableAction == true)
            {
                return;
            }

            if (propertyName == nameof(IsUsed) && ActionIsUsedChanged != null)
            {
                ActionIsUsedChanged();
            }

            if (propertyName == nameof(FilterValue) && ActionFilterValueChanged != null)
            {
                ActionFilterValueChanged();
            }

            if (propertyName == nameof(IsSorted) && ActionIsSortedChanged != null)
            {
                ActionIsSortedChanged();
            }
        }

        public virtual bool IsSkipSet(object oldValue, object newValue)
        {
            return object.Equals(oldValue, newValue);
        }

        private bool _disableAction = false;
        public virtual void DisableChangedAction(Action<HeaderFilterBaseModel> setAction)
        {
            _disableAction = true;
            if (setAction != null)
            {
                setAction(this);
            }
            _disableAction = false;
        }
    }
}
