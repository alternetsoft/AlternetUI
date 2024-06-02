using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties which allow to change tooltips behaviour.
    /// </summary>
    public static class ToolTipFactory
    {
        private static IToolTipFactoryHandler? handler;

        /// <summary>
        /// Gets or sets handler.
        /// </summary>
        public static IToolTipFactoryHandler Handler
        {
            get => handler ??= App.Handler.CreateToolTipFactoryHandler();

            set => handler = value;
        }

        /// <summary>
        /// Sets the delay between subsequent tooltips to appear.
        /// </summary>
        /// <param name="msecs">Delay in milliseconds.</param>
        /// <returns><c>true</c> if operation is successful; <c>false</c> otherwise.</returns>
        public static bool SetReshow(long msecs)
        {
            return Handler.SetReshow(msecs);
        }

        /// <summary>
        /// Enables or disables tooltips globally.
        /// </summary>
        /// <remarks>
        /// May not be supported on all platforms (eg. wxCocoa).
        /// </remarks>
        /// <param name="flag">Enables or disables tooltips.</param>
        /// <returns><c>true</c> if operation is successful; <c>false</c> otherwise.</returns>
        public static bool SetEnabled(bool flag)
        {
            return Handler.SetEnabled(flag);
        }

        /// <summary>
        /// Sets the delay after which the tooltip disappears or how long a tooltip remains visible.
        /// </summary>
        /// <remarks>
        /// May not be supported on all platforms (eg. wxCocoa, GTK).
        /// </remarks>
        /// <param name="msecs">Delay in milliseconds.</param>
        /// <returns><c>true</c> if operation is successful; <c>false</c> otherwise.</returns>
        public static bool SetAutoPop(long msecs)
        {
            return Handler.SetAutoPop(msecs);
        }

        /// <summary>
        /// Sets the delay after which the tooltip appears.
        /// </summary>
        /// <param name="msecs">Delay in milliseconds.</param>
        /// <remarks>
        /// May not be supported on all platforms.
        /// </remarks>
        /// <returns><c>true</c> if operation is successful; <c>false</c> otherwise.</returns>
        public static bool SetDelay(long msecs)
        {
            return Handler.SetDelay(msecs);
        }

        /// <summary>
        /// Sets tooltip maximal width in pixels.
        /// </summary>
        /// <remarks>
        /// By default, tooltips are wrapped at a suitably
        /// chosen width. You can pass -1 as width to disable wrapping them completely,
        /// 0 to restore the default behaviour or an arbitrary positive value to wrap
        /// them at the given width. Notice that this function does not change the width of
        /// the tooltips created before calling it. Currently this function is Windows-only.
        /// </remarks>
        /// <param name="width">ToolTip width in pixels.</param>
        /// <returns><c>true</c> if operation is successful; <c>false</c> otherwise.</returns>
        public static bool SetMaxWidth(int width)
        {
            return Handler.SetMaxWidth(width);
        }
    }
}
