using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Defines all <see cref="LayoutPanel"/> layout styles.
    /// </summary>
    public enum LayoutPanelKind
    {
        /// <summary>
        /// Default layout style. Uses <see cref="Control.Bounds"/> and
        /// <see cref="LayoutPanel.GetDock"/> for layout.
        /// </summary>
        Default,

        /// <summary>
        /// Do not perform any layout, use native layout of the controls.
        /// </summary>
        Native,
    }
}