using System;

namespace Alternet.UI
{
    public class CollectionChangeEventArgs<T> : EventArgs
    {
        public CollectionChangeEventArgs(int index, T item)
        {
            Index = index;
            Item = item;
        }

        public int Index { get; }

        public T Item { get; }
    }
}