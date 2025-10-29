using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies creation options for a list box handler.
    /// </summary>
    /// <remarks>
    /// This enumeration is treated as a bit field; combine values with a bitwise OR.
    /// Use these flags when creating or configuring a list box handler to enable
    /// optional behavior such as displaying check boxes for items.
    /// </remarks>
    [Flags]
    public enum ListBoxHandlerCreateFlags
    {
        /// <summary>
        /// No special options; default behavior.
        /// </summary>
        None = 0x0000,

        /// <summary>
        /// Display a check box next to each item in the list box.
        /// </summary>
        /// <remarks>
        /// When this flag is set, items will show check boxes allowing independent
        /// checked state separate from item selection.
        /// </remarks>
        CheckBoxes = 0x0001,
    }
}
