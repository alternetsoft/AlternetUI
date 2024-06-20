using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Defines visual states of the control.
    /// </summary>
    public enum VisualControlState
    {
        /// <summary>
        /// Normal state.
        /// </summary>
        Normal,

        /// <summary>
        /// Mouse is over the control.
        /// </summary>
        Hovered,

        /// <summary>
        /// The left mouse button was clicked on the control and not yet released.
        /// </summary>
        Pressed,

        /// <summary>
        /// Control is disabled.
        /// </summary>
        Disabled,

        /// <summary>
        /// Control is focused.
        /// </summary>
        Focused,
    }
}
