using System;
using System.ComponentModel;

namespace SimpleDataGrid.ViewModel
{
    public class PagerViewModel : INotifyPropertyChanged
    {
        private int _currentPageIndex = 1;
        private int _pageSize = 30;
        private int _pageCount;
        private int _itemCount;

        public Action ActionCurrentPageIndexChanged { get; set; }
        public Action ActionPageSizeChanged { get; set; }

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

                ActionCurrentPageIndexChanged?.Invoke();
            }
        }

        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                if (_pageSize != value)
                {
                    _pageSize = value;
                    OnPropertyChanged("PageSize");
                    ActionPageSizeChanged?.Invoke();
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
        
        public SimpleCommand PrevCommand { get; private set; }
        public SimpleCommand NextCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
