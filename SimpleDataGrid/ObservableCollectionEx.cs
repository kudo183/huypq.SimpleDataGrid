using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace SimpleDataGrid
{
    public delegate void ResetCompletedEventHandler();

    public interface INotifyCollectionChangedEx : INotifyCollectionChanged
    {
        event ResetCompletedEventHandler BeginReset;
        event ResetCompletedEventHandler EndReset;
    }

    public class ObservableCollectionEx<T> : ObservableCollection<T>, INotifyCollectionChangedEx
    {
        public ObservableCollectionEx() : base() { }
        public ObservableCollectionEx(List<T> list) : base(list) { }
        public ObservableCollectionEx(IEnumerable<T> collection) : base(collection) { }

        public event ResetCompletedEventHandler BeginReset;
        public event ResetCompletedEventHandler EndReset;

        public void Reset(IEnumerable<T> data)
        {
            OnBeginReset();

            Items.Clear();
            foreach (var item in data)
            {
                Items.Add(item);
            }

            OnEndReset();
        }

        public virtual void OnBeginReset()
        {
            BeginReset?.Invoke();
        }

        public virtual void OnEndReset()
        {
            EndReset?.Invoke();
        }
    }
}
