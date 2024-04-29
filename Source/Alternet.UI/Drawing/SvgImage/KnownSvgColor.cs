﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates known svg image color identifiers.
    /// </summary>
    public enum KnownSvgColor
    {
        /// <summary>
        /// Normal image color.
        /// </summary>
        Normal,

        /// <summary>
        /// Disabled image color.
        /// </summary>
        Disabled,

        /// <summary>
        /// Error image color.
        /// </summary>
        Error,

        /// <summary>
        /// Information image color.
        /// </summary>
        Information,

        /// <summary>
        /// Warning image color.
        /// </summary>
        Warning,

        /// <summary>
        /// Highlight Text color.
        /// </summary>
        HighlightText,

        /// <summary>
        /// Maximum value in the enumeration.
        /// </summary>
        MaxValue = HighlightText,
    }
}
