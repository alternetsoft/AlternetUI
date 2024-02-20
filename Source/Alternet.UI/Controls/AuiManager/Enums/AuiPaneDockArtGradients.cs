using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// These are the possible gradient dock art settings for panes
    /// managed by the <see cref="AuiManager"/>.
    /// </summary>
    internal enum AuiPaneDockArtGradients
    {
        /// <summary>
        /// No gradient on the captions, in other words a solid color.
        /// </summary>
        None = 0,

        /// <summary>
        /// Vertical gradient on the captions, in other words a gradal
        /// change in colors from top to bottom.
        /// </summary>
        Vertical = 1,

        /// <summary>
        /// Horizontal gradient on the captions, in other words a gradual change
        /// in colors from left to right.
        /// </summary>
        Horizontal = 2,
    }
}