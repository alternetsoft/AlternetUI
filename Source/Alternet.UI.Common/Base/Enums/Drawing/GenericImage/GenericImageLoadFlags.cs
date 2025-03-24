using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /// <summary>
    /// Enumerates image load flags.
    /// </summary>
    /// <remarks>
    /// This enumeration supports a bitwise combination of its member values.
    /// </remarks>
    [Flags]
    public enum GenericImageLoadFlags
    {
        /// <summary>
        /// Determines if the non-fatal (i.e.not preventing the file from being loaded completely)
        /// problems should result in the calls to log function. It is recommended to customize
        /// handling of these warnings, but if such warnings should be completely suppressed,
        /// clearing this flag provides a simple way to do it.
        /// </summary>
        Verbose = 1,
    }
}
