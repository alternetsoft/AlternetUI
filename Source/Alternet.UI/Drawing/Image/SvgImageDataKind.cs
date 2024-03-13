﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /// <summary>
    /// Enumerates known data parameter types for <see cref="SvgImage"/> constructor.
    /// </summary>
    public enum SvgImageDataKind
    {
        /// <summary>
        /// Type of the data string is autodetected.
        /// </summary>
        Auto,

        /// <summary>
        /// Data string is url.
        /// </summary>
        Url,
    }
}
