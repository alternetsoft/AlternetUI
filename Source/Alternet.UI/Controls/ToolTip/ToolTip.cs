using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{

    /// <summary>
    /// This class holds information about a tooltip associated with a control.
    /// </summary>
    /// <remarks>
    /// The static methods can be used to globally alter tooltips behaviour.
    /// </remarks>
    public class ToolTip : DisposableObject, IToolTip
    {
        internal ToolTip(string message)
            : base(Native.WxOtherFactory.CreateToolTip(message), false)
        {
        }

        internal ToolTip()
            : base(Native.WxOtherFactory.CreateToolTip(string.Empty), false)
        {
        }

        /// <summary>
        /// Gets or sets text of the tooltip.
        /// </summary>
        public string Text
        {
            get
            {
                return Native.WxOtherFactory.ToolTipGetTip(Handle);
            }

            set
            {
                Native.WxOtherFactory.ToolTipSetTip(Handle, value ?? string.Empty);
            }
        }

        // Set the delay between subsequent tooltips to appear.
        public static void SetReshow(long msecs)
        {
            Native.WxOtherFactory.ToolTipSetReshow(msecs);
        }

        // Enable or disable tooltips globally.
        // May not be supported on all platforms (eg. wxCocoa).
        public static void SetEnabled(bool flag)
        {
            Native.WxOtherFactory.ToolTipEnable(flag);
        }

        // Set the delay after which the tooltip disappears or how long a tooltip remains visible.
        // May not be supported on all platforms (eg. wxCocoa, GTK).
        public static void SetAutoPop(long msecs)
        {
            Native.WxOtherFactory.ToolTipSetAutoPop(msecs);
        }

        // Set the delay after which the tooltip appears.
        // May not be supported on all platforms.
        public static void SetDelay(long msecs)
        {
            Native.WxOtherFactory.ToolTipSetDelay(msecs);
        }

        // Set tooltip maximal width in pixels. By default, tooltips are wrapped at a suitably
        // chosen width. You can pass -1 as width to disable wrapping them completely,
        // 0 to restore the default behaviour or an arbitrary positive value to wrap
        // them at the given width. Notice that this function does not change the width of
        // the tooltips created before calling it. Currently this function is wxMSW-only.
        public static void ToolTipSetMaxWidth(int width)
        {
            Native.WxOtherFactory.ToolTipSetMaxWidth(width);
        }

        internal IntPtr GetWindow()
        {
            return Native.WxOtherFactory.ToolTipGetWindow(Handle);
        }

        /// <inheritdoc/>
        protected override void DisposeUnmanagedResources()
        {
            Native.WxOtherFactory.DeleteToolTip(Handle);
        }
    }
}
