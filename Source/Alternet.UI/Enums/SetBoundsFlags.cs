using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Possible flags for <see cref="Control.SetBounds(RectD,SetBoundsFlags)"/>.
    /// </summary>
    [Flags]
    public enum SetBoundsFlags
    {
        /// <summary>
        /// Use internally-calculated width if -1.
        /// </summary>
        AutoWidth = 0x0001,

        /// <summary>
        /// Use internally-calculated height if -1
        /// </summary>
        AutoHeight = 0x0002,

        /// <summary>
        /// Use internally-calculated width and height if each is -1
        /// </summary>
        Auto = AutoWidth | AutoHeight,

        /// <summary>
        /// Ignore missing (-1) dimensions (use existing).
        /// </summary>
        UseExisting = 0x0000,

        /// <summary>
        /// Allow -1 as a valid position.
        /// </summary>
        AllowMinusOne = 0x0004,

        /// <summary>
        /// Don't do parent client adjustments (for implementation only)
        /// </summary>
        NoAdjustments = 0x0008,

        /// <summary>
        /// Change the window position even if it seems to be already correct
        /// </summary>
        Force = 0x0010,

        /// <summary>
        /// Emit size event even if size didn't change
        /// </summary>
        ForceEvent = 0x0020,
    }
}