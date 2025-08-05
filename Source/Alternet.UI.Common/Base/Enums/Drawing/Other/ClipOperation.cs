using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.Drawing
{
    /// <summary>
    /// Enumerates the logical operations that can be performed when combining two regions.
    /// </summary>
    public enum ClipOperation
    {
        /// <summary>
        /// Subtracts the op region from the first region.
        /// </summary>
        Difference = 0,

        /// <summary>
        /// Intersects the two regions.
        /// </summary>
        Intersect = 1,
    }
}
