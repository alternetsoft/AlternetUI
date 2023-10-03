using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// This is the layout direction stored returned by <see cref="Application.LayoutDirection"/>
    /// and other methods for RTL(right-to-left) languages support.
    /// </summary>
    public enum LayoutDirection
    {
        /// <summary>
        /// Unknown layout direction. Use default.
        /// </summary>
        Default,

        /// <summary>
        /// Layout direction is left-to-right.
        /// </summary>
        LeftToRight,

        /// <summary>
        /// Layout direction is right-to-left.
        /// </summary>
        RightToLeft,
    }
}