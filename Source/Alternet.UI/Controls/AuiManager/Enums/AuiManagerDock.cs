using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies <see cref="IAuiPaneInfo"/> pane dock position.
    /// </summary>
    internal enum AuiManagerDock
    {
        /// <summary>
        /// Clears the pane dock position.
        /// </summary>
        None = 0,

        /// <summary>
        /// Sets the pane dock position to the top of the frame.
        /// </summary>
        Top = 1,

        /// <summary>
        /// Sets the pane dock position to the right of the frame.
        /// </summary>
        Right = 2,

        /// <summary>
        /// Sets the pane dock position to the bottom of the frame.
        /// </summary>
        Bottom = 3,

        /// <summary>
        /// Sets the pane dock position to the left of the frame.
        /// </summary>
        Left = 4,

        /// <summary>
        /// Sets the pane dock position to the center of the frame.
        /// </summary>
        Center = 5,
    }
}
