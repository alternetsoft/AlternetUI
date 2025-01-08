using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Alternet.Base.Collections;

namespace Alternet.UI
{
    internal class ListControlItemsAdapter : IListControlItems<object>, IList
    {
        private readonly IListControlItems<ListControlItem> items;

        public ListControlItemsAdapter(IListControlItems<ListControlItem> items)
        {
            this.items = items;

            items.ItemInserted += Items_ItemInserted;
            items.ItemRemoved += Items_ItemRemoved;
            items.ItemRangeAdditionFinished += Items_ItemRangeAdditionFinished;
            items.PropertyChanged += Items_PropertyChanged;
            items.CollectionChanged += Items_CollectionChanged;
        }

        ~ListControlItemsAdapter()
        {
            items.ItemInserted -= Items_ItemInserted;
            items.ItemRemoved -= Items_ItemRemoved;
            items.ItemRangeAdditionFinished -= Items_ItemRangeAdditionFinished;
            items.PropertyChanged -= Items_PropertyChanged;
            items.CollectionChanged -= Items_CollectionChanged;
        }

        public event CollectionItemChangedHandler<object>? ItemInserted;

        public event CollectionItemChangedHandler<object>? ItemRemoved;

        public event CollectionItemRangeChangedHandler<object>? ItemRangeAdditionFinished;

        public event PropertyChangedEventHandler? PropertyChanged;

        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        public bool RangeOpInProgress => items.RangeOpInProgress;

        public int Count => items.Count;

        public bool IsReadOnly => items.IsReadOnly;

        public IList AsList
        {
            get => this;
        }

        bool IList.IsFixedSize => ((IList)items).IsFixedSize;

        bool ICollection.IsSynchronized => ((ICollection)items).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)items).SyncRoot;

        public object this[int index]
        {
            get
            {
                return items[index].Value!;
            }

            set
            {
                items[index].Value = value;
            }
        }

        public void Add(object value)
        {
            ListControlItem item = new();
            item.Value = value;
            items.Add(item);
        }

        public void AddRange(IEnumerable<object> collection)
        {
            foreach (var item in collection)
                Add(item);
        }

        public void Clear()
        {
            items.Clear();
        }

        public bool Contains(object value)
        {
            foreach(var item in items)
            {
                if (item.Value == value)
                    return true;
            }

            return false;
        }

        public void Insert(int index, object value)
        {
            ListControlItem item = new();
            item.Value = value;
            items.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            items.RemoveAt(index);
        }

        public int IndexOf(object item)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Value == item)
                {
                    return i;
                }
            }

            return -1;
        }

        public bool Remove(object item)
        {
            var index = IndexOf(item);

            if(index >= 0)
            {
                items.RemoveAt(index);
                return true;
            }

            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        int IList.Add(object value)
        {
            ListControlItem item = new();
            item.Value = value;
            return ((IList)items).Add(item);
        }

        void IList.Remove(object value)
        {
            Remove(value);
        }

        public void CopyTo(object[] array, int arrayIndex)
        {
            var itm = items.Select(x => x.Value).ToList();
            itm.CopyTo(array, arrayIndex);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            var itm = items.Select(x => x.Value).ToList();
            ((ICollection)itm).CopyTo(array, index);
        }

        public IEnumerator<object> GetEnumerator()
        {
            var itm = items.Select(x => x.Value!);
            return itm.GetEnumerator();
        }

        private void Items_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        private void Items_ItemRemoved(object? sender, int index, ListControlItem item)
        {
            ItemRemoved?.Invoke(this, index, item?.Value!);
        }

        private void Items_ItemInserted(object? sender, int index, ListControlItem item)
        {
            ItemInserted?.Invoke(this, index, item?.Value!);
        }

        private void Items_ItemRangeAdditionFinished(
            object? sender,
            int index,
            IEnumerable<ListControlItem> items)
        {
            ItemRangeAdditionFinished?.Invoke(this, index, GetItems());

            IEnumerable<object> GetItems()
            {
                foreach(var item in items)
                {
                    yield return item.Value!;
                }
            }
        }

        private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged is null)
                return;

            var action = e.Action;

            // Gets the list of new items involved in the change.
            var newItems = ToValues(e.NewItems);

            // Gets the index at which the change occurred.
            var newStartingIndex = e.NewStartingIndex;

            // Gets the list of items affected by a Replace, Remove, or Move action.
            var oldItems = ToValues(e.OldItems);

            // Gets the index at which a Move, Remove, or Replace action occurred.
            var oldStartingIndex = e.OldStartingIndex;

            IList ToValues(IList? items)
            {
                IList result = new List<object>();

                if (items is null)
                    return result;

                foreach(var item in items)
                {
                    result.Add(((ListControlItem)item).Value);
                }

                return result;
            }

            NotifyCollectionChangedEventArgs args;

            switch (action)
            {
                case NotifyCollectionChangedAction.Add:
                    args = CollectionChangedHelper.OnInsertItem(newStartingIndex, newItems);
                    CollectionChanged(this, args);
                    break;
                case NotifyCollectionChangedAction.Move:
                    args = CollectionChangedHelper
                        .OnMoveItem(oldItems, oldStartingIndex, newStartingIndex);
                    CollectionChanged(this, args);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    args = CollectionChangedHelper.OnRemoveItem(oldStartingIndex, oldItems);
                    CollectionChanged(this, args);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    args = CollectionChangedHelper
                        .OnReplaceItem(oldItems, oldStartingIndex, newItems);
                    CollectionChanged(this, args);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    args = CollectionChangedHelper.OnResetItems();
                    CollectionChanged(this, args);
                    break;
            }
        }

        private static class CollectionChangedHelper
        {
            public static NotifyCollectionChangedEventArgs OnResetItems()
            {
                return new(NotifyCollectionChangedAction.Reset);
            }

            public static NotifyCollectionChangedEventArgs OnRemoveItem(int index, IList val)
            {
                return new (NotifyCollectionChangedAction.Remove, val, index);
            }

            public static NotifyCollectionChangedEventArgs OnInsertItem(int index, IList item)
            {
                return new (NotifyCollectionChangedAction.Add, item, index);
            }

            public static NotifyCollectionChangedEventArgs OnReplaceItem(
                IList oldItem,
                int index,
                IList newItem)
            {
                return new(NotifyCollectionChangedAction.Replace, oldItem, newItem, index);
            }

            public static NotifyCollectionChangedEventArgs OnMoveItem(
                IList val,
                int oldIndex,
                int newIndex)
            {
                return new(NotifyCollectionChangedAction.Move, val, newIndex, oldIndex);
            }
        }
    }
}