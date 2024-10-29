using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the drop down control 'Closed' event.
    /// </summary>
    public class ToolStripDropDownClosedEventArgs : EventArgs
    {
        private ToolStripDropDownCloseReason closeReason;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ToolStripDropDownClosedEventArgs" /> class.
        /// </summary>
        /// <param name="reason">One of the <see cref="ToolStripDropDownCloseReason" /> values.</param>
        public ToolStripDropDownClosedEventArgs(ToolStripDropDownCloseReason reason)
        {
            closeReason = reason;
        }

        /// <summary>
        /// Gets the reason that the drop-down control closed.
        /// </summary>
        /// <returns>
        /// One of the <see cref="ToolStripDropDownCloseReason" /> values.
        /// </returns>
        public ToolStripDropDownCloseReason CloseReason => closeReason;
    }
}
