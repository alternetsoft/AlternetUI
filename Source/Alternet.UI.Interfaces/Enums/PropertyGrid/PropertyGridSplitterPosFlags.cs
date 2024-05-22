using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies flags used when splitter position is set in <see cref="PropertyGrid"/>.
    /// </summary>
    [Flags]
    public enum PropertyGridSplitterPosFlags
    {
        /// <summary>
        /// Refresh flag is on.
        /// </summary>
        Refresh = 0x0001,

        /// <summary>
        /// AllPages flag is on.
        /// </summary>
        AllPages = 0x0002,

        /// <summary>
        /// FromEvent flag is on.
        /// </summary>
        FromEvent = 0x0004,

        /// <summary>
        /// FromAutoCenter flag is on.
        /// </summary>
        FromAutoCenter = 0x0008,
    }
}
