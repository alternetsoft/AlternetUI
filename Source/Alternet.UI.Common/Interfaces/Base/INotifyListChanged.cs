using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// A collection implementing this interface will notify listeners of dynamic changes,
    /// e.g. when items get added and removed or the whole list is refreshed.
    /// </summary>
    public interface INotifyListChanged
    {
        /// <summary>
        /// Occurs when the collection changes, either by adding or removing an item.
        /// </summary>
        /// <remarks>
        /// The event handler receives an argument of type
        /// <seealso cref="Alternet.UI.ListChangedEventArgs" />
        /// containing data related to this event.
        /// </remarks>
        event ListChangedEventHandler? CollectionChanged;
    }
}
