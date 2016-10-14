using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace SimpleDataGrid
{
    public delegate void ResetCompletedEventHandler();

    public interface INotifyCollectionChangedEx : INotifyCollectionChanged
    {
        bool IsResetting { get; set; }
        event ResetCompletedEventHandler ResetCompleted;
    }

    public class ObservableCollectionEx<T> : ObservableCollection<T>, INotifyCollectionChangedEx
    {
        public ObservableCollectionEx() : base() { }
        public ObservableCollectionEx(List<T> list) : base(list) { }
        public ObservableCollectionEx(IEnumerable<T> collection) : base(collection) { }

        public bool IsResetting { get; set; }

        public event ResetCompletedEventHandler ResetCompleted;

        public void Reset(IEnumerable<T> data)
        {
            IsResetting = true;
            Clear();
            foreach (var item in data)
            {
                Add(item);
            }
            IsResetting = false;
            OnResetCompleted();
        }

        public virtual void OnResetCompleted()
        {
            var handler = ResetCompleted;
            if (handler != null)
            {
                handler();
            }
        }
    }
}
