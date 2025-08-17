using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Defines visual states of the element as flags.
    /// Supports a bitwise combination of its member values.
    /// </summary>
    [Flags]
    public enum VisualControlStates
    {
        /// <summary>
        /// Represents the absence of any specific value or state.
        /// </summary>
        None = 0,

        /// <summary>
        /// Mouse is over the element.
        /// </summary>
        Hovered = 1,

        /// <summary>
        /// The left mouse button was clicked on the element and not yet released.
        /// </summary>
        Pressed = 2,

        /// <summary>
        /// Element is disabled.
        /// </summary>
        Disabled = 4,

        /// <summary>
        /// Element is focused.
        /// </summary>
        Focused = 8,

        /// <summary>
        /// Element is selected.
        /// </summary>
        Selected = 16,
    }
}
