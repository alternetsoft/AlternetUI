using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates possible log item kinds.
    /// </summary>
    /// <remarks>
    /// This enumeration supports a bitwise combination of its member values.
    /// </remarks>
    [Flags]
    public enum LogItemKindFlags
    {
        /// <summary>
        /// No flags are specified.
        /// </summary>
        None = 0,

        /// <summary>
        /// 'Information' flag.
        /// </summary>
        Information = 1,

        /// <summary>
        /// 'Error' flag.
        /// </summary>
        Error = 2,

        /// <summary>
        /// 'Warning' flag.
        /// </summary>
        Warning = 4,

        /// <summary>
        /// 'Other' flag.
        /// </summary>
        Other = 8,
    }
}
