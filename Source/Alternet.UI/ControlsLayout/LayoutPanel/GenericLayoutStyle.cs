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
    public enum GenericLayoutStyle
    {
        /// <summary>
        /// Default layout style. Behavior depends from the control.
        /// </summary>
        Default,

        /// <summary>
        /// Do not perform any layout, use <see cref="Control.Bounds"/> to set control position.
        /// </summary>
        Native,

        /// <summary>
        /// Uses <see cref="LayoutPanel.GetDock"/> and <see cref="LayoutPanel.SetDock"/> setting
        /// for layout of the children.
        /// </summary>
        DockStyle,

        /// <summary>
        /// Default layout style implemented in the <see cref="Control"/>.
        /// </summary>
        Control,

        /// <summary>
        /// Layout as <see cref="VerticalStackPanel"/>.
        /// </summary>
        VerticalStack,

        /// <summary>
        /// Layout as <see cref="HorizontalStackPanel"/>.
        /// </summary>
        HorizontalStack,

        /// <summary>
        /// Layout as <see cref="Grid"/> control.
        /// </summary>
        Grid,
    }
}