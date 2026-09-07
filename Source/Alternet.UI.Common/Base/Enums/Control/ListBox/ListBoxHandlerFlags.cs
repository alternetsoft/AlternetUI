using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the behavior and appearance options for a list box control.
    /// </summary>
    /// <remarks>This enumeration defines flags that can be combined using a bitwise OR operation to configure
    /// the selection mode, scrollbar visibility, and other display characteristics of a list box.</remarks>
    [Flags]
    public enum ListBoxHandlerFlags
    {
        /// <summary>
        /// Represents a default or uninitialized state with no flags set.
        /// </summary>
        None = 0x0000,

        /// <summary>
        /// Represents a selection mode where only a single item can be selected at a time.
        /// </summary>
        SingleSelection = 0x0020,

        /// <summary>
        /// Represents a mode where multiple items can be selected simultaneously.
        /// </summary>
        MultipleSelection = 0x0040,

        /// <summary>
        /// Represents the extended selection mode for a control, allowing multiple items
        /// to be selected using keyboard or mouse input.
        /// The user can extend the selection by using SHIFT or CTRL keys together with
        /// the cursor movement keys or the mouse.
        /// </summary>
        ExtendedSelection = 0x0080,

        /// <summary>
        /// Specifies that the vertical scroll bar is always displayed, regardless of the content size.
        /// </summary>
        AlwaysShowVertScroll = 0x0200,

        /// <summary>
        /// Represents a style that disables the vertical scroll bar.
        /// </summary>
        NoVertScrollBar = 0x0400,

        /// <summary>
        /// Specifies whether the control
        /// adjusts its size to fully display items without partial clipping.
        /// </summary>
        IntegralHeight = 0x0800,

        /// <summary>
        /// Specifies that the horizontal scroll bar is displayed only when needed (Windows only).
        /// </summary>
        ShowHorzScrollWhenNeeded = 0x40000000,
    }
}