using System;
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

        protected virtual void OnBeginReset()
        {
            BeginReset?.Invoke();
        }

        protected virtual void OnEndReset()
        {
            EndReset?.Invoke();
        }

        public T FindFirst(Func<T, bool> predicate)
        {
            foreach (var item in Items)
            {
                if (predicate(item) == true)
                {
                    return item;
                }
            }

            return default(T);
        }

        public List<T> FindAll(Func<T, bool> predicate)
        {
            var result = new List<T>();

            foreach (var item in Items)
            {
                if (predicate(item) == true)
                {
                    result.Add(item);
                }
            }

            return result;
        }

        public void RemoveFirst(Func<T, bool> predicate)
        {
            for (var i = 0; i < Items.Count; i++)
            {
                if (predicate(Items[i]) == true)
                {
                    RemoveAt(i);
                    return;
                }
            }
        }

        public void RemoveAll(Func<T, bool> predicate)
        {
            for (var i = 0; i < Items.Count; i++)
            {
                if (predicate(Items[i]) == true)
                {
                    RemoveAt(i);
                }
            }
        }
    }
}
