using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates styles of the <see cref="StatusBarPanel"/>
    /// </summary>
    public enum StatusBarPanelStyle
    {
        /// <summary>
        /// The panel appears with the default native border.
        /// </summary>
        Normal = 0x0000,

        /// <summary>
        /// No border is painted around the panel so that it appears flat.
        /// </summary>
        Flat = 0x0001,

        /// <summary>
        /// A raised 3D border is painted around the panel.
        /// </summary>
        Raised = 0x0002,

        /// <summary>
        /// A sunken 3D border is painted around the panel.
        /// </summary>
        Sunken = 0x0003,
    }
}