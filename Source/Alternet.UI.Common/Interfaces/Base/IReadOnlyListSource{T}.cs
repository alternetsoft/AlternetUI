using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a read-only collection of items that can be accessed by index.
    /// </summary>
    /// <remarks>This interface provides a way to retrieve the total number of items
    /// in the collection and
    /// access individual items by their zero-based index.
    /// It is intended for scenarios where read-only access to a
    /// list-like structure is required.</remarks>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    public interface IReadOnlyListSource<T>
    {
        /// <summary>
        /// Gets the total number of items currently in the collection.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Retrieves the item at the specified index in the collection.
        /// </summary>
        /// <param name="index">The zero-based index of the item to retrieve.</param>
        /// <returns>The item at the specified index.</returns>
        T GetItem(int index);

        /// <summary>
        /// Gets the object that provides notifications when the collection changes.
        /// </summary>
        INotifyCollectionChanged Notification { get; }
    }
}
