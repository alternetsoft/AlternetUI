using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies how a control will be sized by the <see cref="LayoutPanel"/>
    /// when its AutoSize setting is enabled.
    /// </summary>
    public enum AutoSizeMode
    {
        /// <summary>
        /// The control grows or shrinks to fit its contents. The control
        /// cannot be resized manually.
        /// </summary>
        GrowAndShrink = 0,

        /// <summary>
        /// The control grows as much as necessary to fit its contents
        /// but does not shrink smaller than the value of its Size property.
        /// </summary>
        GrowOnly = 1,
    }
}
