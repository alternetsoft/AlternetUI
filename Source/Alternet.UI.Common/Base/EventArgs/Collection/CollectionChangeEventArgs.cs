using System;
using Alternet.UI;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the <see cref="BaseCollection{T}.ItemInserted"/>
    /// and <see cref="BaseCollection{T}.ItemRemoved"/> events.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    public class CollectionChangeEventArgs<T> : BaseEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionChangeEventArgs{T}"/> class.
        /// </summary>
        /// <param name="index">The index where the change occurred.</param>
        /// <param name="item">The item affected by the change.</param>
        public CollectionChangeEventArgs(int index, T item)
        {
            Index = index;
            Item = item;
        }

        /// <summary>
        /// The index where the change occurred.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// The item affected by the change.
        /// </summary>
        public T Item { get; }
    }
}