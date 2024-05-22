using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates log item kinds.
    /// </summary>
    public enum LogItemKind
    {
        /// <summary>
        /// Information.
        /// </summary>
        Information,

        /// <summary>
        /// Error item.
        /// </summary>
        Error,

        /// <summary>
        /// Warning item.
        /// </summary>
        Warning,

        /// <summary>
        /// Other item without any image.
        /// </summary>
        Other,
    }
}
