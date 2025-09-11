using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Defines properties specific to context menus, extending the base menu properties.
    /// </summary>
    /// <remarks>This interface is intended to provide a contract for properties that are unique to context
    /// menus, while inheriting common menu properties from <see cref="IMenuProperties"/>.</remarks>
    public interface IContextMenuProperties : IMenuProperties
    {
        /// <summary>
        /// Raises the "Closing" and "Closed" events.
        /// </summary>
        /// <remarks>This method is typically called to notify subscribers that the associated object is
        /// closing. Ensure that any necessary cleanup or finalization is performed before invoking this
        /// method.</remarks>
        /// <param name="e">An <see cref="EventArgs"/> instance containing the event data.</param>
        /// <param name="recursive">The <see langword="bool"/> value indicating
        /// whether the closing event should be raised for child items.</param>
        void RaiseClosing(EventArgs e, bool recursive = true);

        /// <summary>
        /// Raises the "Opening" event.
        /// </summary>
        /// <param name="e">A <see cref="CancelEventArgs"/> instance containing the event data.</param>
        /// <param name="recursive">The <see langword="bool"/> value indicating
        /// whether the closing event should be raised for child items.</param>
        void RaiseOpening(CancelEventArgs e, bool recursive = true);
    }
}
