using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies orientation (vertical, horizontal or both).
    /// </summary>
    [Flags]
    public enum GenericOrientation
    {
        /// <summary>
        /// Horizontal orientation.
        /// </summary>
        Horizontal = 0x0004,

        /// <summary>
        /// Vertical orientation.
        /// </summary>
        Vertical = 0x0008,

        /// <summary>
        /// Both (horizontal and vertical) orientation.
        /// </summary>
        Both = Horizontal | Vertical,
    }
}
