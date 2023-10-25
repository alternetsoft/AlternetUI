using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// Border flags for <see cref="Control"/>.
    public enum ControlBorderStyle
    {
        /// <summary>
        /// Default border. This is different from <see cref="None"/> as by default
        /// the controls do have border.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Displays no border, overriding the default border style for the control.
        /// </summary>
        None = 0x00200000,

        /// <summary>
        /// Displays a border suitable for a static control. Windows only.
        /// </summary>
        Static = 0x01000000,

        /// <summary>
        /// Displays a thin border around the control.
        /// </summary>
        Simple = 0x02000000,

        /// <summary>
        /// Displays a raised border.
        /// </summary>
        Raised = 0x04000000,

        /// <summary>
        /// Displays a sunken border.
        /// </summary>
        Sunken = 0x08000000,

        /// <summary>
        /// Displays a themed border where possible.
        /// </summary>
        Theme = 0x10000000,
    }
}