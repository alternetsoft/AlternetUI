using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /// <summary>
    /// Known colors categories enumeration.
    /// </summary>
    [Flags]
    public enum KnownColorCategory
    {
        /// <summary>
        /// Standard colors (like Black, White, etc.).
        /// </summary>
        Standard = 1,

        /// <summary>
        /// System-defined colors (like ActiveBorder, ControlLight, etc.).
        /// </summary>
        System = 2,

        /// <summary>
        /// Web colors (like BlanchedAlmond, SkyBlue, etc.).
        /// </summary>
        Web = 4,

        /// <summary>
        /// Other colors.
        /// </summary>
        Other = 8,
    }
}
