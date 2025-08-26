using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a contract for an object that provides a <see cref="AbstractControl"/>
    /// to serve as the host for a context menu.
    /// </summary>
    /// <remarks>Implementations of this interface expose a <see cref="AbstractControl"/>
    /// that can be used as the placement target or logical parent for a context menu.
    /// This is typically used in scenarios where context
    /// menus need to be dynamically associated with UI elements.</remarks>
    public interface IContextMenuHost
    {
        /// <summary>
        /// Gets the <see cref="AbstractControl"/> that serves as the host for the context menu.
        /// </summary>
        AbstractControl ContextMenuHost { get; }

        /// <summary>
        /// Gets a value indicating whether the context menu host is currently visible.
        /// </summary>
        bool IsVisible { get; }
    }
}
