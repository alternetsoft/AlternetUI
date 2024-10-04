using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Defines visual states of the control as flags.
    /// Supports a bitwise combination of its member values.
    /// </summary>
    [Flags]
    public enum VisualControlStates
    {
        /// <summary>
        /// Mouse is over the control.
        /// </summary>
        Hovered = 1,

        /// <summary>
        /// The left mouse button was clicked on the control and not yet released.
        /// </summary>
        Pressed = 2,

        /// <summary>
        /// Control is disabled.
        /// </summary>
        Disabled = 4,

        /// <summary>
        /// Control is focused.
        /// </summary>
        Focused = 8,
    }
}
