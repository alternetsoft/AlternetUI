using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Defines flags related to search operations in the web browser control.
    /// </summary>
    [Flags]
    public enum WebBrowserSearchFlags
    {
        /// <summary>
        /// No flags are specified.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Wrap search over begin/end of the document.
        /// </summary>
        Wrap = 0x0001,

        /// <summary>
        /// Search entire words.
        /// </summary>
        EntireWord = 0x0002,

        /// <summary>
        /// Match case during search.
        /// </summary>
        MatchCase = 0x0004,

        /// <summary>
        /// Higlight results.
        /// </summary>
        HighlightResult = 0x0008,

        /// <summary>
        /// Search direction is backwards.
        /// </summary>
        Backwards = 0x0010,
    }
}
