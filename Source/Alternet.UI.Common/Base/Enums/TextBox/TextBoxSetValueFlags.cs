using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates flags used in set value methods of the text box and rich text box controls.
    /// </summary>
    /// <remarks>
    /// This enumeration supports a bitwise combination of its member values.
    /// </remarks>
    [Flags]
    public enum TextBoxSetValueFlags
    {
        /// <summary>
        /// Send no events.
        /// </summary>
        NoEvent = 0,

        /// <summary>
        /// Send events.
        /// </summary>
        SendEvent = 1,

        /// <summary>
        /// Selection only.
        /// </summary>
        SelectionOnly = 2,
    }
}