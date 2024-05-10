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
        /// Do not perform any layout, use <see cref="Control.Bounds"/> to set control position.
        /// </summary>
        None,

        /// <summary>
        /// Uses <see cref="Control.Dock"/> setting for layout of the children.
        /// </summary>
        Dock,

        /// <summary>
        /// Default layout style implemented in the <see cref="Control"/>.
        /// </summary>
        Basic,

        /// <summary>
        /// Layout as <see cref="VerticalStackPanel"/>.
        /// </summary>
        Vertical,

        /// <summary>
        /// Layout as <see cref="HorizontalStackPanel"/>.
        /// </summary>
        Horizontal,
    }
}