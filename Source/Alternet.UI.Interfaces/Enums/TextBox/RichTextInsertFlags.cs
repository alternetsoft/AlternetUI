﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Flags for object insertion in the rich text box control.
    /// </summary>
    /// <remarks>
    /// This enumeration supports a bitwise combination of its member values.
    /// </remarks>
    [Flags]
    public enum RichTextInsertFlags
    {
        /// <summary>
        /// No flags are specified.
        /// </summary>
        None = 0x00,

        /// <summary>
        /// Insert with previous paragraph style.
        /// </summary>
        WithPreviousParagraphStyle = 0x01,

        /// <summary>
        /// Insert interactive.
        /// </summary>
        Interactive = 0x02,
    }
}
