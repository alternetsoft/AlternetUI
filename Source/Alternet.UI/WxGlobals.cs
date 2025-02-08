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

        /// <summary>
        /// Contains macOs related settings.
        /// </summary>
        public static class MacOs
        {
            /// <summary>
            /// Gets or sets whether <see cref="ColorDialog"/> show method
            /// returns OK if color is changed. On macOs color dialog has no
            /// OK and CANCEL buttons, so the only way is to check for color change.
            /// </summary>
            public static bool ColorDialogAcceptIfChanged = true;
        }

        /// <summary>
        /// Contains MSW related settings.
        /// </summary>
        public static class Msw
        {

        }

        /// <summary>
        /// Contains Linux related settings.
        /// </summary>
        public static class Linux
        {
        }
    }
}
