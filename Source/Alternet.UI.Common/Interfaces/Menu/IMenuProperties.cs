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
    public interface IMenuProperties
        : ICollectionObserver<object>, INotifyPropertyChanged, IBaseObjectWithId
    {
    }
}