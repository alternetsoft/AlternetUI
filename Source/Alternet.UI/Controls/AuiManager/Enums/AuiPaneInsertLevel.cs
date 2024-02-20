using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies insert position in <see cref="AuiManager.InsertPane"/>
    /// </summary>
    internal enum AuiPaneInsertLevel
    {
        /// <summary>
        /// Insert position is Pane.
        /// </summary>
        Pane = 0,

        /// <summary>
        /// Insert position is Row.
        /// </summary>
        Row = 1,

        /// <summary>
        /// Insert position is Dock.
        /// </summary>
        Dock = 2,
    }
}
