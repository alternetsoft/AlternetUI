using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Possible pane button states for <see cref="AuiNotebook"/> and
    /// <see cref="AuiToolbar"/>.
    /// </summary>
    [Flags]
    internal enum AuiPaneButtonState
    {
        /// <summary>
        /// Normal button state.
        /// </summary>
        Normal = 0,

        /// <summary>
        /// Hovered button state.
        /// </summary>
        Hover = 1 << 1,

        /// <summary>
        /// Pressed button state.
        /// </summary>
        Pressed = 1 << 2,

        /// <summary>
        /// Disabled button state.
        /// </summary>
        Disabled = 1 << 3,

        /// <summary>
        /// Hidden button state.
        /// </summary>
        Hidden = 1 << 4,

        /// <summary>
        /// Checked button state.
        /// </summary>
        Checked = 1 << 5,
    }
}
