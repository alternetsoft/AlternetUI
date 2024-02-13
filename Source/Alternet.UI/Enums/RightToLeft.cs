using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies whether the text appears from right to left,
    /// such as when using Arabic or Hebrew fonts.</summary>
    public enum RightToLeft
    {
        /// <summary>
        /// The text reads from left to right. This is the default.
        /// </summary>
        No,

        /// <summary>
        /// The text reads from right to left.
        /// </summary>
        Yes,

        /// <summary>
        /// The direction the text read is inherited from the parent control.
        /// </summary>
        Inherit,
    }
}
