using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the formats used with text-related methods of
    /// the <see cref="Clipboard" /> and <see cref="DataObject" /> classes.</summary>
    public enum TextDataFormat
    {
        /// <summary>
        /// Specifies the standard ANSI text format.
        /// </summary>
        Text,

        /// <summary>
        /// Specifies the standard Windows Unicode text format.
        /// </summary>
        UnicodeText,

        /// <summary>
        /// Specifies text consisting of rich text format (RTF) data.
        /// </summary>
        Rtf,

        /// <summary>
        /// Specifies text consisting of HTML data.
        /// </summary>
        Html,

        /// <summary>
        /// Specifies a comma-separated value (CSV) format, which is a common interchange
        /// format used by spreadsheets.
        /// </summary>
        CommaSeparatedValue,
    }
}
