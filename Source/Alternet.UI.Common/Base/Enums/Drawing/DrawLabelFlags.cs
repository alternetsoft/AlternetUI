using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.Drawing
{
    /// <summary>
    /// Enumerates flags that can be used to customize label painting.
    /// </summary>
    [Flags]
    public enum DrawLabelFlags
    {
        /// <summary>
        /// Text may contain html bold tags.
        /// </summary>
        TextHasBold = 1,
    }
}
