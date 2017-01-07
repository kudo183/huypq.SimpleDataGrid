using System;
using System.ComponentModel;

namespace SimpleDataGrid.ViewModel
{
    public class PagerViewModel : INotifyPropertyChanged
    {
        private int _currentPageIndex;
        private int _pageCount;
        private int _itemCount;
        private bool _isEnablePaging;

        public Action ActionCurrentPageIndexChanged { get; set; }
        public Action ActionIsEnablePagingChanged { get; set; }

        public PagerViewModel()
        {
            PrevCommand = new SimpleCommand("PrevCommand",
                () => { CurrentPageIndex--; },
                () => CurrentPageIndex > 1
            );

            NextCommand = new SimpleCommand("NextCommand",
                () => { CurrentPageIndex++; },
                () => CurrentPageIndex < PageCount
            );
        }

        public int CurrentPageIndex
        {
            get { return _currentPageIndex; }
            set
            {
                SetCurrentPageIndexWithoutAction(value);

                if (ActionCurrentPageIndexChanged != null)
                {
                    ActionCurrentPageIndexChanged();
                }
            }
        }

        public int PageCount
        {
            get { return _pageCount; }
            set
            {
                if (_pageCount != value)
                {
                    _pageCount = value;
                    OnPropertyChanged("PageCount");
                    PrevCommand.RaiseCanExecuteChanged();
                    NextCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public int ItemCount
        {
            get { return _itemCount; }
            set
            {
                if (_itemCount != value)
                {
                    _itemCount = value;
                    OnPropertyChanged("ItemCount");
                }
            }
        }

        public bool IsEnablePaging
        {
            get { return _isEnablePaging; }
            set
            {
                if (_isEnablePaging != value)
                {
                    _isEnablePaging = value;
                    OnPropertyChanged("IsEnablePaging");
                    if (ActionIsEnablePagingChanged != null)
                    {
                        ActionIsEnablePagingChanged();
                    }
                }
            }
        }

        public SimpleCommand PrevCommand { get; private set; }
        public SimpleCommand NextCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SetCurrentPageIndexWithoutAction(int value)
        {
            if (_currentPageIndex == value || value == 0)
                return;

            var oldIndex = _currentPageIndex;
            var newIndex = value;

            _currentPageIndex = value;
            OnPropertyChanged("CurrentPageIndex");

            if (oldIndex == 1 || newIndex == 1)
            {
                PrevCommand.RaiseCanExecuteChanged();
            }
            if (oldIndex == _pageCount || newIndex == _pageCount)
            {
                NextCommand.RaiseCanExecuteChanged();
            }
        }
    }
}
