using System;
using System.Collections.Generic;

namespace SimpleDataGrid
{
    public class SortedObservableCollection<T> : ObservableCollectionEx<T>
    {
        Func<T, T, bool> _orderChecker;

        public void SetOrderChecker(Func<T, T, bool> orderChecker)
        {
            if (orderChecker == null)
            {
                throw new ArgumentNullException("orderChecker");
            }
            if (Items.Count > 0)
            {
                throw new NotSupportedException("OrderChecker can only set when Count = 0");
            }
            _orderChecker = orderChecker;
        }

        [Obsolete("This is not supported in this class, use Get method instead", true)]
        public new T this[int index]
        {
            get
            {
                throw new NotSupportedException("OrderedObservableCollection not support get indexer method");
            }
            set
            {
                throw new NotSupportedException("OrderedObservableCollection not support set indexer method");
            }
        }

        public T Get(int index)
        {
            return base[index];
        }

        public new void Reset(IEnumerable<T> data)
        {
            OnBeginReset();

            Items.Clear();
            foreach (var item in data)
            {
                Add(item);
            }

            OnEndReset();
        }

        public new void Add(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if (_orderChecker == null)
            {
                throw new NotSupportedException("OrderChecker not set yet.");
            }

            int index = Items.Count;
            for (int i = 0; i < Items.Count; i++)
            {
                if (_orderChecker(Items[i], item) == false)
                {
                    index = i;
                    break;
                }
            }

            base.InsertItem(index, item);
        }

        [Obsolete("This is not supported in this class.", true)]
        public new void Insert(int index, T item)
        {
            throw new NotSupportedException("OrderedObservableCollection not support Insert method");
        }

        [Obsolete("This is not supported in this class.", true)]
        public new void Move(int oldIndex, int newIndex)
        {
            throw new NotSupportedException("OrderedObservableCollection not support Move method");
        }

        public void UpdateFirst(Func<T, bool> predicate, Action<T> itemUpdater)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }
            if (itemUpdater == null)
            {
                throw new ArgumentNullException("itemUpdater");
            }

            for (var i = 0; i < Items.Count; i++)
            {
                var item = Items[i];
                if (predicate(item) == true)
                {
                    itemUpdater(item);

                    CheckAndCorrectItemOrder(i);
                    return;
                }
            }
        }

        public void UpdateAll(Func<T, bool> predicate, Action<T> itemUpdater)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }
            if (itemUpdater == null)
            {
                throw new ArgumentNullException("itemUpdater");
            }

            for (var i = 0; i < Items.Count; i++)
            {
                var item = Items[i];
                if (predicate(item) == true)
                {
                    itemUpdater(item);

                    CheckAndCorrectItemOrder(i);
                }
            }
        }

        /// <summary>
        /// return true if item is in correct order
        /// </summary>
        /// <param name="itemIndex"></param>
        /// <returns></returns>
        private bool CheckAndCorrectItemOrder(int itemIndex)
        {
            var item = Items[itemIndex];

            //check left order
            for (var i = itemIndex; i > 0; i--)
            {
                var leftItem = Items[i - 1];
                if (_orderChecker(leftItem, item) == true)
                {
                    if (itemIndex != i)
                    {
                        MoveItem(itemIndex, i);
                        return false;
                    }
                    break;
                }
            }

            //check right order
            var endRightIndex = Items.Count - 1;
            for (var i = itemIndex; i < endRightIndex; i++)
            {
                var rightItem = Items[i + 1];
                if (_orderChecker(item, rightItem) == true)
                {
                    if (itemIndex != i)
                    {
                        MoveItem(itemIndex, i);
                        return false;
                    }
                    break;
                }
            }

            return true;
        }
    }
}
