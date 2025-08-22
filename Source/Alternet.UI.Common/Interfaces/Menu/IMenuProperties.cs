using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Defines the properties and behaviors for accessing and interacting
    /// with a collection of menu items.
    /// </summary>
    /// <remarks>This interface provides methods and properties to retrieve
    /// information about a collection of
    /// menu items, such as the total count of items and access to
    /// individual items by index. Implementations of this
    /// interface are expected to provide read-only access to the menu items.</remarks>
    public interface IMenuProperties : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the unique identifier associated with the object.
        /// </summary>
        ObjectUniqueId UniqueId { get; }

        /// <summary>
        /// Gets the object that provides notifications when the menu items collection changes.
        /// </summary>
        INotifyCollectionChanged CollectionNotifier { get; }

        /// <summary>
        /// Gets the total number of menu items currently in the collection.
        /// </summary>
        int ItemCount { get; }

        /// <summary>
        /// Retrieves the child menu item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the child menu item to retrieve.
        /// Must be within the valid range of the child menu items collection.</param>
        /// <returns>An object implementing <see cref="IReadOnlyMenuItemProperties"/>
        /// that represents the menu item at the specified index.</returns>
        IReadOnlyMenuItemProperties GetItem(int index);
    }
}