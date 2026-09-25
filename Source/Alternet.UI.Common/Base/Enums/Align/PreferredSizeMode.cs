using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the preferred size mode for a control.
    /// </summary>
    public enum PreferredSizeMode
    {
        /// <summary>
        /// The preferred size is determined by the content of the control.
        /// </summary>
        Content,

        /// <summary>
        /// The preferred size is determined by the available space in the parent container.
        /// </summary>
        Available,
    }
}
