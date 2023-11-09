using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// File types in <see cref="RichTextBox"/> context.
    /// </summary>
    public enum RichTextFileType
    {
        /// <summary>
        /// Any file type.
        /// </summary>
        Any = 0,

        /// <summary>
        /// Text file type.
        /// </summary>
        Text,

        /// <summary>
        /// Xml file type.
        /// </summary>
        Xml,

        /// <summary>
        /// Html file type.
        /// </summary>
        Html,

        /// <summary>
        /// Rtf file type.
        /// </summary>
        Rtf,

        /// <summary>
        /// Pdf file type.
        /// </summary>
        Pdf,
    }
}