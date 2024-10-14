using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static properties which allow to customize WxWidgest behavior.
    /// </summary>
    public static class WxGlobals
    {
        /// <summary>
        /// Gets or sets whether to disable fix for Graphics.DrawText
        /// on non-Windows operating systems. When this fix is enabled (by default), additional
        /// coordinates transformations are applied so Graphics.Transform is taken into account
        /// when Graphics.DrawText is performed.
        /// </summary>
        public static bool NoTransformForDrawText = false;
    }
}
