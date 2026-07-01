using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.Drawing
{
    /// <summary>
    /// Specifies the kind of the font source.
    /// </summary>
    public enum FontOriginKind
    {
        /// <summary>
        /// The default font.
        /// </summary>
        Default,
        
        /// <summary>
        /// The default monospaced font.
        /// </summary>
        DefaultMono,

        /// <summary>
        /// The font is loaded from an external file.
        /// </summary>
        File,

        /// <summary>
        /// The font is installed in the system.
        /// </summary>
        System,

        /// <summary>
        /// Other kind of font source.
        /// This value is used when the font source kind is not known
        /// or does not fit into any of the other categories.
        /// </summary>
        Other,
    }
}
