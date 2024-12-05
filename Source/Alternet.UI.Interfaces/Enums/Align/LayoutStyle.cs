using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Defines all supported layout styles of the children controls.
    /// </summary>
    public enum LayoutStyle
    {
        /// <summary>
        /// Do not perform any layout, use <c>Control.Bounds</c> to set control position.
        /// </summary>
        None,

        /// <summary>
        /// Uses only <c>Control.Dock</c> setting for layout of the children.
        /// </summary>
        Dock,

        /// <summary>
        /// Default layout style implemented in the control.
        /// </summary>
        Basic,

        /// <summary>
        /// Layout as in vertical stack panel.
        /// </summary>
        Vertical,

        /// <summary>
        /// Layout as in horizontal stack panel.
        /// </summary>
        Horizontal,

        /// <summary>
        /// Layout as in <c>ScrollViewer</c> control.
        /// </summary>
        Scroll,
    }
}