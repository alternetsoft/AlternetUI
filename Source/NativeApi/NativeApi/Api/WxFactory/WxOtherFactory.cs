#pragma warning disable
using ApiCommon;
using Alternet.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace NativeApi.Api
{
    // https://docs.wxwidgets.org/3.2/classwx_rich_tool_tip.html
    // https://docs.wxwidgets.org/3.2/classwx_tool_tip.html
    public class WxOtherFactory
    {
        // RichToolTip

        // Ctor must specify the tooltip title and main message, additional
        // attributes can be set later.
        public static IntPtr CreateRichToolTip(string title, string message) => default;
        public static void DeleteRichToolTip(IntPtr handle) { }

        // Set the background color: if two colors are specified, the background
        // is drawn using a gradient from top to bottom, otherwise a single solid
        // color is used.
        public static void RichToolTipSetBkColor(IntPtr handle, Color color, Color endColor) { }

        // Set the small icon to show: either one of the standard information/
        // warning/error ones (the question icon doesn't make sense for a tooltip)
        // or a custom icon.
        public static void RichToolTipSetIcon(IntPtr handle, ImageSet? bitmapBundle) { }
        public static void RichToolTipSetIcon2(IntPtr handle, int icon) { } // wxICON_* in defs.h

        // Set timeout after which the tooltip should disappear, in milliseconds.
        // By default the tooltip is hidden after system-dependent interval of time
        // elapses but this method can be used to change this or also disable
        // hiding the tooltip automatically entirely by passing 0 in this parameter
        // (but doing this can result in native version not being used).
        // Optionally specify a show delay.
        public static void RichToolTipSetTimeout(IntPtr handle, uint milliseconds, uint millisecondsShowdelay = 0) { }

        // Choose the tip kind, possibly none. By default the tip is positioned
        // automatically, as if wxTipKind_Auto was used.
        public static void RichToolTipSetTipKind(IntPtr handle, int tipKind) { } // wxTipKind

        // Set the title text font. By default it's emphasized using the font style
        // or colour appropriate for the current platform.
        public static void RichToolTipSetTitleFont(IntPtr handle, Font? font) {}

        // Show the tooltip for the given window and optionally a specified area.
        public static void RichToolTipShowFor(IntPtr handle, IntPtr window, Int32Rect rect) { }

        // ToolTip

        public static IntPtr CreateToolTip(string tip) => default;
        public static void DeleteToolTip(IntPtr handle) { }
        public static string ToolTipGetTip(IntPtr handle) => default;
        public static IntPtr ToolTipGetWindow(IntPtr handle) => default;
        public static void ToolTipSetTip(IntPtr handle, string tip) { }

        // Enable or disable tooltips globally. 
        // May not be supported on all platforms (eg. wxCocoa).
        public static void ToolTipEnable(bool flag) { }

        // Set the delay after which the tooltip disappears or how long a tooltip remains visible. 	
        // May not be supported on all platforms (eg. wxCocoa, GTK).
        public static void ToolTipSetAutoPop(long msecs) { }

        // Set the delay after which the tooltip appears. 
        // May not be supported on all platforms.
        public static void ToolTipSetDelay(long msecs) { }

        // Set tooltip maximal width in pixels. By default, tooltips are wrapped at a suitably
        // chosen width. You can pass -1 as width to disable wrapping them completely,
        // 0 to restore the default behaviour or an arbitrary positive value to wrap
        // them at the given width. Notice that this function does not change the width of
        // the tooltips created before calling it. Currently this function is wxMSW-only.
        public static void ToolTipSetMaxWidth(int width) { }

        // Set the delay between subsequent tooltips to appear. 
        public static void ToolTipSetReshow(long msecs) { }
    }
}

/*


 
*/