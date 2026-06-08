using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates image size kinds for use in controls such as toolbars and menus.
    /// </summary>
    public enum ImageSizeKind
    {
        /// <summary>
        /// Specifies that the image size should be determined using <see cref="IconSet.EffectiveSmallSystemIconSize"/>
        /// </summary>
        SmallIcon,

        /// <summary>
        /// Specifies that the image size should be determined using <see cref="IconSet.EffectiveSystemIconSize"/>
        /// </summary>
        LargeIcon,

        /// <summary>
        /// Specifies custom image size.
        /// </summary>
        Custom,
    }
}
