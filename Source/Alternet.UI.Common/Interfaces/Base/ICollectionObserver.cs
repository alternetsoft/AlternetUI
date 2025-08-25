using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a collection of items that can be accessed by index
    /// and has change notification capabilities.
    /// </summary>
    /// <remarks>This interface provides a way to retrieve the total number of items
    /// in the collection and access individual items by their zero-based index.
    /// Changes to the collection can be observed through the <see cref="Notification"/> property.
    /// </remarks>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    public interface ICollectionObserver<T>
    {
        /// <summary>
        /// Gets the object that provides notifications when the collection changes.
        /// </summary>
        INotifyCollectionChanged Notification { get; }

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
    }
}
