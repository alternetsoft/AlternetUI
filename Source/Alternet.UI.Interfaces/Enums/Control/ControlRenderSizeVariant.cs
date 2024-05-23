using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates different rendering sizes of the control.
    /// </summary>
    public enum ControlRenderSizeVariant
    {
        /// <summary>
        /// Normal size
        /// </summary>
        Normal,

        /// <summary>
        /// Smaller size (about 25 % smaller than normal)
        /// </summary>
        Small,

        /// <summary>
        /// Mini size (about 33 % smaller than normal)
        /// </summary>
        Mini,

        /// <summary>
        /// Large size (about 25 % larger than normal)
        /// </summary>
        Large,
    }
}