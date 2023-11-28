using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /// <summary>
    /// Flags for PNG handling to specify which filters to use.  
    /// </summary>
    /// <remarks>
    /// See libpng (http://www.libpng.org/pub/png/libpng-1.2.5-manual.html)
    /// for more information.
    /// </remarks>
    [Flags]
    public enum GenericImagePngSetFilter
    {
        /// <summary>
        /// Filter 'None'.
        /// </summary>
        None = 0x08,

        /// <summary>
        /// Filter 'Sub'.
        /// </summary>
        Sub = 0x10,

        /// <summary>
        /// Filter 'Up'.
        /// </summary>
        Up = 0x20,

        /// <summary>
        /// Filter 'Avg'.
        /// </summary>
        Avg = 0x40,

        /// <summary>
        /// Filter 'Paeth'.
        /// </summary>
        Paeth = 0x80,

        /// <summary>
        /// Fast filters.
        /// </summary>
        FastFilters = None | Sub | Up,

        /// <summary>
        /// All filters.
        /// </summary>
        All = FastFilters | Avg | Paeth,
    }
}

