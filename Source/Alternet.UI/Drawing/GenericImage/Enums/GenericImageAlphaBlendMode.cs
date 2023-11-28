using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /// <summary>
    /// Enumerates constants for specifying alpha blending option in <see cref="GenericImage"/>.
    /// </summary>
    public enum GenericImageAlphaBlendMode
    {
        /// <summary>
        /// Overwrite the original alpha values with the ones being pasted.
        /// </summary>
        Overwrite = 0,

        /// <summary>
        /// Compose the original alpha values with the ones being pasted.
        /// </summary>
        Compose = 1,
    }
}
