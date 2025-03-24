using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Flags restricting the font selection in the font dialog.
    /// </summary>
    /// <remarks>
    /// This enumeration supports a bitwise combination of its member values.
    /// </remarks>
    [Flags]
    public enum FontDialogRestrictSelection
    {
        /// <summary>
        /// No restriction applies.
        /// </summary>
        None = 0,

        /// <summary>
        /// To show only scalable fonts - no raster fonts.
        /// </summary>
        Scalable = 1 << 0,

        /// <summary>
        /// To show only monospaced fonts.
        /// </summary>
        FixedPitch = 1 << 1,
    }
}