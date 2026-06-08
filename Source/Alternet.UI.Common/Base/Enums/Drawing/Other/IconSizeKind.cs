using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates icon size kinds for use in controls such as toolbars and menus.
    /// </summary>
    public enum IconSizeKind
    {
        /// <summary>
        /// Specifies that the icon size should be determined using <see cref="IconSet.EffectiveSmallSystemIconSize"/>
        /// </summary>
        Small,

        /// <summary>
        /// Specifies that the icon should be determined using <see cref="IconSet.EffectiveSystemIconSize"/>
        /// </summary>
        Large,

        /// <summary>
        /// Specifies custom icon size.
        /// </summary>
        Custom,
    }
}
