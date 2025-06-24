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

        /// <summary>
        /// Text may contain new line characters.
        /// If this flag is set, new line characters are processed in the text
        /// and it is split into the lines.
        /// </summary>
        TextHasNewLineChars = 2,
    }
}