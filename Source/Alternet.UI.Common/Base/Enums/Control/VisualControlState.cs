using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Defines visual states of the element.
    /// </summary>
    public enum VisualControlState
    {
        /// <summary>
        /// Normal state.
        /// </summary>
        Normal,

        /// <summary>
        /// Mouse is over the element.
        /// </summary>
        Hovered,

        /// <summary>
        /// The left mouse button was clicked on the element and not yet released.
        /// </summary>
        Pressed,

        /// <summary>
        /// Element is disabled.
        /// </summary>
        Disabled,

        /// <summary>
        /// Element is focused.
        /// </summary>
        Focused,

        /// <summary>
        /// Element is selected.
        /// </summary>
        Selected,
    }
}
