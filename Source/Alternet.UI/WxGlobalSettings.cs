using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static properties which allow to customize WxWidgets behavior.
    /// </summary>
    public static class WxGlobalSettings
    {
        /// <summary>
        /// Gets or sets whether to disable native point and size transformations
        /// inside the library. When <see cref="InternalGraphicsTransform"/> is True,
        /// Graphics.Transform is processed inside the library. By default it is False on Windows
        /// and True on macOs and Linux.
        /// </summary>
        public static bool InternalGraphicsTransform;

        static WxGlobalSettings()
        {
            InternalGraphicsTransform = App.IsMacOS;
        }

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
